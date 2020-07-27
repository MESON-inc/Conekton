using UnityEngine;
using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using Conekton.ARMultiplayer.NetworkMultiplayer.Infrastructure;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Application
{
    public class StubNetworkInfraInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IMultiplayerNetworkInfrastructure>().To<StubNetworkInfra>().AsCached();
        }
    }
}

