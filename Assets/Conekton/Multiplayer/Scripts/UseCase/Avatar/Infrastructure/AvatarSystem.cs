using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;

namespace Conekton.ARMultiplayer.Avatar.Infrastructure
{
    public class AvatarSystem : IAvatarSystem
    {
        [Inject] private IAvatarRepository _repository = null;

        private int _avatarIndex = 1;
        private AvatarID _mainID = new AvatarID { ID = 0, };

        IAvatar IAvatarSystem.CreateMain()
        {
            return _repository.Create(_mainID);
        }

        IAvatar IAvatarSystem.Create()
        {
            AvatarID id = GetNewID();
            return _repository.Create(id);
        }

        IAvatar IAvatarSystem.Create(AvatarID id)
        {
            return _repository.Create(id);
        }

        void IAvatarSystem.Remove(AvatarID id)
        {
            _repository.Remove(id);
        }

        IAvatar IAvatarSystem.Find(AvatarID id) => _repository.Find(id);

        IAvatar IAvatarSystem.GetMain()
        {
            return _repository.Find(_mainID);
        }

        private AvatarID GetNewID()
        {
            return new AvatarID
            {
                ID = (_avatarIndex++).ToString().GetHashCode(),
            };
        }
    }
}

