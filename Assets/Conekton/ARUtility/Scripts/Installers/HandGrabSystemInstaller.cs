using Conekton.ARUtility.GrabSystemUseCase.Domain;
using Conekton.ARUtility.GrabSystemUseCase.Infrastructure;
using UnityEngine;
using Zenject;

namespace Conekton.ARUtility.GrabSystemUseCase.Application
{
    public class HandGrabSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IGrabSystem>()
                .To<GrabSystem>()
                .AsCached();

            Container
                .Bind<IGrabReopsitory>()
                .FromSubContainerResolve()
                .ByNewGameObjectMethod(InstallBindingsToSubContainer)
                .AsCached();
        }

        private void InstallBindingsToSubContainer(DiContainer subContainer)
        {
            subContainer
                .Bind<IGrabReopsitory>()
                .To<GrabRepository>()
                .AsCached();
        }
    }
}
