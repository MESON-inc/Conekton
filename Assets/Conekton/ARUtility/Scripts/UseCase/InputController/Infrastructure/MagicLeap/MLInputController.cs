using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if PLATFORM_LUMIN
using UnityEngine.XR.MagicLeap;

using Zenject;

using Conekton.ARUtility.Input.Domain;

namespace Conekton.ARUtility.Input.Infrastructure
{
    public class MLInputController : IInputController, ITickable, ILateDisposable
    {
        private readonly Vector3 VECTOR3_FORWARD = Vector3.forward;

        private bool _hasInitialized = false;

        private bool _isTriggerDown = false;
        private bool _isTriggerUp = false;

        private bool _triggerDownEvent = false;
        private bool _triggerUpEvent = false;

        private bool _lastTriggerDown = false;
        private bool _lastTriggerUp = false;

        private MLInput.Controller _inputController = null;
        private MLInput.Controller.FeedbackPatternVibe _pattern = MLInput.Controller.FeedbackPatternVibe.ForceDown;
        private MLInput.Controller.FeedbackIntensity _intensity = MLInput.Controller.FeedbackIntensity.Medium;

        bool IInputController.IsTriggerDown => _isTriggerDown;

        bool IInputController.IsTriggerUp => _isTriggerUp;

        Vector3 IInputController.Forward => (this as IInputController).Rotation * VECTOR3_FORWARD;

        Vector3 IInputController.Position => (_inputController == null) ? Vector3.zero : _inputController.Position;

        Quaternion IInputController.Rotation => (_inputController == null) ? Quaternion.identity : _inputController.Orientation;

        Vector2 IInputController.Touch => (_inputController == null) ? Vector2.zero :
                                       _inputController.Touch1Active ? (Vector2)_inputController.Touch1PosAndForce :
                                                                       Vector2.zero;
        bool IInputController.IsTouch => UnityEngine.Input.touchCount > 0;
        bool IInputController.IsTouchDown => false;
        bool IInputController.IsTouchUp => false;

        void ILateDisposable.LateDispose()
        {
            MLInput.OnTriggerDown -= HandleOnTriggerDown;
            MLInput.OnTriggerUp -= HandleOnTriggerUp;
            MLInput.OnControllerConnected -= HandleOnControllerConnected;
        }

        void ITickable.Tick()
        {
            if (!_hasInitialized)
            {
                InitializeMLInputIfNeeded();
                return;
            }

            TriggerCheck();
        }

        private void InitializeMLInputIfNeeded()
        {
            if (_hasInitialized)
            {
                return;
            }

            if (!MLInput.IsStarted)
            {
                return;
            }

            MLResult result = MLInput.Start();

            if (!result.IsOk)
            {
                Debug.LogError("MLInput won't start.");
                return;
            }

            _hasInitialized = true;

            MLInput.OnTriggerDown += HandleOnTriggerDown;
            MLInput.OnTriggerUp += HandleOnTriggerUp;
            MLInput.OnControllerConnected += HandleOnControllerConnected;
        }

        private void TriggerCheck()
        {
            _isTriggerDown = !_lastTriggerDown && _triggerDownEvent;
            _lastTriggerDown = _triggerDownEvent;

            _isTriggerUp = !_lastTriggerUp && _triggerUpEvent;
            _lastTriggerUp = _triggerUpEvent;

            _triggerDownEvent = false;
            _triggerUpEvent = false;
        }


        private void HandleOnControllerConnected(byte controllerId)
        {
            _inputController = MLInput.GetController(controllerId);
        }

        private void HandleOnTriggerDown(byte controllerId, float triggerValue)
        {
            _triggerDownEvent = true;
        }

        private void HandleOnTriggerUp(byte controllerId, float triggerValue)
        {
            _triggerUpEvent = true;
        }

        void IInputController.TriggerHapticVibration(HapticData data)
        {
            // NOTE: It should calculate haptics intensity by data.
            _inputController.StartFeedbackPatternVibe(_pattern, _intensity);
        }

        bool IInputController.IsDown(ButtonType type)
        {
            return false;
        }

        bool IInputController.IsUp(ButtonType type)
        {
            return false;
        }
    }
}
#endif

