using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using Conekton.ARMultiplayer.Demo.RPC.Domain;

namespace Conekton.ARMultiplayer.Demo.RPC.Infrastructure
{
    public class DemoRPCNetwork : MonoBehaviourPun
    {
        [Inject] private IDemoRPCService _service = null;
        [Inject] private IMultiplayerNetworkSystem _networkSystem = null;
        [Inject] private IAvatarService _avatarService = null;

        public void SendID(AvatarID avatarID, DemoID demoID)
        {
            PlayerID playerID = _networkSystem.GetPlayerID(avatarID);
            photonView.RPC(nameof(RpcTest), RpcTarget.Others, playerID, demoID.ID);
        }

        [PunRPC]
        private void RpcTest(PlayerID playerID, int demoID)
        {
            AvatarID aid = _networkSystem.GetAvatarID(playerID);
            DemoID did = new DemoID { ID = demoID };
            IAvatar avatar = _avatarService.Find(aid);
            _service.Set(avatar, did);
        }
    }
}

