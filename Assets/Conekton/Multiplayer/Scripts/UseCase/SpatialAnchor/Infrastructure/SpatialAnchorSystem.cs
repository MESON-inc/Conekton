using System.Collections.Generic;
using System.Linq;
using Zenject;

using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using Conekton.ARMultiplayer.PersistentCoordinate.Domain;
using Conekton.ARMultiplayer.SpatialAnchor.Application;
using Conekton.ARMultiplayer.SpatialAnchor.Domain;
using UnityEngine;

namespace Conekton.ARMultiplayer.SpatialAnchor.Infrastructure
{
    public class SpatialAnchorSystem : ISpatialAnchorSystem
    {
        [Inject] private ISpatialAnchorRepository _anchorRepository = null;
        [Inject] private ISpatialAnchorTunerRepository _anchorTunerRepository = null;
        [Inject] private IPersistentCoordinateService _pcaService = null;
        [Inject] private SpatialAnchorUtility _spatialAnchorUtility = null;

        public event CreatedAnchorEvent OnCreatedAnchor;

        private int _index = 0;
        private Dictionary<PlayerID, SpatialAnchorID> _database = new Dictionary<PlayerID, SpatialAnchorID>();

        ISpatialAnchor ISpatialAnchorSystem.GetOrCreateAnchor(PlayerID playerID) => GetOrCreateAnchor(playerID);
        ISpatialAnchorTuner ISpatialAnchorSystem.CreateTuner() => CreateTuner();

        void ISpatialAnchorSystem.RegisterTuner(ISpatialAnchorTuner tuner, PlayerID playerID) => RegisterTuner(tuner, playerID);
        
        Pose ISpatialAnchorSystem.GetAnchorPose(Dictionary<PCAID, Pose> comparePCAData)
        {
            var localPCA = _pcaService.GetAllPCA();

            var filterdLocalPCA = localPCA
                .Where(p => comparePCAData.ContainsKey(p.ID))
                .ToArray();

            if (filterdLocalPCA.Length == 0)
            {
                return default;
            }

            Vector3 averagePos = Vector3.zero;
            float x = 0, y = 0, z = 0, w = 0;

            Quaternion firstRot = filterdLocalPCA[0].Rotation;
            bool foundFirstRot = false;

            foreach (var p in filterdLocalPCA)
            {
                Pose offsetPose = _spatialAnchorUtility.ConvertToWorldPose(p, comparePCAData[p.ID]);

                averagePos += offsetPose.position;

                if (!foundFirstRot)
                {
                    firstRot = offsetPose.rotation;
                    foundFirstRot = true;
                }

                float dot = Quaternion.Dot(firstRot, offsetPose.rotation);
                float multi = dot > 0f ? 1f : -1f;

                x += offsetPose.rotation.x * multi;
                y += offsetPose.rotation.y * multi;
                z += offsetPose.rotation.z * multi;
                w += offsetPose.rotation.w * multi;
            }

            averagePos /= filterdLocalPCA.Length;

            float k = 1f / Mathf.Sqrt(x * x + y * y + z * z + w * w);
            Quaternion averageRot = new Quaternion(x * k, y * k, z * k, w * k);

            return new Pose(averagePos, averageRot);
        }

        private void RegisterTuner(ISpatialAnchorTuner tuner, PlayerID playerID)
        {
            ISpatialAnchor anchor = GetOrCreateAnchor(playerID);
            tuner.BindAnchor(anchor);
        }

        private ISpatialAnchor GetOrCreateAnchor(PlayerID playerID)
        {
            if (_database.ContainsKey(playerID))
            {
                return _anchorRepository.Find(_database[playerID]);
            }

            return CreateAnchor(playerID);
        }

        private ISpatialAnchor CreateAnchor(PlayerID playerID)
        {
            SpatialAnchorID anchorID = CreateNewID(playerID);
            _database.Add(playerID, anchorID);

            ISpatialAnchor anchor = _anchorRepository.Create(anchorID);

            OnCreatedAnchor?.Invoke(anchor);

            return anchor;
        }

        private ISpatialAnchorTuner CreateTuner()
        {
            return _anchorTunerRepository.Create();
        }

        private SpatialAnchorID CreateNewID(PlayerID playerID)
        {
            int id = _index++;

            return new SpatialAnchorID
            {
                ID = id,
                PlayerID = playerID,
            };
        }
    }
}
