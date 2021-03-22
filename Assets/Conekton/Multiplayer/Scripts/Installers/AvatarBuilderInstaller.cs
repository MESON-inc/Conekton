using Conekton.ARMultiplayer.AvatarBody.Application;
using UnityEngine;
using Zenject;
using Conekton.ARMultiplayer.AvatarBody.Domain;
using Conekton.ARMultiplayer.AvatarBody.Infrastructure;
using Conekton.ARMultiplayer.AvatarBuilder.Domain;
using AvatarBodyClass = Conekton.ARMultiplayer.AvatarBody.Presentation.AvatarBody;

namespace Conekton.ARMultiplayer.Avatar.Application
{
    public class AvatarBuilderInstaller : MonoInstaller
    {
        [SerializeField] private byte _defaultMainAvatarBodyType = (byte)'A';
        
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<MainAvatarBuilder>()
                .AsCached()
                .WithArguments(_defaultMainAvatarBodyType)
                .NonLazy();

            Container
                .Bind<IRemoteAvatarBuilder>()
                .To<RemoteAvatarBuilder>()
                .AsCached();
        }
    }
}