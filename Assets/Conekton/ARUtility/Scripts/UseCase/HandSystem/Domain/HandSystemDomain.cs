using UnityEngine;

namespace Conekton.ARUtility.HandSystemUseCase.Domain
{
    public delegate void ChangeHandStateEvent(HandType handType);

    /// <summary>
    /// This is the finger type to get some parameters.
    /// </summary>
    public enum FingerType
    {
        Thumb,
        Index,
        Middle,
        Ring,
        Pinky,
    }

    /// <summary>
    /// This is the hand gesture type to get whitch current gesture type is.
    /// </summary>
    public enum HandGestureType
    {
        None,
        Rock,
        Paper,
        Scissors,
    }

    /// <summary>
    /// This is the hand type.
    /// </summary>
    public enum HandType
    {
        Left,
        Right,
    }

    /// <summary>
    /// This interface represents the hand object that is provided by SDKs.
    /// </summary>
    public interface IHand
    {
        bool IsTracked { get; }
        HandType GetHandType();
        bool GetFingerIsPinching(FingerType fingerType);
        Pose GetPose();
        Vector3 GetPosition();
        Quaternion GetRotation();
        bool TryGetPalmPositionAndNormal(out Vector3 position, out Vector3 normal);
        float GetFingerStrength(FingerType fingerType);
    }

    /// <summary>
    /// This interface provides a service to get a hand objects.
    /// </summary>
    public interface IHandProvider
    {
        IHand GetHand(HandType handType);
        IHand[] GetAllHands();
    }

    /// <summary>
    /// This interface provides to get hands information.
    /// </summary>
    public interface IHandSystem
    {
        event ChangeHandStateEvent ChangeHandState;
        bool IsTracked(HandType handType);
        HandGestureType GetGestureType(HandType handType);
        (Vector3, Vector3) GetPalmPositionNormal(HandType handType);
        Pose GetPose(HandType handType);
        float GetFingerStrength(HandType handType, FingerType fingerType);
    }
}

