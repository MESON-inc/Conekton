using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conekton.ARUtility.Player.Domain
{
    public enum HumanPoseType
    {
        None = 0,
        Head = 1,
        LeftHand = 2,
        RightHand = 3,
        Other = 9999,
    }

    public interface IPlayer
    {
        Transform Root { get; }
        GameObject CameraRig { get; }
        Camera MainCamera { get; }
        Vector3 Position { get; }
        Vector3 Forward { get; }
        Quaternion Rotation { get; }
        Pose GetHumanPose(HumanPoseType type);
        Pose GetHumanLocalPose(HumanPoseType type);
        bool IsActiveHumanPose(HumanPoseType type);
    }
}

