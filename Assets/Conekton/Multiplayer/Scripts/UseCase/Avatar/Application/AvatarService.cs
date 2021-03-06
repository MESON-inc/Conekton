using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.AvatarBody.Domain;

namespace Conekton.ARMultiplayer.Avatar.Application
{
    public class AvatarService : IAvatarService, IInitializable
    {
        [Inject] private IAvatarSystem _system = null;
        [Inject(Id = "Player")] private IAvatarController _playerAvatarController = null;
        [Inject] private IAvatarBodySystem _avatarBodySystem = null;

        IAvatar IAvatarService.Create()
        {
            IAvatar avatar = _system.Create();

            IAvatarBody body = _avatarBodySystem.GetOrCreate((byte)'B');
            body.SetAvatar(avatar);
            
            return avatar;
        }
        void IAvatarService.Remove(AvatarID id) => _system.Remove(id);
        IAvatar IAvatarService.Find(AvatarID id) => _system.Find(id);
        IAvatar IAvatarService.GetMain() => GetOrCreateMain();

        void IInitializable.Initialize()
        {
            GetOrCreateMain();
        }

        private IAvatar GetOrCreateMain()
        {
            if (_system.GetMain() == null)
            {
                IAvatar avatar = _system.CreateMain();
                avatar.SetAvatarController(_playerAvatarController);

                IAvatarBody body = _avatarBodySystem.GetOrCreate((byte)'A');
                body.SetAvatar(avatar);
            }

            return _system.GetMain();
        }
    }
}

