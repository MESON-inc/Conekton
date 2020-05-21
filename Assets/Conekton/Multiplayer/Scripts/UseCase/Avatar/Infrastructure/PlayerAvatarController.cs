using UnityEngine;

using Zenject;

using Conekton.ARUtility.Player.Domain;
using Conekton.ARMultiplayer.Avatar.Domain;

namespace Conekton.ARMultiplayer.Avatar.Infrastructure
{
    public class PlayerAvatarController : IAvatarController
    {
        [Inject] private IPlayer _player = null;

        Pose IAvatarController.GetHeadPose()
        {
            return new Pose
            {
                position = _player.Position,
                rotation = _player.Rotation,
            };
        }

        Pose IAvatarController.GetHandPose(AvatarPoseType type)
        {
            if (type == AvatarPoseType.Left)
            {
                return _player.GetHumanPose(HumanPoseType.LeftHand);
            }
            else
            {
                return _player.GetHumanPose(HumanPoseType.RightHand);
            }
        }
    }
}

