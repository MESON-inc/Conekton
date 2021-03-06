using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;

namespace Conekton.ARMultiplayer.Avatar.Application
{
    public class AvatarService : IAvatarService, IInitializable
    {
        [Inject] private IAvatarSystem _system = null;
        [Inject(Id = "Player")] private IAvatarController _playerAvatarController = null;

        IAvatar IAvatarService.Create()
        {
            IAvatar avatar = _system.Create();

            // IAvatarBody body = _avatarBodySystem.GetOrCreate((byte)'B');
            // body.SetAvatar(avatar);
            
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
            }

            return _system.GetMain();
        }
    }
}

