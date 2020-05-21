using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARMultiplayer.SpatialAnchor.Domain;

namespace Conekton.ARMultiplayer.SpatialAnchor.Infrastructure
{
    public class SpatialAnchorRepository : ISpatialAnchorRepository
    {
        [Inject] private Presentation.SpatialAnchor.SpatialAnchorFactory _anchorFactory = null;

        private Dictionary<SpatialAnchorID, ISpatialAnchor> _database = new Dictionary<SpatialAnchorID, ISpatialAnchor>();

        ISpatialAnchor ISpatialAnchorRepository.Create(SpatialAnchorID anchorID) => Create(anchorID);
        ISpatialAnchor ISpatialAnchorRepository.Find(SpatialAnchorID anchorID) => Find(anchorID);

        private ISpatialAnchor Create(SpatialAnchorID anchorID)
        {
            ISpatialAnchor anchor = _anchorFactory.Create(anchorID);

            AddAnchor(anchorID, anchor);

            return anchor;
        }

        private ISpatialAnchor Find(SpatialAnchorID anchorID)
        {
            if (_database.ContainsKey(anchorID))
            {
                return _database[anchorID];
            }

            return null;
        }

        private void AddAnchor(SpatialAnchorID anchorID, ISpatialAnchor anchor)
        {
            if (Find(anchorID) != null)
            {
                Debug.LogWarning($"{anchorID} is already exist in the database. The anchor won't be added this time.");
                return;
            }

            _database.Add(anchorID, anchor);
        }
    }
}

