using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using Zenject;

using Conekton.ARMultiplayer.NetworkMultiplayer.Infrastructure;
using UnityEngine;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Application
{
    public class MultiplayerContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IMultiplayerNetworkContext>()
                .To<DefaultMultiplayerNetworkContext>()
                .AsCached();
        }
    }
}
