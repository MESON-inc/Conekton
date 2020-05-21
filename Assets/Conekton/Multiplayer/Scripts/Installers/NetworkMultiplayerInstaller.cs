using UnityEngine;
using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using Conekton.ARMultiplayer.NetworkMultiplayer.Infrastructure;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Application
{
    public class NetworkMultiplayerInstaller : MonoInstaller
    {
        [SerializeField] private bool _isStubMode = true;
        [SerializeField] private GameObject _photonNetworkInfraPrefab = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MultiplayerNetworkSystem>().AsCached().NonLazy();

            if (_isStubMode)
            {
                Container.Bind<IMultiplayerNetworkInfrastructure>().To<StubNetworkInfra>().AsCached();
            }
            else
            {
                Container.Bind<IMultiplayerNetworkInfrastructure>().To<PhotonNetworkInfra>().FromComponentInNewPrefab(_photonNetworkInfraPrefab).AsCached();
            }
        }
    }
}

