using Conekton.ARMultiplayer.AvatarBody.Application;
using UnityEngine;
using Zenject;
using Conekton.ARMultiplayer.AvatarBody.Domain;
using Conekton.ARMultiplayer.AvatarBody.Infrastructure;
using AvatarBodyClass = Conekton.ARMultiplayer.AvatarBody.Presentation.AvatarBody;

namespace Conekton.ARMultiplayer.Avatar.Application
{
    public class AvatarBodyInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _avatarBodyFactoryPrefab = null;
        [SerializeField] private byte _defaultMainAvatarBodyType = (byte)'A';

        public override void InstallBindings()
        {
            Container
                .Bind<IAvatarBodySystem>()
                .FromSubContainerResolve()
                .ByNewGameObjectMethod(InstallBindingsToSubContainer)
                .AsCached();
            
            Container
                .BindInterfacesAndSelfTo<MainAvatarBuilder>()
                .AsCached()
                .WithArguments(_defaultMainAvatarBodyType)
                .NonLazy();
        }
        
        private void InstallBindingsToSubContainer(DiContainer subContainer)
        {
            subContainer
                .Bind<IAvatarBodySystem>()
                .To<AvatarBodySystem>()
                .AsCached();
            
            subContainer
                .Bind<IAvatarBodyRepository>()
                .To<AvatarBodyRepository>()
                .AsCached();

            subContainer
                .Bind<AvatarBodyFactory>()
                .FromComponentInNewPrefab(_avatarBodyFactoryPrefab)
                .AsCached();
        }
    }
}