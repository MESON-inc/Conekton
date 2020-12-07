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
        [SerializeField] private GameObject _avatarBodyPrefab = null;

        public override void InstallBindings()
        {
            Container
                .Bind<IAvatarBodySystem<AvatarBodyTypeArgs>>()
                .FromSubContainerResolve()
                .ByNewGameObjectMethod(InstallBindingsToSubContainer)
                .AsCached();
        }
        
        private void InstallBindingsToSubContainer(DiContainer subContainer)
        {
            subContainer
                .Bind<IAvatarBodySystem<AvatarBodyTypeArgs>>()
                .To<AvatarBodySystem>()
                .AsCached();
            
            subContainer
                .Bind<IAvatarBodyRepository>()
                .To<AvatarBodyRepository>()
                .AsCached();
            
            subContainer
                .BindFactory<AvatarBodyTypeArgs, AvatarBodyClass, AvatarBodyFactory>()
                .FromComponentInNewPrefab(_avatarBodyPrefab);
        }
    }
}