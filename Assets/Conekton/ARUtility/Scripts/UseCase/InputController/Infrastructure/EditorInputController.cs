using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARUtility.Input.Domain;

namespace Conekton.ARUtility.Input.Infrastructure
{
    public class EditorInputController : MonoBehaviour, IInputController
    {
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

        bool IInputController.IsTriggerDown => UnityEngine.Input.GetKeyDown(_triggerDownKey);
        bool IInputController.IsTriggerUp => UnityEngine.Input.GetKeyDown(_triggerUpKey);
        bool IInputController.IsTouch => UnityEngine.Input.GetKey(_touchDownKey);
        bool IInputController.IsTouchDown => UnityEngine.Input.GetKeyDown(_touchDownKey);
        bool IInputController.IsTouchUp => UnityEngine.Input.GetKeyDown(_touchUpKey);

        Vector3 IInputController.Forward => _rotation * VECTOR3_FORWARD;

        Vector3 IInputController.Position => _position;

        Quaternion IInputController.Rotation => _rotation;

        Vector2 IInputController.Touch => Vector2.zero;

        void IInputController.TriggerHapticVibration(HapticData data)
        {
            // do nothing.
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

