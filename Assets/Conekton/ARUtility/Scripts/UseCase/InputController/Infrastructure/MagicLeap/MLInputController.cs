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
    public class MLInputController : IInputController, IInitializable, ITickable, ILateDisposable
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

        bool IInputController.IsTriggerDown(ControllerType type) => _isTriggerDown;

        bool IInputController.IsTriggerUp(ControllerType type) => _isTriggerUp;

        Vector3 IInputController.GetForward(ControllerType type) => (this as IInputController).GetRotation(type) * VECTOR3_FORWARD;

        Vector3 IInputController.GetPosition(ControllerType type) => (_inputController == null) ? Vector3.zero : _inputController.Position;

        Quaternion IInputController.GetRotation(ControllerType type) => (_inputController == null) ? Quaternion.identity : _inputController.Orientation;

        Vector2 IInputController.Touch(ControllerType type) => (_inputController == null) ? Vector2.zero :
                                       _inputController.Touch1Active ? (Vector2)_inputController.Touch1PosAndForce :
                                                                       Vector2.zero;
        bool IInputController.IsTouch(ControllerType type) => UnityEngine.Input.touchCount > 0;
        bool IInputController.IsTouchDown(ControllerType type) => false;
        bool IInputController.IsTouchUp(ControllerType type) => false;

        void IInitializable.Initialize()
        {
            InitializeMLInputIfNeeded();
        }

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

        /// <summary>
        /// Try get controller reference.
        /// 
        /// This is because "MLInput.OnControllerConnected" event won't invoke.
        /// I think why Zenject.IInitialize timing is too late to register an event.
        /// So this method try to get a reference cotroller at the initializing.
        /// </summary>
        private void TryGetController()
        {
            if (_inputController != null)
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
                return;
            }

            _inputController = MLInput.GetController(0);
        }

        private void InitializeMLInputIfNeeded()
        {
            if (_hasInitialized)
            {
                return;
            }

            if (!MLInput.IsStarted)
            {
                Debug.Log("<<<< MLInput hasn't started >>>>");
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

            TryGetController();

            Debug.Log(_inputController);

            Debug.Log("<<<< Sccessed to initialize MLInput >>>>");
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

            Debug.Log($"<<<< Got a MLInput.Controller {_inputController} >>>>");
        }

        private void HandleOnTriggerDown(byte controllerId, float triggerValue)
        {
            _triggerDownEvent = true;
        }

        private void HandleOnTriggerUp(byte controllerId, float triggerValue)
        {
            _triggerUpEvent = true;
        }

        void IInputController.TriggerHapticVibration(ControllerType type, HapticData data)
        {
            // NOTE: It should calculate haptics intensity by data.
            _inputController.StartFeedbackPatternVibe(_pattern, _intensity);
        }

        bool IInputController.IsDown(ControllerType controllerType, ButtonType buttonType)
        {
            return false;
        }

        bool IInputController.IsUp(ControllerType controllerType, ButtonType buttonType)
        {
            return false;
        }
    }
}
#endif
