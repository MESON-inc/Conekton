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
        
        private List<IWorldAnchor> _worldAnchors = new List<IWorldAnchor>();

        public void AddAnchor(IWorldAnchor anchor)
        {
            if (_worldAnchors.Contains(anchor))
            {
                return;
            }
            
            _worldAnchors.Add(anchor);
            UpdatePose();
        }

        public void Clear()
        {
            _worldAnchors.Clear();
        }

        private void UpdatePose()
        {
            Pose pose = GetAveragePose();
            
            transform.SetPositionAndRotation(pose.position, pose.rotation);
            
            OnMoved?.Invoke(this);
        }

        private Pose GetAveragePose()
        {
            Vector3 resultPos = Vector3.zero;
            Quaternion resultRot = Quaternion.identity;
            
            foreach (var a in _worldAnchors)
            {
                resultPos += a.RelativePose.position;
            }

            resultPos /= _worldAnchors.Count;
            
            return new Pose(resultPos, resultRot);
        }
    }
}