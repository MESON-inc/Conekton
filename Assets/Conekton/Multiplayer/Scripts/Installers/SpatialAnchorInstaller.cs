using UnityEngine;

using Zenject;

using Conekton.ARMultiplayer.SpatialAnchor.Domain;
using Conekton.ARMultiplayer.SpatialAnchor.Infrastructure;

namespace Conekton.ARMultiplayer.SpatialAnchor.Application
{
    public class SpatialAnchorInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _spatialAnchorPrefab = null;

        public override void InstallBindings()
        {
            Container
                .Bind<ISpatialAnchorService>()
                .FromSubContainerResolve()
                .ByNewGameObjectMethod(InstallBindingSystemToSubContainer)
                .AsCached()
                .NonLazy();

            Container
                .BindFactory<SpatialAnchorID, Presentation.SpatialAnchor, Presentation.SpatialAnchor.SpatialAnchorFactory>()
                .FromComponentInNewPrefab(_spatialAnchorPrefab)
                .AsCached();

            Container.Bind<SpatialAnchorUtility>().AsCached();
        }

        private void InstallBindingSystemToSubContainer(DiContainer subContainer)
        {
            subContainer.Bind<ISpatialAnchorSystem>().To<SpatialAnchorSystem>().AsCached();
            subContainer.Bind<ISpatialAnchorRepository>().To<SpatialAnchorRepository>().AsCached();
            subContainer.Bind<ISpatialAnchorTunerRepository>().To<PhotonSpatialAnchorTunerRepository>().AsCached();
            subContainer.BindInterfacesAndSelfTo<SpatialAnchorService>().AsCached();
        }
    }
}

