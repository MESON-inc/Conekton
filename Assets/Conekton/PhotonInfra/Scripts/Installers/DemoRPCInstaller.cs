using UnityEngine;
using Zenject;

using Conekton.ARMultiplayer.Demo.RPC.Infrastructure;

namespace Conekton.ARMultiplayer.Demo.RPC.Application
{
    public class DemoRPCInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DemoRPCService>().AsCached().NonLazy();
        }
    }
}

