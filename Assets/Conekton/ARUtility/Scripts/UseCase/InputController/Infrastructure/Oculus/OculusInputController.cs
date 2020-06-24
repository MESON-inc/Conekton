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

        bool IInputController.IsTriggerDown => OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        bool IInputController.IsTriggerUp => OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        bool IInputController.IsTouch => OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.RTouch);

        bool IInputController.IsTouchDown => OVRInput.GetDown(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.RTouch);

        bool IInputController.IsTouchUp => OVRInput.GetUp(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.RTouch);

        Vector3 IInputController.Forward => (this as IInputController).Rotation * VECTOR3_FORWARD;

        Vector3 IInputController.Position => _player.Root.localToWorldMatrix.MultiplyPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RHand));

        Quaternion IInputController.Rotation => _player.Root.rotation * OVRInput.GetLocalControllerRotation(OVRInput.Controller.RHand);

        Vector2 IInputController.Touch => OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

        void IInputController.TriggerHapticVibration(HapticData data)
        {
            OVRInput.SetControllerVibration(data.Frequency, data.Amplitude);
        }

        bool IInputController.IsDown(ButtonType type)
        {
            switch (type)
            {
                case ButtonType.One:
                    return OVRInput.GetDown(OVRInput.Button.One);

                case ButtonType.Two:
                    return OVRInput.GetDown(OVRInput.Button.Two);

                case ButtonType.Three:
                    return OVRInput.GetDown(OVRInput.Button.Three);

                case ButtonType.Four:
                    return OVRInput.GetDown(OVRInput.Button.Four);

                default:
                    return false;
            }
        }

        bool IInputController.IsUp(ButtonType type)
        {
            switch (type)
            {
                case ButtonType.One:
                    return OVRInput.GetUp(OVRInput.Button.One);

                case ButtonType.Two:
                    return OVRInput.GetUp(OVRInput.Button.Two);

                case ButtonType.Three:
                    return OVRInput.GetUp(OVRInput.Button.Three);

                case ButtonType.Four:
                    return OVRInput.GetUp(OVRInput.Button.Four);

                default:
                    return false;
            }
        }
    }
}
#endif

