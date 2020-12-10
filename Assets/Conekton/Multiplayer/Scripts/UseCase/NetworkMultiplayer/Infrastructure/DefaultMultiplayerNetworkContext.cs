using System.Collections;
using System.Collections.Generic;
using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Infrastructure
{
    public class DefaultMultiplayerNetworkContext : IMultiplayerNetworkContext
    {
        private IMultiplayerNetworkSystem _networkSystem = null;
        private IRoomOptions _roomOptions = null;
        private object _args = null;

        public bool AutoConnect => true;

        [Inject]
        private void Construct(IMultiplayerNetworkSystem system, IRoomOptions options)
        {
            _networkSystem = system;
            _roomOptions = options;
            _networkSystem.OnConnected += HandleOnConnected;
        }

        public void SetArgument(object args) => _args = args;

        public void Connect()
        {
            _networkSystem.Connect(_roomOptions);
        }

        public void Disconnect()
        {
            _networkSystem.Disconnect();
        }

        private void HandleOnConnected()
        {
            _networkSystem.CreateRemotePlayerForLocalPlayer(_args);
        }
    }
}