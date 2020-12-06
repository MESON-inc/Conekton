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
        
        [Inject] private IWorldOriginService _worldOriginService = null;
        
        public Transform Transform => transform;
        
        private IWorldAnchor _anchor = null;
        private Vector3 _offsetPosition = Vector3.zero;
        private Quaternion _offsetRotation = Quaternion.identity;

        private void Awake()
        {
            _worldOriginService.Register(this);
        }

        public void SetAnchor(IWorldAnchor anchor)
        {
            _anchor = anchor;
            _offsetPosition = _anchor.Transform.worldToLocalMatrix.MultiplyPoint3x4(transform.position);
            _offsetRotation = Quaternion.Inverse(_anchor.Transform.rotation) * transform.rotation;
        }
        
        public void MoveToPose(Pose pose)
        {
            _anchor.Transform.SetPositionAndRotation(pose.position, pose.rotation);

            Vector3 pos = _anchor.Transform.localToWorldMatrix.MultiplyPoint3x4(_offsetPosition);
            Quaternion rot = pose.rotation * _offsetRotation;
            transform.SetPositionAndRotation(pos, rot);
            
            OnMoved?.Invoke(this);
        }
    }
}