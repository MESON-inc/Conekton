using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARUtility.Player.Domain;
using Conekton.ARUtility.Input.Domain;

namespace Conekton.ARUtility.Input.Infrastructure
{
    public class MobileInputController : IInputController
    {
        [Inject] private IPlayer _player = null;

        bool IInputController.IsTriggerDown(ControllerType type) => IsTriggerDown(type);
        bool IInputController.IsTriggerUp(ControllerType type) => IsTriggerUp(type);
        bool IInputController.IsTouch(ControllerType type) => UnityEngine.Input.touchCount > 0;
        bool IInputController.IsTouchDown(ControllerType type) => IsTouchDown(type);
        bool IInputController.IsTouchUp(ControllerType type) => IsTouchUp(type);

        private bool IsTouchDown(ControllerType type)
        {
            if (UnityEngine.Input.touchCount == 0)
            {
                return false;
            }

            Touch touch = UnityEngine.Input.GetTouch(0);
            return touch.phase == TouchPhase.Began;
        }

        private bool IsTouchUp(ControllerType type)
        {
            if (UnityEngine.Input.touchCount == 0)
            {
                return false;
            }

            Touch touch = UnityEngine.Input.GetTouch(0);
            return touch.phase == TouchPhase.Ended;
        }

        private bool IsTriggerDown(ControllerType type)
        {
            if (UnityEngine.Input.touchCount == 0)
            {
                return false;
            }

            Touch touch = UnityEngine.Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                return true;
            }

            return false;
        }

        private bool IsTriggerUp(ControllerType type)
        {
            if (UnityEngine.Input.touchCount == 0)
            {
                return false;
            }

            Touch touch = UnityEngine.Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                return true;
            }

            return false;
        }

        Vector3 IInputController.GetForward(ControllerType type) => _player.CameraRig.transform.forward;

        Vector3 IInputController.GetPosition(ControllerType type) => _player.Position;

        Quaternion IInputController.GetRotation(ControllerType type) => _player.Rotation;

        Vector2 IInputController.GetTouch(ControllerType type) => Vector2.zero;

        void IInputController.TriggerHapticVibration(ControllerType type, HapticData data)
        {
            // do nothing.
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

