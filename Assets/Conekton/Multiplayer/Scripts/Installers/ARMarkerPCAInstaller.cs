using UnityEngine;
using Zenject;

namespace Conekton.ARMultiplayer.PersistentCoordinate.Application
{
    public class ARMarkerPCAInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ARMarkerPCABinder>().AsCached().NonLazy();
        }
    }
}

