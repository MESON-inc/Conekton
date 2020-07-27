using Zenject;

using Conekton.ARMultiplayer.NetworkMultiplayer.Infrastructure;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Application
{
    public class MultiplayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MultiplayerNetworkSystem>().AsCached().NonLazy();
        }
    }
}

