#if UNITY_ANDROID && PLATFORM_NREAL
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NRKernal;
using Zenject;

using Conekton.ARUtility.Input.Domain;
using Conekton.ARUtility.Input.Application;

namespace Conekton.ARUtility.Input.Infrastructure
{
    public class NRInputController : IInputController
    {
        [Inject] private NRInputSettings _inputSettings = null;

        private readonly Vector3 VECTOR3_FORWARD = Vector3.forward;

        bool IInputController.IsTriggerDown => NRInput.GetButtonDown(ControllerButton.TRIGGER);

        bool IInputController.IsTriggerUp => NRInput.GetButtonUp(ControllerButton.TRIGGER);

        bool IInputController.IsTouchDown => NRInput.GetButtonDown(ControllerButton.TRIGGER);

        bool IInputController.IsTouchUp => NRInput.GetButtonDown(ControllerButton.TRIGGER);

        bool IInputController.IsTouch => NRInput.GetButton(ControllerButton.TRIGGER);

        Vector3 IInputController.Forward => (this as IInputController).Rotation * VECTOR3_FORWARD;

        Vector3 IInputController.Position => NRInput.AnchorsHelper.GetAnchor(ControllerAnchorEnum.RightLaserAnchor).position;

        Quaternion IInputController.Rotation => NRInput.AnchorsHelper.GetAnchor(ControllerAnchorEnum.RightLaserAnchor).rotation;

        Vector2 IInputController.Touch => NRInput.GetTouch();

        void IInputController.TriggerHapticVibration(HapticData data)
        {
            NRInput.TriggerHapticVibration(data.DurationSeconds, data.Frequency, data.Amplitude);
        }

        bool IInputController.IsDown(ButtonType type)
        {
            switch (type)
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

        bool IInputController.IsUp(ButtonType type)
        {
            switch (type)
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

