using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.AvatarBody.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARMultiplayer.Avatar.Application
{
    public class MainAvatarBuilder : IInitializable
    {
        [Inject] private IAvatarService _avatarService = null;
        [Inject] private IAvatarBodySystem _avatarBodySystem = null;

        private byte _mainAvatarBodyType = 0;

        public MainAvatarBuilder(byte bodyType)
        {
            _mainAvatarBodyType = bodyType;
        }

        public void Initialize()
        {
            IAvatar mainAvatar = _avatarService.GetMain();
            IAvatarBody avatarBody = _avatarBodySystem.GetOrCreate(_mainAvatarBodyType);
            avatarBody.SetAvatar(mainAvatar);
        }
    }
}