using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARMultiplayer.PersistentCoordinate.Domain;
using Zenject;

namespace Conekton.ARMultiplayer.PersistentCoordinate.Infrastructure
{
    public class FixedPCA : MonoBehaviour, IPCA
    {
        [Inject] private IPersistentCoordinateService _persistentCoordinateService = null;
        
        [SerializeField] private Priority _priority = Priority.High;
        [SerializeField] private string _uniqueID = "";
        
        private void Awake()
        {
            if (string.IsNullOrEmpty(_uniqueID))
            {
                Debug.LogWarning("Unique ID is not set. This may be problem for calculating persistent coordinate.", gameObject);
            }
            else
            {
                _persistentCoordinateService.Register(this);
            }
        }

        PCAID IPCA.ID { get; set; }

        string IPCA.UniqueID => _uniqueID;

        Priority IPCA.Priority => _priority;

        Vector3 IPCA.Position => transform.position;

        Quaternion IPCA.Rotation => transform.rotation;
    }
}

