using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARMultiplayer.PersistentCoordinate.Domain;

namespace Conekton.ARMultiplayer.SpatialAnchor.Application
{
    public class SpatialAnchorUtility
    {
        private Transform _transformHelper = null;

        public SpatialAnchorUtility()
        {
            _transformHelper = new GameObject("TransformHelper").transform;
            _transformHelper.hideFlags = HideFlags.HideInHierarchy;
        }

        public Pose ConvertToLocalPose(IPCA pca, Transform baseTrans)
        {
            _transformHelper.SetPositionAndRotation(pca.Position, pca.Rotation);

            Vector3 offsetPos = _transformHelper.InverseTransformPoint(baseTrans.position);
            Quaternion offsetRot = Quaternion.Inverse(_transformHelper.rotation) * Quaternion.LookRotation(baseTrans.forward);

            return new Pose
            {
                position = offsetPos,
                rotation = offsetRot,
            };
        }

        public Pose ConvertToWorldPose(IPCA pca, Pose pose)
        {
            _transformHelper.SetPositionAndRotation(pca.Position, pca.Rotation);

            return new Pose
            {
                position = _transformHelper.TransformPoint(pose.position),
                rotation = _transformHelper.rotation * pose.rotation,
            };
        }
    }
}

