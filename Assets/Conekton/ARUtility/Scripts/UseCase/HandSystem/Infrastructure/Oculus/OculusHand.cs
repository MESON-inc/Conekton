using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using Conekton.ARUtility.HandSystemUseCase.Domain;
using Conekton.ARUtility.HandSystemUseCase.Application;

namespace Conekton.ARUtility.HandSystemUseCase.Infrastructure
{
    [RequireComponent(typeof(OculusHandPalm))]
    public class OculusHand : MonoBehaviour
#if UNITY_ANDROID && PLATFORM_OCULUS
        , IHand
#endif
    {
        [SerializeField] private OculusHandPalm _oculusHandPalm = null;

#if UNITY_ANDROID && PLATFORM_OCULUS
        private OVRHand _hand = null;

        private HandType _handType = HandType.Left;
        private HandType HandTypeInfo
        {
            get
            {
                if (_hand == null)
                {
                    ResolveHandType();
                }

                return _handType;
            }
        }

        #region ### MonoBehaviour ###
        private void Awake()
        {
            _ = HandTypeInfo;
        }
        #endregion ### MonoBehaviour ###

        public bool GetFingerIsPinching(FingerType fingerType)
        {
            OVRHand.HandFinger finger = ConvertFingerType(fingerType);
            return _hand.GetFingerIsPinching(finger);
        }

        public float GetFingerStrength(FingerType fingerType)
        {
            OVRHand.HandFinger finger = ConvertFingerType(fingerType);
            return _hand.GetFingerPinchStrength(finger);
        }

        public HandType GetHandType()
        {
            return HandTypeInfo;
        }

        public bool TryGetPalmPositionAndNormal(out Vector3 position, out Vector3 normal)
        {
            return _oculusHandPalm.TryGetPositionAndNormal(out position, out normal);
        }

        public Pose GetPose()
        {
            return new Pose(GetPosition(), GetRotation());
        }

        public Vector3 GetPosition()
        {
            return _hand.PointerPose.position;
        }

        public Quaternion GetRotation()
        {
            return _hand.PointerPose.rotation;
        }

        private static OVRHand.HandFinger ConvertFingerType(FingerType fingerType)
        {
            switch (fingerType)
            {
                case FingerType.Thumb:
                    return OVRHand.HandFinger.Thumb;

                case FingerType.Index:
                    return OVRHand.HandFinger.Index;

                case FingerType.Middle:
                    return OVRHand.HandFinger.Middle;

                case FingerType.Ring:
                    return OVRHand.HandFinger.Ring;

                case FingerType.Pinky:
                    return OVRHand.HandFinger.Pinky;

                default:
                    return OVRHand.HandFinger.Index;
            }
        }

        private void ResolveHandType()
        {
            _hand = GetComponent<OVRHand>();

            FieldInfo info = _hand.GetType().GetField("HandType", BindingFlags.NonPublic | BindingFlags.Instance);
            if (info is null)
            {
                return;
            }
            
            OVRHand.Hand handType = (OVRHand.Hand)info.GetValue(_hand);

            switch (handType)
            {
                case OVRHand.Hand.HandLeft:
                    _handType = HandType.Left;
                    break;
                
                case OVRHand.Hand.HandRight:
                    _handType = HandType.Right;
                    break;
            }
        }
#endif
    }
}

