using System.Collections;
using System.Collections.Generic;
using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using UnityEngine;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Infrastructure
{
    public class DefaultMultiplayerNetworkContext : IMultiplayerNetworkContext
    {
        private IMultiplayerNetworkSystem _networkSystem = null;
        private object _args = null;

        public bool AutoConnect => true;

        public void SetSystem(IMultiplayerNetworkSystem system)
        {
            _networkSystem = system;
        }

        public void SetArgument(object args)
        {
            _args = args;
        }

        public void Connect()
        {
            // No need to connect because the system will connect to the server automatically.
        }

        public void OnConnected()
        {
            _networkSystem.CreateRemotePlayerForLocalPlayer(_args);
        }
    }
}