using Zenject;

using Conekton.ARMultiplayer.PersistentCoordinate.Domain;
using Conekton.ARMultiplayer.PersistentCoordinate.Infrastructure;

namespace Conekton.ARMultiplayer.PersistentCoordinate.Application
{
    public class PhotonPersistentCoordinateInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IPersistentCoordinateService>()
                .FromSubContainerResolve()
                .ByNewGameObjectMethod(InstallBindingsToSubContainer)
                .AsCached();

            RegisterCustomTypeToPhoton();
        }

        private void InstallBindingsToSubContainer(DiContainer subContainer)
        {
            subContainer.Bind<IPersistentCoordinateService>().To<PersistentCoordinateService>().AsCached();
            subContainer.Bind<IPersistentCoordinateSystem>().To<PersistentCoordinateSystem>().AsCached();
            subContainer.Bind<IPersistentCoordinateRepository>().To<PersistentCoordinateRepository>().AsCached();
        }

        private void RegisterCustomTypeToPhoton()
        {
            ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(PCAID), (byte)'A', PCAIDSerializer.Serialize, PCAIDSerializer.Deserialize);
        }
    }
}

