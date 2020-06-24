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

        bool IInputController.IsTriggerDown => IsTriggerDown();
        bool IInputController.IsTriggerUp => IsTriggerUp();
        bool IInputController.IsTouch => UnityEngine.Input.touchCount > 0;
        bool IInputController.IsTouchDown => IsTouchDown();
        bool IInputController.IsTouchUp => IsTouchUp();

        private bool IsTouchDown()
        {
            if (UnityEngine.Input.touchCount == 0)
            {
                return false;
            }

            Touch touch = UnityEngine.Input.GetTouch(0);
            return touch.phase == TouchPhase.Began;
        }

        private bool IsTouchUp()
        {
            if (UnityEngine.Input.touchCount == 0)
            {
                return false;
            }

            Touch touch = UnityEngine.Input.GetTouch(0);
            return touch.phase == TouchPhase.Ended;
        }

        private bool IsTriggerDown()
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

        private bool IsTriggerUp()
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

        Vector3 IInputController.Forward => _player.CameraRig.transform.forward;

        Vector3 IInputController.Position => _player.Position;

        Quaternion IInputController.Rotation => _player.Rotation;

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
    }
}

