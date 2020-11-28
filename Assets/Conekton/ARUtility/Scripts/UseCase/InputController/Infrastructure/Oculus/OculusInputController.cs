#if UNITY_ANDROID && PLATFORM_OCULUS
using UnityEngine;
using Zenject;
using Conekton.ARUtility.Player.Domain;
using Conekton.ARUtility.Input.Domain;

namespace Conekton.ARUtility.Input.Infrastructure
{
    public class OculusInputController : IInputController
    {
        [Inject] private IPlayer _player = null;

        private readonly Vector3 VECTOR3_FORWARD = Vector3.forward;

        private OVRInput.Controller GetOculusTouchType(ControllerType type)
        {
            if (type == ControllerType.Left)
            {
                return OVRInput.Controller.LTouch;
            }

            return OVRInput.Controller.RTouch;
        }

        private OVRInput.Controller GetOculusControllerType(ControllerType type)
        {
            if (type == ControllerType.Left)
            {
                return OVRInput.Controller.LHand;
            }

            return OVRInput.Controller.RHand;
        }

        private OVRInput.Button GetOculusButtonType(ButtonType type)
        {
            switch (type)
            {
                case ButtonType.One:
                    return OVRInput.Button.One;

                case ButtonType.Two:
                    return OVRInput.Button.Two;

                case ButtonType.Three:
                    return OVRInput.Button.Three;

                case ButtonType.Four:
                    return OVRInput.Button.Four;

                default:
                    return OVRInput.Button.One;
            }
        }

        protected virtual Vector3 GetPosition(ControllerType type)
        {
            OVRInput.Controller controllerType = GetOculusControllerType(type);
            return _player.Root.localToWorldMatrix.MultiplyPoint(OVRInput.GetLocalControllerPosition(controllerType));
        }

        bool IInputController.IsTriggerDown(ControllerType type)
        {
            OVRInput.Controller touchType = GetOculusTouchType(type);
            return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, touchType);
        }

        bool IInputController.IsTriggerUp(ControllerType type)
        {
            OVRInput.Controller touchType = GetOculusTouchType(type);
            return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, touchType);
        }

        bool IInputController.IsTouch(ControllerType type)
        {
            OVRInput.Controller touchType = GetOculusTouchType(type);
            return OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, touchType);
        }

        bool IInputController.IsTouchDown(ControllerType type)
        {
            OVRInput.Controller touchType = GetOculusTouchType(type);
            return OVRInput.GetDown(OVRInput.Touch.PrimaryThumbstick, touchType);
        }

        bool IInputController.IsTouchUp(ControllerType type)
        {
            OVRInput.Controller touchType = GetOculusTouchType(type);
            return OVRInput.GetUp(OVRInput.Touch.PrimaryThumbstick, touchType);
        }

        Vector3 IInputController.GetForward(ControllerType type) => (this as IInputController).GetRotation(type) * VECTOR3_FORWARD;

        Vector3 IInputController.GetPosition(ControllerType type)
        {
            return GetPosition(type);
        }

        Quaternion IInputController.GetRotation(ControllerType type)
        {
            OVRInput.Controller controllerType = GetOculusControllerType(type);
            return _player.Root.rotation * OVRInput.GetLocalControllerRotation(controllerType);
        }

        Vector2 IInputController.GetTouch(ControllerType type)
        {
            OVRInput.Controller touchType = GetOculusTouchType(type);
            return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, touchType);
        }

        void IInputController.TriggerHapticVibration(ControllerType type, HapticData data)
        {
            OVRInput.Controller controller = GetOculusControllerType(type);
            OVRInput.SetControllerVibration(data.Frequency, data.Amplitude, controller);
        }

        bool IInputController.IsDown(ControllerType controllerType, ButtonType buttonType)
        {
            OVRInput.Controller touchType = GetOculusTouchType(controllerType);
            OVRInput.Button button = GetOculusButtonType(buttonType);
            return OVRInput.GetDown(button, touchType);
        }

        bool IInputController.IsUp(ControllerType controllerType, ButtonType buttonType)
        {
            OVRInput.Controller touchType = GetOculusTouchType(controllerType);
            OVRInput.Button button = GetOculusButtonType(buttonType);
            return OVRInput.GetUp(button, touchType);
        }
    }
}
#endif