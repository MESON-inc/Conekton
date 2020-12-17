using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using ExitGames.Client.Photon;
using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using UnityEngine;
using Photon.Realtime;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Infrastructure
{
    static public class PlayerIDSerializer
    {
        static public object Deserialize(byte[] data)
        {
            int i1 = (data[0] << 0);
            int i2 = (data[1] << 8);
            int i3 = (data[2] << 16);
            int i4 = (data[3] << 24);

            PlayerID result = new PlayerID();
            result.ID = i1 + i2 + i3 + i4;

            return result;
        }

        static public byte[] Serialize(object customType)
        {
            PlayerID pid = (PlayerID)customType;

            int id = pid.ID;

            byte b1 = (byte)((id >> 0) & 0xff);
            byte b2 = (byte)((id >> 8) & 0xff);
            byte b3 = (byte)((id >> 16) & 0xff);
            byte b4 = (byte)((id >> 24) & 0xff);

            return new byte[] { b1, b2, b3, b4, };
        }
    }

    public class AvatarReferenceData
    {
        public int Count = 0;
        public AvatarID AvatarID = AvatarID.NoSet;
    }

    public class PhotonNetworkInfra : MonoBehaviourPunCallbacks, IMultiplayerNetworkInfrastructure
    {
        [SerializeField] private string _remotePlayerPath = "Multiplayer/PhotonRemotePlayer";

        private bool _needsToReconnect = false;
        private string _roomName = "";
        private IRoomOptions _roomOptions = null;

        public event ConnectedEvent OnServerConnected;
        public event DisconnectedEvent OnServerDisconnected;
        public event PlayerConnectedEvent OnPlayerConnected;
        public event PlayerDisconnectedEvent OnPlayerDisconnected;

        private Dictionary<PlayerID, AvatarReferenceData> _database = new Dictionary<PlayerID, AvatarReferenceData>();
        
        bool IMultiplayerNetworkInfrastructure.IsMaster => PhotonNetwork.IsMasterClient;

        bool IMultiplayerNetworkInfrastructure.IsConnected => PhotonNetwork.IsConnected;

        void IMultiplayerNetworkInfrastructure.Connect(string roomName, IRoomOptions roomOptions)
        {
            Debug.Log("Will connect to a Photon server.");

            _roomName = roomName;
            _roomOptions = roomOptions;

            PhotonNetwork.ConnectUsingSettings();
        }

        void IMultiplayerNetworkInfrastructure.Disconnect()
        {
            Debug.Log("Will disconnect from a Photon server.");

            PhotonNetwork.Disconnect();
        }

        IRemotePlayer IMultiplayerNetworkInfrastructure.CreateRemotePlayer(object args)
        {
            GameObject remoteGO = PhotonNetwork.Instantiate(_remotePlayerPath, Vector3.zero, Quaternion.identity, 0, new object[] { args });
            return remoteGO.GetComponent<IRemotePlayer>();
        }

        PlayerID[] IMultiplayerNetworkInfrastructure.GetAllRemotePlayerID()
        {
            return PhotonNetwork.PlayerList.Where(p => !p.IsLocal).Select(p => CreatePlayerID(p.ActorNumber)).ToArray();
        }

        PlayerID IMultiplayerNetworkInfrastructure.ResolvePlayerID(object args)
        {
            PhotonView view = args as PhotonView;
            int remoteID = view.Owner.ActorNumber;
            return CreatePlayerID(remoteID);
        }

        PlayerID IMultiplayerNetworkInfrastructure.GetPlayerID(AvatarID avatarID)
        {
            foreach (var key in _database.Keys)
            {
                if (_database[key].AvatarID == avatarID)
                {
                    return key;
                }
            }

            return PlayerID.NoSet;
        }

        AvatarID IMultiplayerNetworkInfrastructure.GetAvatarID(PlayerID playerID)
        {
            if (_database.ContainsKey(playerID))
            {
                return _database[playerID].AvatarID;
            }

            return AvatarID.NoSet;
        }

        void IMultiplayerNetworkInfrastructure.RegisterMainAvatar(AvatarID avatarID)
        {
            PlayerID pid = CreatePlayerID(PhotonNetwork.LocalPlayer.ActorNumber);
            RegisterAvatar(pid, avatarID);
        }

        int IMultiplayerNetworkInfrastructure.RegisterAvatar(PlayerID playerID, AvatarID avatarID) => RegisterAvatar(playerID, avatarID);

        int IMultiplayerNetworkInfrastructure.UnregisterAvatar(PlayerID playerID) => UnregisterAvatar(playerID);

        void IMultiplayerNetworkInfrastructure.UnregisterMainAvatar()
        {
            PlayerID pid = CreatePlayerID(PhotonNetwork.LocalPlayer.ActorNumber);
            UnregisterAvatar(pid);
        }

        #region ### MonoBehaviour ###
        private void Start()
        {
            SetupPhoton();
        }

        private void Update()
        {
            if (_needsToReconnect)
            {
                if (PhotonNetwork.ReconnectAndRejoin())
                {
                    _needsToReconnect = false;
                }
            }
        }

        private void OnDestroy()
        {
            (this as IMultiplayerNetworkInfrastructure).Disconnect();
        }

        #endregion ### MonoBehaviour ###

        #region ### For Photon ###
        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby");

            JoinOrCreateRoom();
        }

        private void OnPhotonRandomJoinFailed()
        {
            Debug.Log("OnPhotonRandomJoinFailed");

            // Will create a room. If connection failed, it means master client is not found.
            PhotonNetwork.CreateRoom(null);
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");

            JoinOrCreateRoom();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");

            OnServerConnected?.Invoke();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnected from a Photon server.");

            _needsToReconnect = true;

            OnServerDisconnected?.Invoke();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"On Photon Player connected ID {newPlayer.ActorNumber}.");

            OnPlayerConnected?.Invoke(CreatePlayerID(newPlayer.ActorNumber));
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"On Photon Player disconnected ID {otherPlayer.ActorNumber}");

            OnPlayerDisconnected?.Invoke(CreatePlayerID(otherPlayer.ActorNumber));
        }
        #endregion ### For Photon ###

        private void JoinOrCreateRoom()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.PlayerTtl = _roomOptions.PlayerTtl;
            roomOptions.EmptyRoomTtl = _roomOptions.EmptyRoomTtl;

            Debug.Log($"Will enter {_roomName} with options, PlayerTtl {_roomOptions.PlayerTtl.ToString()}, EmptyRoomTtl {_roomOptions.EmptyRoomTtl.ToString()}");

            PhotonNetwork.JoinOrCreateRoom(_roomName, roomOptions, TypedLobby.Default);
        }

        private void SetupPhoton()
        {
            PhotonNetwork.SendRate = 10;

            RegisterPlayerIDTypeToPhoton();
        }

        private void RegisterPlayerIDTypeToPhoton()
        {
            PhotonPeer.RegisterType(typeof(PlayerID), (byte)'D', PlayerIDSerializer.Serialize, PlayerIDSerializer.Deserialize);
        }

        private int RegisterAvatar(PlayerID playerID, AvatarID avatarID)
        {
            if (_database.ContainsKey(playerID))
            {
                return ++_database[playerID].Count;
            }
            else
            {
                _database.Add(playerID, new AvatarReferenceData { Count = 1, AvatarID = avatarID, });
                return _database[playerID].Count;
            }
        }

        private int UnregisterAvatar(PlayerID playerID)
        {
            if (!_database.ContainsKey(playerID))
            {
                return 0;
            }

            int count = --_database[playerID].Count;

            if (count == 0)
            {
                _database.Remove(playerID);
            }

            return count;
        }

        private PlayerID CreatePlayerID(int id)
        {
            return new PlayerID { ID = id };
        }
    }
}

