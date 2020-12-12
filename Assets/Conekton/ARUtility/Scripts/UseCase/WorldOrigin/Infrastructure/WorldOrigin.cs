using System;
using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.UseCase.WorldOrigin.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARUtility.UseCase.WorldOrigin.Infrastructure
{
    public class WorldOrigin : MonoBehaviour, IWorldOrigin
    {
        public event MovedEvent OnMoved;
        
        public Transform Transform => transform;
        
        private List<IWorldMarker> _worldMarkers = new List<IWorldMarker>();

        private void Update()
        {
            UpdatePose();
        }

        public void AddAnchor(IWorldMarker marker)
        {
            if (_worldMarkers.Contains(marker))
            {
                return;
            }
            
            _worldMarkers.Add(marker);
            
            UpdatePose();
        }

        public void Clear()
        {
            _worldMarkers.Clear();
        }

        private void UpdatePose()
        {
            if (_worldMarkers.Count == 0)
            {
                return;
            }
            
            Pose pose = GetAveragePose();
            
            transform.SetPositionAndRotation(pose.position, pose.rotation);
            
            OnMoved?.Invoke(this);
        }

        private Pose GetAveragePose()
        {
            if (_worldMarkers.Count == 0)
            {
                return default;
            }
            
            Vector3 resultPos = Vector3.zero;
            Quaternion firstRot = _worldMarkers[0].RelativePose.rotation;
            float x = 0, y = 0, z = 0, w = 0;
            
            foreach (var a in _worldMarkers)
            {
                resultPos += a.RelativePose.position;

                float dot = Quaternion.Dot(firstRot, a.RelativePose.rotation);
                float multi = dot > 0 ? 1f : -1f;

                x += a.RelativePose.rotation.x * multi;
                y += a.RelativePose.rotation.y * multi;
                z += a.RelativePose.rotation.z * multi;
                w += a.RelativePose.rotation.w * multi;
            }

            resultPos /= _worldMarkers.Count;

            float k = 1f / Mathf.Sqrt(x * x + y * y + z * z + w * w);
            Quaternion resultRot = new Quaternion(x * k, y * k, z * k, w * k);
            
            return new Pose(resultPos, resultRot);
        }
    }
}