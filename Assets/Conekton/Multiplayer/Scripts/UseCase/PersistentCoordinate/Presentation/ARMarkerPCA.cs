using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARMultiplayer.PersistentCoordinate.Domain;

namespace Conekton.ARMultiplayer.PersistentCoordinate.Infrastructure
{
    public class ARMarkerPCA : IPCA
    {
        private Transform _target = null;
        private string _uniqueID = "";
        private Priority _priority = Priority.Low;

        public ARMarkerPCA(Transform target, string uniqueID, Priority priority)
        {
            _target = target;
            _uniqueID = uniqueID;
            _priority = priority;
        }

        PCAID IPCA.ID { get; set; }

        string IPCA.UniqueID => _uniqueID;

        Priority IPCA.Priority => _priority;

        Vector3 IPCA.Position => _target.position;

        Quaternion IPCA.Rotation => _target.rotation;
    }
}

