using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;

using Conekton.ARMultiplayer.Demo.RPC.Domain;
using Conekton.ARMultiplayer.Demo.RPC.Application;

namespace Conekton.ARMultiplayer.Demo.RPC.Infrastructure
{
    public class DemoRPCService : IDemoRPCService, IInitializable, ITickable
    {
        [Inject] private IAvatarSystem _avatarSystem = null;
        [Inject] private DemoRPCNetwork _demoRPCNetwork = null;
        [Inject] private IMultiplayerNetworkSystem _networkSystem = null;

        private CollisionDetection _lastDetection = null;
        private float _timer = 0;
        private readonly float _limit = 2f;
        private bool _timerStart = false;

        private Dictionary<DemoID, CollisionDetection> _database = new Dictionary<DemoID, CollisionDetection>();

        void IInitializable.Initialize()
        {
            CollisionDetection[] detections = GameObject.FindObjectsOfType<CollisionDetection>();

            foreach (var d in detections)
            {
                d.OnTriggerEnterEvent += HandleTriggerEnter;
                d.OnTriggerExitEvent += HandleTriggerExit;

                _database.Add(d.DemoID, d);
            }

            ResetTimer();
        }

        void ITickable.Tick()
        {
            if (_timerStart)
            {
                if (CountDown())
                {
                    Get();
                }
            }
        }

        void IDemoRPCService.Set(IAvatar avatar, DemoID demoID)
        {
            if (!_database.ContainsKey(demoID))
            {
                return;
            }

            CollisionDetection detection = _database[demoID];
            IAvatarWearable wearable = detection.GetComponent<IAvatarWearable>();
            avatar.SetWearable(wearable);

            detection.GetComponent<Collider>().enabled = false;
        }

        private void Get()
        {
            Debug.Log($"Get {_lastDetection.name}");

            AvatarID avatarID = _avatarSystem.CreateMain().AvatarID;
            IAvatar avatar = _avatarSystem.Find(avatarID);

            DemoID demoID = _lastDetection.DemoID;

            (this as IDemoRPCService).Set(avatar, demoID);

            _demoRPCNetwork.SendID(avatarID, demoID);
        }

        private void HandleTriggerEnter(CollisionDetection detection)
        {
            Debug.Log("On Trigger");

            _lastDetection = detection;
            _timerStart = true;
            ResetTimer();
        }

        private void HandleTriggerExit(CollisionDetection detection)
        {
            Debug.Log("On Exit");

            _lastDetection = null;
            _timerStart = false;
            ResetTimer();
        }

        private bool CountDown()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _timerStart = false;
                return true;
            }

            return false;
        }

        private void ResetTimer()
        {
            _timer = _limit;
        }
    }
}
