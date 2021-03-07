using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.AvatarBody.Domain;
using Conekton.ARMultiplayer.AvatarBuilder.Domain;
using Zenject;

namespace Conekton.ARMultiplayer.Avatar.Application
{
    public class RemoteAvatarBuilder : IRemoteAvatarBuilder
    {
        [Inject] private IAvatarService _avatarService = null;
        [Inject] private IAvatarBodySystem _avatarBodySystem = null;

        public (IAvatar, IAvatarBody) Build(byte bodyType)
        {
            IAvatar avatar = _avatarService.GetMain();
            IAvatarBody avatarBody = _avatarBodySystem.GetOrCreate(bodyType);
            avatarBody.SetAvatar(avatar);

            return (avatar, avatarBody);
        }
    }
}