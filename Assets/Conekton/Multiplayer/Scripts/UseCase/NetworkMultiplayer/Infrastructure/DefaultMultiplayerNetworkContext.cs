using System.Collections;
using System.Collections.Generic;
using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Infrastructure
{
    public class DefaultMultiplayerNetworkContext : IMultiplayerNetworkContext, IInitializable
    {
        private IMultiplayerNetworkSystem _networkSystem = null;
        private IRoomOptions _roomOptions = null;
        private string _roomName = "";

        public bool AutoConnect { get; private set; } = true;

        [Inject]
        private void Construct(IMultiplayerNetworkSystem system, IRoomOptions options)
        {
            _networkSystem = system;
            _roomOptions = options;
            _roomName = _roomOptions.DefaultRoomName;
            _networkSystem.OnConnected += HandleOnConnected;
        }

        public DefaultMultiplayerNetworkContext(bool autoConnect)
        {
            AutoConnect = autoConnect;
        }

        void IInitializable.Initialize()
        {
            if (AutoConnect)
            {
                Connect();
            }
        }

        public NetworkArgs Args { get; set; } = new NetworkArgs();
        
        public void SetRoomName(string roomName) => _roomName = roomName;

        public void Connect()
        {
            _networkSystem.Connect(_roomName, _roomOptions);
        }

        public void Disconnect()
        {
            _networkSystem.Disconnect();
        }

        private void HandleOnConnected()
        {
            _networkSystem.CreateRemotePlayerForLocalPlayer(Args);
        }
    }
}