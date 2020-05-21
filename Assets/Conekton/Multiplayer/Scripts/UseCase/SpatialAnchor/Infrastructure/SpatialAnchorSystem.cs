using System.Collections.Generic;

using Zenject;

using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using Conekton.ARMultiplayer.SpatialAnchor.Domain;

namespace Conekton.ARMultiplayer.SpatialAnchor.Infrastructure
{
    public class SpatialAnchorSystem : ISpatialAnchorSystem
    {
        [Inject] private ISpatialAnchorRepository _anchorRepository = null;
        [Inject] private ISpatialAnchorTunerRepository _anchorTunerRepository = null;

        public event CreatedAnchorEvent OnCreatedAnchor;

        private int _index = 0;
        private Dictionary<PlayerID, SpatialAnchorID> _database = new Dictionary<PlayerID, SpatialAnchorID>();

        ISpatialAnchor ISpatialAnchorSystem.GetOrCreateAnchor(PlayerID playerID) => GetOrCreateAnchor(playerID);
        ISpatialAnchorTuner ISpatialAnchorSystem.CreateTuner() => CreateTuner();

        void ISpatialAnchorSystem.RegisterTuner(ISpatialAnchorTuner tuner, PlayerID playerID) => RegisterTuner(tuner, playerID);

        private void RegisterTuner(ISpatialAnchorTuner tuner, PlayerID playerID)
        {
            ISpatialAnchor anchor = GetOrCreateAnchor(playerID);
            tuner.BindAnchor(anchor);
        }

        private ISpatialAnchor GetOrCreateAnchor(PlayerID playerID)
        {
            if (_database.ContainsKey(playerID))
            {
                return _anchorRepository.Find(_database[playerID]);
            }

            return CreateAnchor(playerID);
        }

        private ISpatialAnchor CreateAnchor(PlayerID playerID)
        {
            SpatialAnchorID anchorID = CreateNewID(playerID);
            _database.Add(playerID, anchorID);

            ISpatialAnchor anchor = _anchorRepository.Create(anchorID);

            OnCreatedAnchor?.Invoke(anchor);

            return anchor;
        }

        private ISpatialAnchorTuner CreateTuner()
        {
            return _anchorTunerRepository.Create();
        }

        private SpatialAnchorID CreateNewID(PlayerID playerID)
        {
            int id = _index++;

            return new SpatialAnchorID
            {
                ID = id,
                PlayerID = playerID,
            };
        }
    }
}

