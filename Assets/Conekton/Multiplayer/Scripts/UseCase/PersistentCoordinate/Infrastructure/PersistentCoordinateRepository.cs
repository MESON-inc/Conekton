using System.Collections.Generic;
using UnityEngine;

using Conekton.ARMultiplayer.PersistentCoordinate.Domain;

namespace Conekton.ARMultiplayer.PersistentCoordinate.Infrastructure
{
    public class PersistentCoordinateRepository : IPersistentCoordinateRepository
    {
        private Dictionary<PCAID, IPCA> _database = new Dictionary<PCAID, IPCA>(50);

        IReadOnlyCollection<IPCA> IPersistentCoordinateRepository.GetAllPCA()
        {
            return _database.Values;
        }

        void IPersistentCoordinateRepository.Register(IPCA pca)
        {
            if (!_database.ContainsKey(pca.ID))
            {
                _database.Add(pca.ID, pca);
            }
        }

        void IPersistentCoordinateRepository.Unregister(IPCA pca)
        {
            if (_database.ContainsKey(pca.ID))
            {
                _database.Remove(pca.ID);
            }
        }
    }
}

