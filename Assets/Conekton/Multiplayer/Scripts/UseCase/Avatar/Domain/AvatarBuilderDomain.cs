using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.AvatarBody.Domain;

namespace Conekton.ARMultiplayer.AvatarBuilder.Domain
{
    public interface IMainAvatarBuilder
    {
    }

    public interface IRemoteAvatarBuilder
    {
        (IAvatar, IAvatarBody) Build(AvatarID avatarID, byte bodyType);
    }
}

