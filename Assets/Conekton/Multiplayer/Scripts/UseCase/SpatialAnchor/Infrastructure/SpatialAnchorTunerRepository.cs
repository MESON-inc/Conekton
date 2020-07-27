using UnityEngine;

using Conekton.ARMultiplayer.SpatialAnchor.Domain;

namespace Conekton.ARMultiplayer.SpatialAnchor.Infrastructure
{
    public class SpatialAnchorTunerRepository : MonoBehaviour, ISpatialAnchorTunerRepository
    {
        [SerializeField] private GameObject _anchorTunerPrefab = null;

        ISpatialAnchorTuner ISpatialAnchorTunerRepository.Create()
        {
            GameObject go = GameObject.Instantiate(_anchorTunerPrefab);
            return go.GetComponent<ISpatialAnchorTuner>();
        }
    }
}

