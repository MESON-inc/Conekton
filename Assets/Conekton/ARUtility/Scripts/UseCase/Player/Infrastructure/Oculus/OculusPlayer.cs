using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARUtility.Player.Domain;

namespace Conekton.ARUtility.Player.Infrastructure
{
    public class OculusPlayer : MonoBehaviour
#if UNITY_ANDROID && PLATFORM_OCULUS
        , IPlayer
#endif
    {
        [SerializeField] private GameObject _cameraRig = null;
        [SerializeField] private Camera _camera = null;

#if UNITY_ANDROID && PLATFORM_OCULUS
        private Transform Root => transform;

        Transform IPlayer.Root => Root;
        GameObject IPlayer.CameraRig => _cameraRig;
        Camera IPlayer.MainCamera => _camera;
        Vector3 IPlayer.Position => _cameraRig.transform.position;
        Vector3 IPlayer.Forward => _cameraRig.transform.forward;
        Quaternion IPlayer.Rotation => _cameraRig.transform.rotation;
        Pose IPlayer.GetHumanPose(HumanPoseType type) => GetHumanPose(type);
        Pose IPlayer.GetHumanLocalPose(HumanPoseType type) => GetHumanLocalPose(type);
        bool IPlayer.IsActiveHumanPose(HumanPoseType type) => IsActiveHumanPose(type);

        private Pose GetHumanPose(HumanPoseType type)
        {
            switch (type)
            {
                case HumanPoseType.Head:
                    return new Pose(_cameraRig.transform.position, _cameraRig.transform.rotation);

                // NOTE: Hand tracking feature is comming soon.
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
                    return new Pose(_cameraRig.transform.localPosition, _cameraRig.transform.localRotation);

                // NOTE: Hand tracking feature is comming soon.
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

