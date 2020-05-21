using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARMultiplayer.PersistentCoordinate.Domain;

namespace Conekton.ARMultiplayer.PersistentCoordinate.Infrastructure
{
    public class PersistentCoordinateSystem : IPersistentCoordinateSystem
    {
        [Inject] private IPersistentCoordinateRepository _persistentRepository = null;

        IReadOnlyCollection<IPCA> IPersistentCoordinateSystem.GetAllPCA() => _persistentRepository.GetAllPCA();

        IPCA[] IPersistentCoordinateSystem.GetNearPCA(Vector3 position, int count)
        {
            IReadOnlyCollection<IPCA> pcas = _persistentRepository.GetAllPCA();

            // NOTE: Subtract a priority as value from squared distance then High priority will return small value than others.
            return pcas
                .OrderBy(a => (a.Position - position).sqrMagnitude - (int)a.Priority)
                .Take(count)
                .ToArray();
        }

        void IPersistentCoordinateSystem.Register(IPCA pca) => Register(pca);
        void IPersistentCoordinateSystem.Unregister(IPCA pca) => Unregister(pca);

        private PCAID CreateNewID(IPCA pca)
        {
            return new PCAID
            {
                ID = $"{pca.GetType().ToString()}-{pca.UniqueID}",
            };
        }

        private void Register(IPCA pca)
        {
            if (PCAID.IsNotSet(pca.ID))
            {
                pca.ID = CreateNewID(pca);
            }

            Debug.Log($"Registerd a PCA ID [{pca.ID}]");

            _persistentRepository.Register(pca);
        }

        private void Unregister(IPCA pca)
        {
            if (PCAID.IsNotSet(pca.ID))
            {
                return;
            }

            Debug.Log($"Unregisterd a PCA ID [{pca.ID}]");

            _persistentRepository.Unregister(pca);
        }
    }
}

