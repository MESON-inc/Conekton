using UnityEngine;
using Zenject;

using Conekton.ARUtility.Player.Domain;
using Conekton.ARUtility.Player.Infrastructure;
using Conekton.ARUtility.UseCase.WorldOrigin.Domain;
using Conekton.ARUtility.UseCase.WorldOrigin.Infrastructure;

namespace Conekton.ARUtility.UseCase.WorldOrigin.Applications
{
    public class WorldOriginInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _worldOriginPrefab = null;
        
        public override void InstallBindings()
        {
            Container
                .Bind<IWorldOrigin>()
                .To<Infrastructure.WorldOrigin>()
                .FromComponentInNewPrefab(_worldOriginPrefab)
                .AsSingle();

            Container
                .Bind<IWorldMarkerController>()
                .To<WorldMarkerController>()
                .AsCached();
        }
    }
}
