using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using Conekton.ARUtility.HandSystemUseCase.Domain;

namespace Conekton.ARUtility.HandSystemUseCase.Infrastructure
{
    public class StubHand : MonoBehaviour, IHand
    {
        public HandType HandType { get; set; } = HandType.Left;

        private float _pinchStrength = 0;

        public float PinchStrength
        {
            get => _pinchStrength;
            set
            {
                _pinchStrength = Mathf.Clamp01(value);
            }
        }

        #region ### Interface IHand ###
        public bool IsTracked => true;

        public bool GetFingerIsPinching(FingerType fingerType)
        {
            return _pinchStrength > 0.5f;
        }

        public float GetFingerStrength(FingerType fingerType)
        {
            return _pinchStrength;
        }

        public HandType GetHandType()
        {
            return HandType;
        }

        public bool TryGetPalmPositionAndNormal(out Vector3 position, out Vector3 normal)
        {
            position = transform.position;
            normal = transform.forward;
            return true;
        }

        public Pose GetPose()
        {
            return new Pose(GetPosition(), GetRotation());
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public Quaternion GetRotation()
        {
            return transform.rotation;
        }
        #endregion ### Interface IHand ###
    }
}

