using UnityEngine;
using Conekton.ARUtility.Player.Domain;

namespace Conekton.ARUtility.Player.Infrastructure
{
#if UNITY_WSA || UNITY_WSA_10_0
    public class MRTKPlayer : IPlayer
    {
        private Camera PlayerCamera => Camera.main;
        private GameObject PlayerCameraRig => PlayerCamera.gameObject;

        Transform IPlayer.Root => PlayerCamera.transform.parent;
        GameObject IPlayer.CameraRig => PlayerCameraRig;
        Camera IPlayer.MainCamera => PlayerCamera;
        Vector3 IPlayer.Position => PlayerCameraRig.transform.position;
        Vector3 IPlayer.Forward => PlayerCameraRig.transform.forward;
        Quaternion IPlayer.Rotation => PlayerCameraRig.transform.rotation;
        Pose IPlayer.GetHumanPose(HumanPoseType type) => GetHumanPose(type);
        Pose IPlayer.GetHumanLocalPose(HumanPoseType type) => GetHumanLocalPose(type);
        bool IPlayer.IsActiveHumanPose(HumanPoseType type) => IsActiveHumanPose(type);

        private Pose GetHumanPose(HumanPoseType type)
        {
            switch (type)
            {
                case HumanPoseType.Head:
                    return new Pose(PlayerCameraRig.transform.position, PlayerCameraRig.transform.rotation);
                case HumanPoseType.LeftHand:
                    return new Pose(Vector3.zero, Quaternion.identity);

                case HumanPoseType.RightHand:
                    return new Pose(Vector3.zero, Quaternion.identity);
            }
            return default;
        }

        private Pose GetHumanLocalPose(HumanPoseType type)
        {
            switch (type)
            {
                case HumanPoseType.Head:
                    return new Pose(PlayerCameraRig.transform.localPosition, PlayerCameraRig.transform.localRotation);
                
                case HumanPoseType.LeftHand:
                    return new Pose(Vector3.zero, Quaternion.identity);

                case HumanPoseType.RightHand:
                    return new Pose(Vector3.zero, Quaternion.identity);
            }
            return default;
        }

        private bool IsActiveHumanPose(HumanPoseType type)
        {
            switch (type)
            {
                case HumanPoseType.Head:
                    return true;
            }
            return false;
        }
#endif
    }
}

