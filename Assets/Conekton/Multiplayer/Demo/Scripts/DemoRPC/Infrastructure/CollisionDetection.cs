using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARMultiplayer.Demo.RPC.Domain;

namespace Conekton.ARMultiplayer.Demo.RPC.Application
{
    public class CollisionDetection : MonoBehaviour
    {
        public event System.Action<CollisionDetection> OnTriggerEnterEvent;
        public event System.Action<CollisionDetection> OnTriggerStayEvent;
        public event System.Action<CollisionDetection> OnTriggerExitEvent;

        [SerializeField] private DemoID _demoID = default;

        public DemoID DemoID => _demoID;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(this);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayEvent?.Invoke(this);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitEvent?.Invoke(this);
        }
    }
}

