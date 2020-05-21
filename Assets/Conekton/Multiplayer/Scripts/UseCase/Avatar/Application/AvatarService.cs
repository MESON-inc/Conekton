using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;

namespace Conekton.ARMultiplayer.Avatar.Application
{
    public class AvatarService : IAvatarService, IInitializable
    {
        [Inject] private IAvatarSystem _system = null;
        [Inject(Id = "Player")] private IAvatarController _playerAvatarController = null;

        IAvatar IAvatarService.Create() => _system.Create();
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
            }

            return _system.GetMain();
        }
    }
}

