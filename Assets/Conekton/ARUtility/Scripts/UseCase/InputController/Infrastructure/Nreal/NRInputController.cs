#if UNITY_ANDROID && PLATFORM_NREAL
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NRKernal;
using Zenject;

using Conekton.ARUtility.Input.Domain;
using Conekton.ARUtility.Input.Application;
using ControllerType = Conekton.ARUtility.Input.Domain.ControllerType;

namespace Conekton.ARUtility.Input.Infrastructure
{
    public class NRInputController : IInputController
    {
        [Inject] private NRInputSettings _inputSettings = null;

        private readonly Vector3 VECTOR3_FORWARD = Vector3.forward;

        bool IInputController.IsTriggerDown(ControllerType type) => NRInput.GetButtonDown(ControllerButton.TRIGGER);

        bool IInputController.IsTriggerUp(ControllerType type) => NRInput.GetButtonUp(ControllerButton.TRIGGER);

        bool IInputController.IsTouchDown(ControllerType type) => NRInput.GetButtonDown(ControllerButton.TRIGGER);

        bool IInputController.IsTouchUp(ControllerType type) => NRInput.GetButtonDown(ControllerButton.TRIGGER);

        bool IInputController.IsTouch(ControllerType type) => NRInput.GetButton(ControllerButton.TRIGGER);

        Vector3 IInputController.GetForward(ControllerType type) => (this as IInputController).GetRotation(type) * VECTOR3_FORWARD;

        Vector3 IInputController.GetPosition(ControllerType type) => NRInput.AnchorsHelper.GetAnchor(ControllerAnchorEnum.RightLaserAnchor).position;

        Quaternion IInputController.GetRotation(ControllerType type) => NRInput.AnchorsHelper.GetAnchor(ControllerAnchorEnum.RightLaserAnchor).rotation;

        Vector2 IInputController.Touch(ControllerType type) => NRInput.GetTouch();

        void IInputController.TriggerHapticVibration(ControllerType type, HapticData data)
        {
            NRInput.TriggerHapticVibration(data.DurationSeconds, data.Frequency, data.Amplitude);
        }

        bool IInputController.IsDown(ControllerType controllerType, ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.One:
                    return NRInput.GetButtonDown(_inputSettings.One);

                case ButtonType.Two:
                    return NRInput.GetButtonDown(_inputSettings.Two);

                case ButtonType.Three:
                    return NRInput.GetButtonDown(_inputSettings.Three);

                case ButtonType.Four:
                    return NRInput.GetButtonDown(_inputSettings.Four);

                default:
                    return false;
            }
        }

        bool IInputController.IsUp(ControllerType controllerType, ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.One:
                    return NRInput.GetButtonUp(_inputSettings.One);

                case ButtonType.Two:
                    return NRInput.GetButtonUp(_inputSettings.Two);

                case ButtonType.Three:
                    return NRInput.GetButtonUp(_inputSettings.Three);

                case ButtonType.Four:
                    return NRInput.GetButtonUp(_inputSettings.Four);

                default:
                    return false;
            }
        }
    }
}
#endif
