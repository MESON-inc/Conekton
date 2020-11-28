using UnityEngine;

namespace Conekton.ARUtility.Input.Domain
{
    /// <summary>
    /// This is a data structure.
    /// This will be used for "TriggerHapticVibration" interface.
    /// </summary>
    public struct HapticData
    {
        public float DurationSeconds;
        public float Frequency;
        public float Amplitude;

        static public HapticData Default => new HapticData { DurationSeconds = 0.1f, Frequency = 200f, Amplitude = 0.8f, };
    }

    /// <summary>
    /// ButtonType enum means to use that it can't abstract button types such as OVRInput.Button.One.
    /// </summary>
    public enum ButtonType
    {
        One,
        Two,
        Three,
        Four,
    }

    /// <summary>
    /// ControllerType enum means to get either left or right handed.
    /// </summary>
    public enum ControllerType
    {
        Left,
        Right,
    }

    /// <summary>
    /// IInputController provides ways to access each platform controller.
    /// </summary>
    public interface IInputController
    {
        bool IsTriggerDown(ControllerType type);
        bool IsTriggerUp(ControllerType type);
        bool IsTouch(ControllerType type);
        bool IsTouchDown(ControllerType type);
        bool IsTouchUp(ControllerType type);
        Vector3 GetPosition(ControllerType type);
        Vector3 GetForward(ControllerType type);
        Quaternion GetRotation(ControllerType type);
        Vector2 Touch(ControllerType type);
        void TriggerHapticVibration(ControllerType type, HapticData data);
        bool IsDown(ControllerType controllerType, ButtonType buttonType);
        bool IsUp(ControllerType controllerType, ButtonType buttonType);
    }
}
