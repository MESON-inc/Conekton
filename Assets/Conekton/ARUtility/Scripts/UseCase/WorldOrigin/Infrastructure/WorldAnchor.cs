using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.UseCase.WorldOrigin.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARUtility.UseCase.WorldOrigin.Infrastructure
{
    public class WorldAnchor : MonoBehaviour, IWorldAnchor
    {
        private IWorldOrigin _worldOrigin = null;

        private Vector3 _offsetPos = Vector3.zero;
        private Quaternion _offsetRot = Quaternion.identity;

        [Inject]
        public void SetOrigin(IWorldOrigin origin)
        {
            _worldOrigin = origin;
        }

        private void Start()
        {
            _offsetPos = _worldOrigin.Transform.worldToLocalMatrix.MultiplyPoint3x4(transform.position);
            _offsetRot = Quaternion.Inverse(_worldOrigin.Transform.rotation) * transform.rotation;
        }

        private void Update()
        {
            Vector3 pos = _worldOrigin.Transform.localToWorldMatrix.MultiplyPoint3x4(_offsetPos);
            Quaternion rot = _worldOrigin.Transform.rotation * _offsetRot;

            transform.SetPositionAndRotation(pos, rot);
        }
    }
}