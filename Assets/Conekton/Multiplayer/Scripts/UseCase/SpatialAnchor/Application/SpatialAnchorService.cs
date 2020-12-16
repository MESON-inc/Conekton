using System.Collections.Generic;
using UnityEngine;
using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using Conekton.ARMultiplayer.PersistentCoordinate.Domain;
using Conekton.ARMultiplayer.SpatialAnchor.Domain;

namespace Conekton.ARMultiplayer.SpatialAnchor.Application
{
    public class SpatialAnchorService : ISpatialAnchorService, IInitializable, ILateDisposable
    {
        public event CreatedAnchorEvent OnCreatedAnchor;

        [Inject] private ISpatialAnchorSystem _anchorSystem = null;
        [Inject] private IMultiplayerNetworkSystem _networkSystem = null;

        void IInitializable.Initialize()
        {
            _networkSystem.OnCreatedLocalPlayer += HandleOnCreatedLocalPlayer;
            _networkSystem.OnCreatedRemotePlayer += HandleOnCreatedRemotePlayer;
        }

        void ILateDisposable.LateDispose()
        {
            _networkSystem.OnCreatedLocalPlayer -= HandleOnCreatedLocalPlayer;
            _networkSystem.OnCreatedRemotePlayer -= HandleOnCreatedRemotePlayer;
        }

        ISpatialAnchor ISpatialAnchorService.GetOrCreateAnchor(PlayerID playerID) => _anchorSystem.GetOrCreateAnchor(playerID);

        Pose ISpatialAnchorService.GetAnchorPose(Dictionary<PCAID, Pose> comparePCAData) => _anchorSystem.GetAnchorPose(comparePCAData);

        void ISpatialAnchorService.RegisterTuner(ISpatialAnchorTuner tuner, object args)
        {
            PlayerID playerID = _networkSystem.ResolvePlayerID(args);
            _anchorSystem.RegisterTuner(tuner, playerID);
        }

        private void HandleOnCreatedLocalPlayer(IAvatar avatar, IRemotePlayer remotePlayer)
        {
            ISpatialAnchor anchor = _anchorSystem.GetOrCreateAnchor(remotePlayer.PlayerID);
            _anchorSystem.CreateTuner();

            anchor.AddTransform(avatar.Root);
        }

        private void HandleOnCreatedRemotePlayer(IAvatar avatar, IRemotePlayer remotePlayer)
        {
            ISpatialAnchor anchor = _anchorSystem.GetOrCreateAnchor(remotePlayer.PlayerID);

            anchor.AddTransform(avatar.Root);
        }
    }
}

