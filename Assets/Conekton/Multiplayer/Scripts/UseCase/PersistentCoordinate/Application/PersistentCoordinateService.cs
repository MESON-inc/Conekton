using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARMultiplayer.PersistentCoordinate.Domain;

namespace Conekton.ARMultiplayer.PersistentCoordinate.Application
{
    public class PersistentCoordinateService : IPersistentCoordinateService
    {
        [Inject] private IPersistentCoordinateSystem _persistentSystem = null;

        IReadOnlyCollection<IPCA> IPersistentCoordinateService.GetAllPCA() => _persistentSystem.GetAllPCA();
        IPCA[] IPersistentCoordinateService.GetNearPCA(Vector3 position, int count) => _persistentSystem.GetNearPCA(position, count);
        void IPersistentCoordinateService.Register(IPCA pca) => _persistentSystem.Register(pca);
        void IPersistentCoordinateService.Unregister(IPCA pca) => _persistentSystem.Unregister(pca);
    }
}

