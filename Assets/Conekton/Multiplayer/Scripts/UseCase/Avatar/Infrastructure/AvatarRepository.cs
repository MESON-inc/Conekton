using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Conekton.ARMultiplayer.Avatar.Application;
using Conekton.ARMultiplayer.Avatar.Domain;

namespace Conekton.ARMultiplayer.Avatar.Infrastructure
{
    public class AvatarRepository : IAvatarRepository
    {
        [Inject] private AvatarFactory _avatarFactory = null;

        private Dictionary<AvatarID, IAvatar> _avatars = new Dictionary<AvatarID, IAvatar>();

        IAvatar IAvatarRepository.Create(AvatarID id)
        {
            IAvatar avatar = Find(id);

            if (avatar == null)
            {
                avatar = _avatarFactory.Create(id);
                _avatars.Add(id, avatar);
            }

            return avatar;
        }

        void IAvatarRepository.Remove(AvatarID id)
        {
            if (!_avatars.ContainsKey(id))
            {
                return;
            }

            if (TryGetAvatar(id, out IAvatar avatar))
            {
                avatar.Destory();
            }

            _avatars.Remove(id);

            Debug.Log($"Left avatar count is {_avatars.Keys.Count}");
        }

        private bool TryGetAvatar(AvatarID id, out IAvatar avatar)
        {
            if (!_avatars.TryGetValue(id, out avatar))
            {
                return false;
            }

            if (avatar == null)
            {
                return false;
            }

            if (avatar is MonoBehaviour missingCheck)
            {
                if (missingCheck == null)
                {
                    return false;
                }
            }

            return true;
        }

        IAvatar IAvatarRepository.Find(AvatarID id) => Find(id);

        private IAvatar Find(AvatarID id)
        {
            if (_avatars.ContainsKey(id))
            {
                return _avatars[id];
            }

            return null;
        }
    }
}