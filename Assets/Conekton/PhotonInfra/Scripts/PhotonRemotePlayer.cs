using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Zenject;

using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using Conekton.ARMultiplayer.Avatar.Domain;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Infrastructure
{
    public class PhotonRemotePlayer : MonoBehaviourPunCallbacks, IRemotePlayer, IPunObservable, IPunInstantiateMagicCallback 
    {
        private Pose _headPose = default;
        private Pose _leftHandPose = default;
        private Pose _rightHandPose = default;

        private IMultiplayerNetworkSystem _networkSystem = null;
        private IAvatarController _targetAvatarController = null;
        private bool _hasTargetAvatarController = false;
        private PlayerID _playerID = PlayerID.NoSet;

        private void OnDestroy()
        {
            PhotonNetwork.Destroy(gameObject);
            OnDestroyingRemotePlayer?.Invoke(this);
        }

        [Inject]
        private void Inject(IMultiplayerNetworkSystem networkSystem)
        {
            _networkSystem = networkSystem;
            
            if (photonView.IsMine)
            {
                _networkSystem.CreateRemotePlayerLocalPlayer(this, photonView);
            }
            else
            {
                _networkSystem.CreatedRemotePlayer(this, photonView);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // For the head.
                Pose headPose = GetHeadPose();
                stream.SendNext(headPose.position);
                stream.SendNext(headPose.rotation);

                // For the left hand.
                Pose leftHandPose = GetHandPose(AvatarPoseType.Left);
                stream.SendNext(leftHandPose.position);
                stream.SendNext(leftHandPose.rotation);

                // For the right hand.
                Pose rightHandPose = GetHandPose(AvatarPoseType.Right);
                stream.SendNext(rightHandPose.position);
                stream.SendNext(rightHandPose.rotation);
            }
            else
            {
                // For the head.
                Vector3 headPos = (Vector3)stream.ReceiveNext();
                Quaternion headRot = (Quaternion)stream.ReceiveNext();
                SetHeadPose(new Pose(headPos, headRot));

                Vector3 lhandPos = (Vector3)stream.ReceiveNext();
                Quaternion lhandRot = (Quaternion)stream.ReceiveNext();
                SetHandPose(new Pose(lhandPos, lhandRot), AvatarPoseType.Left);

                Vector3 rhandPos = (Vector3)stream.ReceiveNext();
                Quaternion rhandRot = (Quaternion)stream.ReceiveNext();
                SetHandPose(new Pose(rhandPos, rhandRot), AvatarPoseType.Right);
            }
        }
        
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            object[] instantiationData = info.photonView.InstantiationData;

            NetworkArgs args = null;
            if (instantiationData?.Length > 0)
            {
                args = instantiationData[0] as NetworkArgs;
            }
            _networkSystem.ReceivedRemotePlayerCustomData(this, args);
        }

        #region ### for IAvatarController interface ###
        Pose IAvatarController.GetHeadPose() => GetHeadPose();

        Pose IAvatarController.GetHandPose(AvatarPoseType type) => GetHandPose(type);
        #endregion ### for IAvatarController interface ###

        #region ### for IRemotePlayer interface ###
        public event DestroyingRemotePlayerEvent OnDestroyingRemotePlayer;

        PlayerID IRemotePlayer.PlayerID
        {
            get => _playerID;
            set => SetPlayerID(value);
        }

        void IRemotePlayer.SetTargetAvatarController(IAvatarController avatarController)
        {
            _hasTargetAvatarController = avatarController != null;
            _targetAvatarController = avatarController;
        }
        #endregion ### for IRemotePlayer interface ###

        private void SetPlayerID(PlayerID playeriD)
        {
            name = $"RemotePlayer-[{playeriD.ID}]";
            _playerID = playeriD;
        }

        private Pose GetHeadPose()
        {
            if (_hasTargetAvatarController)
            {
                return _targetAvatarController.GetHeadPose();
            }
            else
            {
                return _headPose;
            }
        }

        private Pose GetHandPose(AvatarPoseType type)
        {
            if (_hasTargetAvatarController)
            {
                return _targetAvatarController.GetHandPose(type);
            }

            switch (type)
            {
                case AvatarPoseType.Left:
                    return _leftHandPose;

                case AvatarPoseType.Right:
                    return _rightHandPose;
            }

            return default;
        }

        private void SetHeadPose(Pose pose)
        {
            if (_hasTargetAvatarController)
            {
                return;
            }

            _headPose = pose;
        }

        private void SetHandPose(Pose pose, AvatarPoseType type)
        {
            if (_hasTargetAvatarController)
            {
                return;
            }

            switch (type)
            {
                case AvatarPoseType.Left:
                    _leftHandPose = pose;
                    break;

                case AvatarPoseType.Right:
                    _rightHandPose = pose;
                    break;
            }
        }
    }
}
