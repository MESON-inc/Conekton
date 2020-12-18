using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using Zenject;

using Conekton.ARMultiplayer.NetworkMultiplayer.Infrastructure;
using UnityEngine;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Application
{
    public class MultiplayerContextInstaller : MonoInstaller
    {
        [SerializeField] private bool _autoConnect = true;
        
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<DefaultMultiplayerNetworkContext>()
                .AsCached()
                .WithArguments(_autoConnect)
                .NonLazy();
        }
    }
}
