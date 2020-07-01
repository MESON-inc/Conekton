using Conekton.ARMultiplayer.Avatar.Domain;
using UnityEngine;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Domain
{
    /// <summary>
    /// This event will invoke when a client has connected to the server.
    /// </summary>
    public delegate void ConnectedEvent();

    /// <summary>
    /// This event will invoke when a client has disconnected from the server.
    /// </summary>
    public delegate void DisconnectedEvent();

    /// <summary>
    /// This event will invoke when any player has connected to the server.
    /// </summary>
    /// <param name="playerID">Connected player ID.</param>
    public delegate void PlayerConnectedEvent(PlayerID playerID);

    /// <summary>
    /// This event will invoke when any player has been disconnected from the server.
    /// </summary>
    /// <param name="playerID">Disconnected player ID.</param>
    public delegate void PlayerDisconnectedEvent(PlayerID playerID);

    /// <summary>
    /// This event will invoke when a remote player object has been created.
    /// </summary>
    /// <param name="avatar">Reference of an avatar that is controled by remote player.</param>
    /// <param name="remotePlayer">A remote player that created the avatar.</param>
    public delegate void CreatedRemotePlayerEvent(IAvatar avatar, IRemotePlayer remotePlayer);

    /// <summary>
    /// This event will invoke when a local player object has been created.
    /// </summary>
    /// <param name="avatar">Reference of the local avatar.</param>
    /// <param name="remotePlayer">A remote player object that is used syncing between a local player and a remote player.</param>
    public delegate void CreatedLocalPlayerEvent(IAvatar avatar, IRemotePlayer remotePlayer);

    /// <summary>
    /// This event will invoke when a remote player object has been destroyed.
    /// </summary>
    /// <param name="avatar">Destroyed avatar object.</param>
    /// <param name="remotePlayer">A remote player that controled the destroyed avatar.</param>
    public delegate void DestroyedRemotePlayerEvent(IAvatar avatar, IRemotePlayer remotePlayer);

    /// <summary>
    /// This event will invoke when a remote player is being destroyed.
    /// 
    /// This is because some developers want to know about a remote player is being destroyed so the developers can control something before the object is destroyed. 
    /// </summary>
    /// <param name="remotePlayer"></param>
    public delegate void DestroyingRemotePlayerEvent(IRemotePlayer remotePlayer);

    /// <summary>
    /// This is a PlayerID data struct.
    /// </summary>
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

    /// <summary>
    /// IRemotePlayer present ways to control a remote player's avatar.
    /// </summary>
    public interface IRemotePlayer : IAvatarController
    {
        event DestroyingRemotePlayerEvent OnDestroyingRemotePlayer;
        PlayerID PlayerID { get; set; }
        void SetTargetAvatarController(IAvatarController avatarController);
    }

    /// <summary>
    /// IMultiplayerNetworkSystem is the main system for the networking.
    /// 
    /// The class that implement this interface will control network status with IMultiplayerNetworkInfrastructure.
    /// </summary>
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

    /// <summary>
    /// IMultiplayerNetworkInfrastructure is abstructed ways to access each network infrastructure such as Photon.
    /// </summary>
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

