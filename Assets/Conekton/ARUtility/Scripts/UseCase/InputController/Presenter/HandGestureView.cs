using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARUtility.Player.Domain;

namespace Conekton.ARUtility.Input.Presenter
{
    public class HandGestureView : MonoBehaviour
    {
        [Inject] private IPlayer _player = null;

        [SerializeField] private Transform _leftHandTrans = null;
        [SerializeField] private Transform _rightHandTrans = null;

        private void Update()
        {
            UpdatePose();
        }

        private void UpdatePose()
        {
            UpdatePose(HumanPoseType.LeftHand);
            UpdatePose(HumanPoseType.RightHand);
        }

        private void UpdatePose(HumanPoseType type)
        {
            if (_player.IsActiveHumanPose(type))
            {
                Pose pose = _player.GetHumanPose(type);
                Transform trans = GetTransform(type);
                trans.SetPositionAndRotation(pose.position, pose.rotation);
            }
        }

        private Transform GetTransform(HumanPoseType type)
        {
            switch (type)
            {
                case HumanPoseType.LeftHand:
                    return _leftHandTrans;

                case HumanPoseType.RightHand:
                    return _rightHandTrans;
            }

            return null;
        }
    }
}

