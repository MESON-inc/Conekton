using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_WSA
using UnityEngine.XR.WSA;
using System.Runtime.InteropServices;

using Zenject;

using Conekton.ARUtility.Input.Domain;

namespace Conekton.ARUtility.Input.Infrastructure
{
    public class MRTKInputController : IInputController
    {
        private bool isTriggerDown = false;
        private bool isTriggerUp = false;
        private bool isTouch = false;
        private bool isTouchDown = false;
        private bool isTouchUp = false;
        private Vector3 position = Vector3.zero;
        private Vector3 forward = Vector3.forward;
        private Quaternion rotation = Quaternion.identity;
        private Vector2 touch = Vector2.one;

        bool IInputController.IsTriggerDown => isTriggerDown;

        bool IInputController.IsTriggerUp => isTriggerUp;

        bool IInputController.IsTouch => isTouch;

        bool IInputController.IsTouchDown => isTouchDown;

        bool IInputController.IsTouchUp => isTouchUp;

        Vector3 IInputController.Position => position;

        Vector3 IInputController.Forward => forward;

        Quaternion IInputController.Rotation => rotation;

        Vector2 IInputController.Touch => touch;

        void IInputController.TriggerHapticVibration(HapticData data)
        {
            throw new NotImplementedException();
        }

        bool IInputController.IsDown(ButtonType type)
        {
            throw new NotImplementedException();
        }

        bool IInputController.IsUp(ButtonType type)
        {
            throw new NotImplementedException();
        }
    }
}
#endif

