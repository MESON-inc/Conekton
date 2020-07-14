using UnityEngine;
using Zenject;

using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using Conekton.ARMultiplayer.NetworkMultiplayer.Infrastructure;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Application
{
    public class PhotonNetworkInfraInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _photonNetworkInfraPrefab = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MultiplayerNetworkSystem>().AsCached().NonLazy();
            Container.Bind<IMultiplayerNetworkInfrastructure>().To<PhotonNetworkInfra>().FromComponentInNewPrefab(_photonNetworkInfraPrefab).AsCached();
        }
    }
}

