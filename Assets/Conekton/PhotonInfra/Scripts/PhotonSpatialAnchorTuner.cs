using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Zenject;

using Conekton.ARUtility.Player.Domain;
using Conekton.ARMultiplayer.SpatialAnchor.Domain;
using Conekton.ARMultiplayer.SpatialAnchor.Application;
using Conekton.ARMultiplayer.PersistentCoordinate.Domain;

namespace Conekton.ARMultiplayer.SpatialAnchor.Presentation
{
    public class PhotonSpatialAnchorTuner : MonoBehaviourPunCallbacks, ISpatialAnchorTuner, IPunObservable
    {
        private IPlayer _player = null;
        private ISpatialAnchor _spatialAnchor = null;
        private IPersistentCoordinateService _pcaService = null;
        private ISpatialAnchorService _anchorService = null;
        private SpatialAnchorUtility _spatialAnchorUtility = null;

        private Dictionary<PCAID, Pose> _remotePCADatabase = new Dictionary<PCAID, Pose>();

        void ISpatialAnchorTuner.BindAnchor(ISpatialAnchor anchor)
        {
            _spatialAnchor = anchor;
        }

        [Inject]
        private void Inject(IPlayer player, ISpatialAnchorService anchorService, IPersistentCoordinateService pcaService, SpatialAnchorUtility spatialAnchorUtility)
        {
            _spatialAnchorUtility = spatialAnchorUtility;
            _player = player;
            _pcaService = pcaService;
            _anchorService = anchorService;
            _anchorService.RegisterTuner(this, photonView);
        }

        #region ### MonoBehaviour ###
        private void Update()
        {
            UpdateAnchor();
        }
        #endregion ### MonoBehaviour ###

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                SendData(stream);
            }
            else
            {
                ReceiveData(stream);
            }
        }

        private void SendData(PhotonStream stream)
        {
            int count = 20;
            IPCA[] pcas = _pcaService.GetNearPCA(_player.Position, count);

            stream.SendNext(pcas.Length);

            foreach (var p in pcas)
            {
                stream.SendNext(p.ID);
                Pose pose = _spatialAnchorUtility.ConvertToLocalPose(p, transform);
                stream.SendNext(pose.position);
                stream.SendNext(pose.rotation);
            }
        }

        private void ReceiveData(PhotonStream stream)
        {
            _remotePCADatabase.Clear();

            int count = (int)stream.ReceiveNext();

            for (int i = 0; i < count; i++)
            {
                PCAID id = (PCAID)stream.ReceiveNext();
                Vector3 pos = (Vector3)stream.ReceiveNext();
                Quaternion rot = (Quaternion)stream.ReceiveNext();

                _remotePCADatabase.Add(id, new Pose
                {
                    position = pos,
                    rotation = rot,
                });
            }

            Pose adaptPose = _anchorService.GetAnchorPose(_remotePCADatabase);
            transform.SetPositionAndRotation(adaptPose.position, adaptPose.rotation);
        }

        private void UpdateAnchor()
        {
            if (_spatialAnchor == null)
            {
                return;
            }

            _spatialAnchor.SetPose(new Pose
            {
                position = transform.position,
                rotation = transform.rotation,
            });
        }
    }
}
