#if UNITY_ANDROID && PLATFORM_OCULUS
using UnityEngine;

using Zenject;

using Conekton.ARUtility.Player.Domain;
using Conekton.ARUtility.Input.Domain;

namespace Conekton.ARUtility.Input.Infrastructure
{
    public class OculusInputController3DoF : IInputController
    {
        [Inject] private IPlayer _player = null;

        private readonly Vector3 VECTOR3_FORWARD = Vector3.forward;
        private readonly Vector3 PIVOT_OFFSET = new Vector3(0.1f, -0.35f, 0f);
        private readonly float HAND_DISTANCE = 0.3f;

        bool IInputController.IsTriggerDown => OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        bool IInputController.IsTriggerUp => OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        bool IInputController.IsTouch => OVRInput.Get(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.RTouch);

        bool IInputController.IsTouchDown => OVRInput.GetDown(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.RTouch);

        bool IInputController.IsTouchUp => OVRInput.GetUp(OVRInput.Touch.PrimaryThumbstick, OVRInput.Controller.RTouch);

        Vector3 IInputController.Forward => (this as IInputController).Rotation * VECTOR3_FORWARD;

        Vector3 IInputController.Position
        {
            get
            {
                Vector3 pivot = _player.Position + PIVOT_OFFSET;
                Vector3 handPos = (_player.Rotation * VECTOR3_FORWARD) * HAND_DISTANCE;
                return pivot + handPos;
            }
        }

        Quaternion IInputController.Rotation => _player.Root.rotation * OVRInput.GetLocalControllerRotation(OVRInput.Controller.RHand);

        Vector2 IInputController.Touch => OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

        void IInputController.TriggerHapticVibration(HapticData data)
        {
            OVRInput.SetControllerVibration(data.Frequency, data.Amplitude);
        }
    }
}
#endif

