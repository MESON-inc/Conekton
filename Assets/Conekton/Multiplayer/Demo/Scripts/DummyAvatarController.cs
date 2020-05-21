using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.Avatar.Infrastructure;

namespace Conekton.ARMultiplayer.Avatar.Mock
{
    public class DummyAvatarController : MonoBehaviour, IAvatarController
    {
        [Inject] private IAvatarService _avatarService = null;

        [SerializeField] private Transform _headTrans = null;
        [SerializeField] private Transform _leftHandTrans = null;
        [SerializeField] private Transform _rightHandTrans = null;

        [SerializeField] private Wearable _headWearable1 = null;
        [SerializeField] private Wearable _headWearable2 = null;
        [SerializeField] private Wearable _leftHandWearable = null;
        [SerializeField] private Wearable _rightHandWearable = null;

        private void Start()
        {
            IAvatar avatar = _avatarService.Create();
            avatar.SetAvatarController(this);

            WearablePack pack = new WearablePack(new[]
            {
                _headWearable1,
                _headWearable2,
                _leftHandWearable,
                _rightHandWearable,
            });

            avatar.SetWearablePack(pack);
        }

        Pose IAvatarController.GetHeadPose()
        {
            return new Pose
            {
                position = _headTrans.position,
                rotation = _headTrans.rotation,
            };
        }

        Pose IAvatarController.GetHandPose(AvatarPoseType type)
        {
            Transform trans = GetHand(type);
            return new Pose
            {
                position = trans.position,
                rotation = trans.rotation,
            };
        }


        private Transform GetHand(AvatarPoseType type)
        {
            switch (type)
            {
                case AvatarPoseType.Left:
                    return _leftHandTrans;

                case AvatarPoseType.Right:
                    return _rightHandTrans;
            }

            return null;
        }
    }
}

