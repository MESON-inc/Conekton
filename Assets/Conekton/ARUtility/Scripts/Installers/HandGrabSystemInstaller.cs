using Conekton.ARUtility.HandGrabSystemUseCase.Domain;
using Conekton.ARUtility.HandGrabSystemUseCase.Infrastructure;
using UnityEngine;
using Zenject;

namespace Conekton.ARUtility.HandGrabSystemUseCase.Application
{
    public class HandGrabSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IHandGrabSystem>()
                .To<HandGrabSystem>()
                .AsCached();

            Container
                .Bind<IHandGrabReopsitory>()
                .FromSubContainerResolve()
                .ByNewGameObjectMethod(InstallBindingsToSubContainer)
                .AsCached();
        }

        private void InstallBindingsToSubContainer(DiContainer subContainer)
        {
            subContainer
                .Bind<IHandGrabReopsitory>()
                .To<HandGrabRepository>()
                .AsCached();
        }
    }
}
