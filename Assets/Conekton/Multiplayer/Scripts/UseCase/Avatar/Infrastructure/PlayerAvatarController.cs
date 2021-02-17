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

        Pose IAvatarController.GetRootPose()
        {
            return new Pose(_player.Root.position, _player.Root.rotation);
        }

        Pose IAvatarController.GetHeadPose()
        {
            return new Pose(_player.Position, _player.Rotation);
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

