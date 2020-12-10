using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Infrastructure
{
    public class MultiplayerNetworkSystem : IMultiplayerNetworkSystem, IInitializable, ILateDisposable
    {
        [Inject] private IMultiplayerNetworkInfrastructure _infra = null;
        [Inject] private IAvatarService _avatarService = null;

        public event ConnectedEvent OnConnected;
        public event DisconnectedEvent OnDisconnected;
        public event CreatedRemotePlayerEvent OnCreatedRemotePlayer;
        public event CreatedLocalPlayerEvent OnCreatedLocalPlayer;
        public event DestroyedRemotePlayerEvent OnDestroyedRemotePlayer;
        public event ReceivedRemotePlayerCustomDataEvent OnReceivedRemotePlayerCustomData;

        private IAvatarController _mainAvatarController = null;

        #region ### for Zenject interfaces ###

        void IInitializable.Initialize()
        {
            _infra.OnServerConnected += HandleOnConnected;
            _infra.OnServerDisconnected += HandleOnDisconnected;
            _infra.OnPlayerConnected += HandlePlayerConnected;
            _infra.OnPlayerDisconnected += HandlePlayerDisconnected;
        }

        void ILateDisposable.LateDispose()
        {
            _infra.OnServerConnected -= HandleOnConnected;
            _infra.OnServerDisconnected -= HandleOnDisconnected;
            _infra.OnPlayerConnected -= HandlePlayerConnected;
            _infra.OnPlayerDisconnected -= HandlePlayerDisconnected;
        }
        #endregion ### for Zenject interfaces ###

        private void HandlePlayerConnected(PlayerID playerID)
        {
            Debug.Log($"New player has connected {playerID}");
        }

        private void HandlePlayerDisconnected(PlayerID playerID)
        {
            Debug.Log($"Other player has disconnected {playerID}");
        }

        private void HandleOnConnected()
        {
            OnConnected?.Invoke();
        }

        private void HandleOnDisconnected()
        {
            DestoryLocalPlayer();

            OnDisconnected?.Invoke();
        }

        #region ### for IMultiplayerNetworkSystem interfaces ###
        bool IMultiplayerNetworkSystem.IsConnected => _infra.IsConnected;

        void IMultiplayerNetworkSystem.Connect(IRoomOptions roomOptions)
        {
            _infra.Connect(roomOptions);
        }

        void IMultiplayerNetworkSystem.Disconnect()
        {
            _infra.Disconnect();
        }

        void IMultiplayerNetworkSystem.CreatedRemotePlayer(IRemotePlayer remotePlayer, object args) => CreatedRemotePlayer(remotePlayer, args);
        void IMultiplayerNetworkSystem.CreateRemotePlayerLocalPlayer(IRemotePlayer remotePlayer, object args) => CreateRemotePlayerLocalPlayer(remotePlayer, args);
        void IMultiplayerNetworkSystem.RemoveRemotePlayer(IRemotePlayer remotePlayer) => RemoveRemotePlayer(remotePlayer.PlayerID);
        PlayerID IMultiplayerNetworkSystem.ResolvePlayerID(object args) => _infra.ResolvePlayerID(args);
        PlayerID IMultiplayerNetworkSystem.GetPlayerID(AvatarID avatarID) => _infra.GetPlayerID(avatarID);
        AvatarID IMultiplayerNetworkSystem.GetAvatarID(PlayerID playerID) => _infra.GetAvatarID(playerID);
        #endregion ### for IMultiplayerNetworkSystem interfaces ###

        private IAvatar GetOrCreateAvatar(PlayerID playerID)
        {
            AvatarID avatarID = _infra.GetAvatarID(playerID);
            IAvatar avatar = _avatarService.Find(avatarID);

            if (avatar == null)
            {
                avatar = _avatarService.Create();
            }

            _infra.RegisterAvatar(playerID, avatar.AvatarID);

            return avatar;
        }

        void IMultiplayerNetworkSystem.CreateRemotePlayerForLocalPlayer(object args)
        {
            Debug.Log("Will create Remote Player for Local Player.");

            IAvatar avatar = _avatarService.GetMain();

            if (_mainAvatarController == null)
            {
                _mainAvatarController = avatar.AvatarController;
            }

            _infra.RegisterMainAvatar(avatar.AvatarID);
            _infra.CreateRemotePlayer(args);
        }

        public void ReceivedRemotePlayerCustomData(IRemotePlayer remotePlayer, object args)
        {
            OnReceivedRemotePlayerCustomData?.Invoke(remotePlayer, args);
        }

        private void CreateRemotePlayerLocalPlayer(IRemotePlayer remotePlayer, object args)
        {
            PlayerID pid = _infra.ResolvePlayerID(args);
            IAvatar avatar = _avatarService.GetMain();

            remotePlayer.PlayerID = pid;
            remotePlayer.OnDestroyingRemotePlayer += HandleRemotePlayerDestroying;
            remotePlayer.SetTargetAvatarController(_mainAvatarController);
            avatar.SetAvatarController(remotePlayer);

            OnCreatedLocalPlayer?.Invoke(avatar, remotePlayer);
        }

        private void CreatedRemotePlayer(IRemotePlayer remotePlayer, object args)
        {
            PlayerID pid = _infra.ResolvePlayerID(args);
            IAvatar avatar = GetOrCreateAvatar(pid);
            avatar.SetAvatarController(remotePlayer);

            remotePlayer.PlayerID = pid;
            remotePlayer.OnDestroyingRemotePlayer += HandleRemotePlayerDestroying;

            OnCreatedRemotePlayer?.Invoke(avatar, remotePlayer);
        }

        private void RemoveRemotePlayer(PlayerID playerID)
        {
            AvatarID avatarID = _infra.GetAvatarID(playerID);

            Debug.Log($"Will remove a remote player ({playerID}) and Avatar {avatarID}");

            if (_infra.UnregisterAvatar(playerID) == 0)
            {
                RemoveAvatarIfNeeded(avatarID);
            }
        }

        private void RemoveAvatarIfNeeded(AvatarID avatarID)
        {
            AvatarID mainID = _avatarService.GetMain().AvatarID;

            if (mainID != avatarID)
            {
                _avatarService.Remove(avatarID);
            }
        }

        private void HandleRemotePlayerDestroying(IRemotePlayer remotePlayer)
        {
            remotePlayer.OnDestroyingRemotePlayer -= HandleRemotePlayerDestroying;

            AvatarID avatarID = _infra.GetAvatarID(remotePlayer.PlayerID);
            IAvatar avatar = _avatarService.Find(avatarID);

            OnDestroyedRemotePlayer?.Invoke(avatar, remotePlayer);

            RemoveRemotePlayer(remotePlayer.PlayerID);
        }

        private void DestoryLocalPlayer()
        {
            _infra.UnregisterMainAvatar();
        }
    }
}