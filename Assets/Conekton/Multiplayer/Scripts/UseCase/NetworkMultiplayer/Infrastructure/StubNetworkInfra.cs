using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using System.Threading.Tasks;
using UnityEngine;

using Conekton.ARMultiplayer.Avatar.Domain;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Infrastructure
{
    public class StubNetworkInfra : IMultiplayerNetworkInfrastructure
    {
        public event ConnectedEvent OnServerConnected;
        public event DisconnectedEvent OnServerDisconnected;
        public event PlayerConnectedEvent OnPlayerConnected;
        public event PlayerDisconnectedEvent OnPlayerDisconnected;

        private bool _isConnected = false;

        bool IMultiplayerNetworkInfrastructure.IsConnected => _isConnected;

        void IMultiplayerNetworkInfrastructure.Connect(string roomName, IRoomOptions roomOptions)
        {
            Debug.Log("Connect to stub server.");

            DelayConnect();
        }

        void IMultiplayerNetworkInfrastructure.Disconnect()
        {
            Debug.Log("Disonnect from stub server.");

            DelayDisconnect();
        }

        IRemotePlayer IMultiplayerNetworkInfrastructure.CreateRemotePlayer(object args)
        {
            return null;
        }

        PlayerID[] IMultiplayerNetworkInfrastructure.GetAllRemotePlayerID()
        {
            return new PlayerID[0];
        }

        PlayerID IMultiplayerNetworkInfrastructure.ResolvePlayerID(object args) => default;

        PlayerID IMultiplayerNetworkInfrastructure.GetPlayerID(AvatarID avatarID) => CreatePlayerID(avatarID.ID);

        AvatarID IMultiplayerNetworkInfrastructure.GetAvatarID(PlayerID playerID) => AvatarID.NoSet;

        void IMultiplayerNetworkInfrastructure.RegisterMainAvatar(AvatarID avatarID) { }

        int IMultiplayerNetworkInfrastructure.RegisterAvatar(PlayerID playerID, AvatarID avatarID) => 1;

        int IMultiplayerNetworkInfrastructure.UnregisterAvatar(PlayerID playerID) => 0;
        void IMultiplayerNetworkInfrastructure.UnregisterMainAvatar() { }

        async private void DelayConnect()
        {
            await Task.Delay(500);

            _isConnected = true;

            OnServerConnected?.Invoke();
        }

        async private void DelayDisconnect()
        {
            await Task.Delay(100);

            _isConnected = false;

            OnServerDisconnected?.Invoke();
        }

        private PlayerID CreatePlayerID(int id)
        {
            return new PlayerID { ID = id };
        }
    }
}

