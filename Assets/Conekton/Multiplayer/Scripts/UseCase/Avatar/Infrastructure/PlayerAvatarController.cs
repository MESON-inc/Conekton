using UnityEngine;

using Zenject;

using Conekton.ARUtility.Player.Domain;
using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARUtility.Input.Domain;

namespace Conekton.ARMultiplayer.Avatar.Infrastructure
{
    public class PlayerAvatarController : IAvatarController
    {
        [Inject] private IPlayer _player = null;
        [Inject] private IInputController _inputController = null;

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
                return new Pose(_inputController.GetPosition(ControllerType.Left), _inputController.GetRotation(ControllerType.Left));
            }
            else
            {
                return new Pose(_inputController.GetPosition(ControllerType.Right), _inputController.GetRotation(ControllerType.Right));
            }
        }
    }
}

