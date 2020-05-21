using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARUtility.Player.Domain;

namespace Conekton.ARUtility.Player.Infrastructure
{
    public class EditorPlayer : MonoBehaviour, IPlayer
    {
        [SerializeField] private GameObject _cameraRig = null;
        [SerializeField] private Camera _camera = null;

        private Transform Root => transform;

        Transform IPlayer.Root => Root;
        GameObject IPlayer.CameraRig => _cameraRig;
        Camera IPlayer.MainCamera => _camera;
        Vector3 IPlayer.Position => _cameraRig.transform.position;
        Vector3 IPlayer.Forward => _cameraRig.transform.forward;
        Quaternion IPlayer.Rotation => _cameraRig.transform.rotation;
        Pose IPlayer.GetHumanPose(HumanPoseType type) => default;
        Pose IPlayer.GetHumanLocalPose(HumanPoseType type) => default;
        bool IPlayer.IsActiveHumanPose(HumanPoseType type) => false;
    }
}
