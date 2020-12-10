using UnityEngine;
using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.Avatar.Infrastructure;
using Conekton.ARMultiplayer.Avatar.Presentation;

namespace Conekton.ARMultiplayer.Avatar.Application
{
    public class AvatarInstaller : MonoInstaller
    {
        [SerializeField] private Presentation.Avatar _avatarPrefab;

        public override void InstallBindings()
        {
            Container
                .Bind<IAvatarService>()
                .FromSubContainerResolve()
                .ByNewGameObjectMethod(InstallBindingsToSubContainer)
                .AsCached()
                .NonLazy();
        }

        private void InstallBindingsToSubContainer(DiContainer subContainer)
        {
            subContainer
                .BindInterfacesAndSelfTo<AvatarService>()
                .AsCached()
                .NonLazy();
                
            subContainer
                .Bind<IAvatarSystem>()
                .To<AvatarSystem>()
                .AsCached();
            
            subContainer
                .Bind<IAvatarRepository>()
                .To<AvatarRepository>()
                .AsCached();

            subContainer
                .BindInterfacesAndSelfTo<PlayerAvatarController>()
                .AsCached();
            
            subContainer
                .Bind<IAvatarController>()
                .WithId("Player")
                .To<PlayerAvatarController>()
                .FromResolve();


            subContainer
                .BindFactory<AvatarID, Presentation.Avatar, AvatarFactory>()
                .FromComponentInNewPrefab(_avatarPrefab);
        }
    }
}

