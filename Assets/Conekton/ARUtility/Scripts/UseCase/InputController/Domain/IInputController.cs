using UnityEngine;

namespace Conekton.ARUtility.Input.Domain
{
    public struct HapticData
    {
        public float DurationSeconds;
        public float Frequency;
        public float Amplitude;

        static public HapticData Default => new HapticData { DurationSeconds = 0.1f, Frequency = 200f, Amplitude = 0.8f, };
    }

    public interface IInputController
    {
        bool IsTriggerDown { get; }
        bool IsTriggerUp { get; }
        bool IsTouch { get; }
        bool IsTouchDown { get; }
        bool IsTouchUp { get; }
        Vector3 Position { get; }
        Vector3 Forward { get; }
        Quaternion Rotation { get; }
        Vector2 Touch { get; }
        void TriggerHapticVibration(HapticData data);
    }
}
