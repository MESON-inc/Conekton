using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARUtility.Input.Domain;
using Conekton.ARUtility.Input.Application;

namespace Conekton.ARUtility.Input.Infrastructure
{
    public class EditorInputController : MonoBehaviour, IInputController
    {
        [Inject] private EditorInputSettings _inputSettings = null;

        [SerializeField] private KeyCode _triggerDownKey = KeyCode.D;
        [SerializeField] private KeyCode _triggerUpKey = KeyCode.U;

        [Header("For position control")]
        [SerializeField] private KeyCode _leftKey = KeyCode.LeftArrow;
        [SerializeField] private KeyCode _rightKey = KeyCode.RightArrow;
        [SerializeField] private KeyCode _forwardKey = KeyCode.UpArrow;
        [SerializeField] private KeyCode _backKey = KeyCode.DownArrow;
        [SerializeField] private KeyCode _upKey = KeyCode.Q;
        [SerializeField] private KeyCode _downKey = KeyCode.E;

        [Header("For rotation control")]
        [SerializeField] private KeyCode _leftTurnKey = KeyCode.R;
        [SerializeField] private KeyCode _rightTurnKey = KeyCode.T;

        [Header("For touch control")]
        [SerializeField] private KeyCode _touchDownKey = KeyCode.Y;
        [SerializeField] private KeyCode _touchUpKey = KeyCode.G;

        [SerializeField] private float _speed = 0.01f;
        [SerializeField] private float _rotSpeed = 1f;

        private Vector3 _position = Vector3.zero;
        private Quaternion _rotation = Quaternion.identity;
        private readonly Vector3 VECTOR3_FORWARD = Vector3.forward;

        bool IInputController.IsTriggerDown(ControllerType type) => UnityEngine.Input.GetKeyDown(_triggerDownKey);
        bool IInputController.IsTriggerUp(ControllerType type) => UnityEngine.Input.GetKeyDown(_triggerUpKey);
        bool IInputController.IsTouch(ControllerType type) => UnityEngine.Input.GetKey(_touchDownKey);
        bool IInputController.IsTouchDown(ControllerType type) => UnityEngine.Input.GetKeyDown(_touchDownKey);
        bool IInputController.IsTouchUp(ControllerType type) => UnityEngine.Input.GetKeyDown(_touchUpKey);

        Vector3 IInputController.GetForward(ControllerType type) => _rotation * VECTOR3_FORWARD;

        Vector3 IInputController.GetPosition(ControllerType type) => _position;

        Quaternion IInputController.GetRotation(ControllerType type) => _rotation;

        Vector2 IInputController.Touch(ControllerType type) => Vector2.zero;

        void IInputController.TriggerHapticVibration(ControllerType type, HapticData data)
        {
            // do nothing.
        }

        bool IInputController.IsDown(ControllerType controllerType, ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.One:
                    return UnityEngine.Input.GetKeyDown(_inputSettings.One);

                case ButtonType.Two:
                    return UnityEngine.Input.GetKeyDown(_inputSettings.Two);

                case ButtonType.Three:
                    return UnityEngine.Input.GetKeyDown(_inputSettings.Three);

                case ButtonType.Four:
                    return UnityEngine.Input.GetKeyDown(_inputSettings.Four);

                default:
                    return false;
            }
        }

        bool IInputController.IsUp(ControllerType controllerType, ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.One:
                    return UnityEngine.Input.GetKeyUp(_inputSettings.One);

                case ButtonType.Two:
                    return UnityEngine.Input.GetKeyUp(_inputSettings.Two);

                case ButtonType.Three:
                    return UnityEngine.Input.GetKeyUp(_inputSettings.Three);

                case ButtonType.Four:
                    return UnityEngine.Input.GetKeyUp(_inputSettings.Four);

                default:
                    return false;
            }
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKey(_leftKey))
            {
                _position += Vector3.left * _speed;
            }

            if (UnityEngine.Input.GetKey(_rightKey))
            {
                _position += Vector3.right * _speed;
            }

            if (UnityEngine.Input.GetKey(_forwardKey))
            {
                _position += Vector3.forward * _speed;
            }

            if (UnityEngine.Input.GetKey(_backKey))
            {
                _position += Vector3.back * _speed;
            }

            if (UnityEngine.Input.GetKey(_upKey))
            {
                _position += Vector3.up * _speed;
            }

            if (UnityEngine.Input.GetKey(_downKey))
            {
                _position += Vector3.down * _speed;
            }

            if (UnityEngine.Input.GetKey(_rightTurnKey))
            {
                _rotation *= Quaternion.Euler(new Vector3(0, _rotSpeed, 0));
            }

            if (UnityEngine.Input.GetKey(_leftTurnKey))
            {
                _rotation *= Quaternion.Euler(new Vector3(0, -_rotSpeed, 0));
            }
        }
    }
}

