using Conekton.ARMultiplayer.Avatar.Domain;
using UnityEngine;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Domain
{
    public delegate void ConnectedEvent();
    public delegate void DisconnectedEvent();
    public delegate void PlayerConnectedEvent(PlayerID playerID);
    public delegate void PlayerDisconnectedEvent(PlayerID playerID);
    public delegate void CreatedRemotePlayerEvent(IAvatar avatar, IRemotePlayer remotePlayer);
    public delegate void CreatedLocalPlayerEvent(IAvatar avatar, IRemotePlayer remotePlayer);
    public delegate void DestroyedRemotePlayerEvent(IAvatar avatar, IRemotePlayer remotePlayer);
    public delegate void DestroyingRemotePlayerEvent(IRemotePlayer remotePlayer);

    public struct PlayerID
    {
        static public PlayerID NoSet = new PlayerID { ID = -1 };

        public int ID;

        static public bool operator ==(PlayerID a, PlayerID b)
        {
            return a.ID == b.ID;
        }

        static public bool operator !=(PlayerID a, PlayerID b)
        {
            return a.ID != b.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return (PlayerID)obj == this;
        }

        public override string ToString()
        {
            return $"Player ID - [{ID}]";
        }
    }

    public interface IPoseProvider
    {
        Pose GetHeadPose();
        Pose GetHandPose(AvatarPoseType type);
        void SetHeadPose(Pose pose);
        void SetHandPose(AvatarPoseType type, Pose pose);
    }

    public interface IRemotePlayer : IAvatarController
    {
        event DestroyingRemotePlayerEvent OnDestroyingRemotePlayer;
        PlayerID PlayerID { get; set; }
        void SetTargetAvatarController(IAvatarController avatarController);
    }

    public interface IMultiplayerNetworkSystem
    {
        event ConnectedEvent OnConnected;
        event DisconnectedEvent OnDisconnected;
        event CreatedRemotePlayerEvent OnCreatedRemotePlayer;
        event CreatedLocalPlayerEvent OnCreatedLocalPlayer;
        event DestroyedRemotePlayerEvent OnDestroyedRemotePlayer;
        bool IsConnected { get; }
        void Connect();
        void Disconnect();
        PlayerID ResolvePlayerID(object args);
        PlayerID GetPlayerID(AvatarID avatarID);
        AvatarID GetAvatarID(PlayerID playerID);
        void CreateRemotePlayerLocalPlayer(IRemotePlayer remotePlayer, object args);
        void CreatedRemotePlayer(IRemotePlayer remotePlayer, object args);
        void RemoveRemotePlayer(IRemotePlayer remotePlayer);
    }

    public interface IMultiplayerNetworkInfrastructure
    {
        event ConnectedEvent OnServerConnected;
        event DisconnectedEvent OnServerDisconnected;
        event PlayerConnectedEvent OnPlayerConnected;
        event PlayerDisconnectedEvent OnPlayerDisconnected;
        bool IsConnected { get; }
        void Connect();
        void Disconnect();
        IRemotePlayer CreateRemotePlayer();
        PlayerID[] GetAllRemotePlayerID();
        PlayerID GetPlayerID(AvatarID avatarID);
        /// <summary>
        /// This interface depend on a platform.
        /// 
        /// This interface purpose to use resolving PlayerID from object that depend on a platform.
        /// </summary>
        PlayerID ResolvePlayerID(object args);
        AvatarID GetAvatarID(PlayerID playerID);
        void RegisterMainAvatar(AvatarID avatarID);
        /// <summary>
        /// Register interface will return reference count of an avatar.
        /// </summary>
        int RegisterAvatar(PlayerID playerID, AvatarID avatarID);
        /// <summary>
        /// Unregister interface will return reference count of an avatar.
        /// </summary>
        int UnregisterAvatar(PlayerID playerID);
        void UnregisterMainAvatar();
    }

    public interface IMultiplayerNetworkIDRepository { }
}

