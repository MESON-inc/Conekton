using UnityEngine;
using Zenject;

using Conekton.ARUtility.UseCase.ARAnchor.Domain;
using Conekton.ARUtility.UseCase.ARAnchor.Infrastructure;

namespace Conekton.ARUtility.UseCase.ARAnchor.Application
{
    public class ARAnchorInstaller : MonoInstaller
    {
        [SerializeField] private Presentation.ARAnchor _anchorPrefab = null;

        public override void InstallBindings()
        {
            Container
                .Bind<IARAnchorService>()
                .FromSubContainerResolve()
                .ByNewGameObjectMethod(InstallBindingsToSubContainer)
                .AsCached();

            Container
                .BindFactory<AnchorID, Presentation.ARAnchor, Presentation.ARAnchor.Factory>()
                .FromComponentInNewPrefab(_anchorPrefab);
        }

        private void InstallBindingsToSubContainer(DiContainer subContainer)
        {
            subContainer.Bind<IARAnchorService>().To<ARAnchorService>().AsCached();
            subContainer.Bind<IARAnchorSystem>().To<ARAnchorSystem>().AsCached();
            subContainer.Bind<IARAnchorRepository>().To<ARAnchorRepository>().AsCached();
        }
    }
}
