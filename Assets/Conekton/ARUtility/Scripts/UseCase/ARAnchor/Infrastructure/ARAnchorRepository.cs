using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARUtility.UseCase.ARAnchor.Domain;

namespace Conekton.ARUtility.UseCase.ARAnchor.Infrastructure
{
    public class ARAnchorRepository : IARAnchorRepository
    {
        [Inject] private Presentation.ARAnchor.Factory _factory = null;

        private Dictionary<AnchorID, IARAnchor> _database = new Dictionary<AnchorID, IARAnchor>();

        IARAnchor IARAnchorRepository.Find(AnchorID anchorID) => Find(anchorID);

        IARAnchor IARAnchorRepository.Create(AnchorID anchorID)
        {
            IARAnchor anchor = Find(anchorID);

            if (anchor == null)
            {
                anchor = _factory.Create(anchorID);
                AddAnchor(anchor);
            }

            return anchor;
        }

        private IARAnchor Find(AnchorID anchorID)
        {
            if (_database.ContainsKey(anchorID))
            {
                return _database[anchorID];
            }

            return null;
        }

        private void AddAnchor(IARAnchor anchor)
        {
            if (_database.ContainsKey(anchor.ID))
            {
                return;
            }
            
            _database.Add(anchor.ID, anchor);
        }
    }
}

