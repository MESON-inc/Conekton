using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Zenject;

using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using Conekton.ARMultiplayer.SpatialAnchor.Domain;

namespace Conekton.ARMultiplayer.SpatialAnchor.Infrastructure
{
    public class PhotonSpatialAnchorTunerRepository : ISpatialAnchorTunerRepository
    {
        private readonly string _tunerPath = "SpatialAnchor/PhotonSpatialAnchorTuner";

        [Inject] private IMultiplayerNetworkSystem _networkSystem = null;

        ISpatialAnchorTuner ISpatialAnchorTunerRepository.Create()
        {
            if (!_networkSystem.IsConnected)
            {
                Debug.LogError("Network haven't connected yet.");
                return null;
            }

            GameObject go = PhotonNetwork.Instantiate(_tunerPath, Vector3.zero, Quaternion.identity, 0);
            return go.GetComponent<ISpatialAnchorTuner>();
        }
    }
}

