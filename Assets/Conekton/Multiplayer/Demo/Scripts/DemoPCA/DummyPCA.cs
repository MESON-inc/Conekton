using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARMultiplayer.PersistentCoordinate.Domain;

namespace Conekton.ARMultiplayer.PersistentCoordinate.Mock
{
    public class DummyPCA : MonoBehaviour, IPCA
    {
        private string _uniqueID = "";

        PCAID IPCA.ID { get; set; }

        string IPCA.UniqueID => _uniqueID;

        Priority IPCA.Priority => Priority.Low;

        Vector3 IPCA.Position => transform.position;

        Quaternion IPCA.Rotation => transform.rotation;

        public void SetUniqueID(string id)
        {
            _uniqueID = id;
        }
    }
}

