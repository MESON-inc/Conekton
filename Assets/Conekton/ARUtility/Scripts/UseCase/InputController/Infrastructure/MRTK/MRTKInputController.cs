using UnityEngine;
using Conekton.ARUtility.Input.Domain;
using Zenject;
#if UNITY_WSA
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine.XR.WSA.Input;

namespace Conekton.ARUtility.Input.Infrastructure
{
    public class MRTKInputController : IInputController, IMixedRealityGestureHandler, ITickable
    {
        private const float FINGER_DISTANCE_CLICK = 0.03f;

        private bool _isTriggerDown = false;
        private bool _isTriggerUp = false;
        private bool _isTriggerHolding = false;
        private Vector3 _position = Vector3.zero;
        private Vector3 _forward = Vector3.forward;
        private Quaternion _rotation = Quaternion.identity;

        bool IInputController.IsTriggerDown => _isTriggerDown;

        bool IInputController.IsTriggerUp => _isTriggerUp;

        bool IInputController.IsTouch => false; // non implement

        bool IInputController.IsTouchDown => false; // non implement

        bool IInputController.IsTouchUp => false; // non implement

        Vector3 IInputController.Position => _position;

        Vector3 IInputController.Forward => _forward;

        Quaternion IInputController.Rotation => _rotation;

        Vector2 IInputController.Touch => Vector2.zero;

        void IInputController.TriggerHapticVibration(HapticData data)
        {
            // do nothing.
        }

        bool IInputController.IsDown(ButtonType type)
        {
            return false;
        }

        bool IInputController.IsUp(ButtonType type)
        {
            return false;
        }

        void IMixedRealityGestureHandler.OnGestureStarted(InputEventData eventData)
        {
            Debug.Log($"OnGestureUpdated [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");
            _isTriggerDown = true;
        }

        void IMixedRealityGestureHandler.OnGestureUpdated(InputEventData eventData)
        {
            Debug.Log($"OnGestureUpdated [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");
            _isTriggerDown = false;
            _isTriggerUp = false;
        }

        void IMixedRealityGestureHandler.OnGestureCompleted(InputEventData eventData)
        {
            Debug.Log($"OnGestureCompleted [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");
            _isTriggerUp = true;
        }

        void IMixedRealityGestureHandler.OnGestureCanceled(InputEventData eventData)
        {
            // do nothing.
        }

        void ITickable.Tick()
        {
            UpdateInputController();
        }

        void UpdateInputController()
        {
            IMixedRealityHand jointedHand = HandJointUtils.FindHand(Handedness.Right);
            if (jointedHand == null)
            {
                return;
            }

            UpdateControllerPose(jointedHand);
            UpdateTriggerState(jointedHand);
        }

        void UpdateControllerPose(IMixedRealityHand jointedHand)
        {
            bool isGetWrist = jointedHand.TryGetJoint(TrackedHandJoint.Wrist, out MixedRealityPose wristPose);
            if (isGetWrist)
            {
                _position = wristPose.Position;
                _rotation = wristPose.Rotation;
                _forward = wristPose.Forward;
            }
        }

        void UpdateTriggerState(IMixedRealityHand jointedHand)
        {
            bool isGetIndexFinger =
                jointedHand.TryGetJoint(TrackedHandJoint.IndexTip, out MixedRealityPose indexFingerPose);
            bool isGetThumbFinger =
                jointedHand.TryGetJoint(TrackedHandJoint.ThumbTip, out MixedRealityPose thumbFingerPose);
            if (isGetIndexFinger && isGetThumbFinger)
            {
                float fingerDistance = Vector3.Distance(indexFingerPose.Position, thumbFingerPose.Position);
                bool isClickFinger = fingerDistance < FINGER_DISTANCE_CLICK;

                // update isTriggerDown
                if (!_isTriggerHolding && isClickFinger)
                {
                    _isTriggerHolding = true;
                    _isTriggerDown = true;
                }
                else
                {
                    _isTriggerDown = false;
                }

                // update isTriggerUp
                if (_isTriggerHolding && !isClickFinger)
                {
                    _isTriggerHolding = false;
                    _isTriggerUp = true;
                }
                else
                {
                    _isTriggerUp = false;
                }
            }
        }
    }
}
#endif