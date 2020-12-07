using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Conekton.ARMultiplayer.AvatarBody.Application;
using Conekton.ARMultiplayer.AvatarBody.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARMultiplayer.AvatarBody.Infrastructure
{
    public class AvatarBodyRepository : IAvatarBodyRepository<AvatarBodyTypeArgs>
    {
        private class BodyStore
        {
            private Dictionary<IAvatarBody, bool> _database = new Dictionary<IAvatarBody, bool>();

            public IAvatarBody Get()
            {
                return _database
                    .Where(kv => !kv.Value)
                    .Select(kv => kv.Key)
                    .FirstOrDefault();
            }

            public IAvatarBody GetByID(AvatarBodyID id)
            {
                return _database
                    .Where(kv => kv.Key.BodyID == id)
                    .Select(kv => kv.Key)
                    .FirstOrDefault();
            }

            public void Use(IAvatarBody body)
            {
                if (_database.ContainsKey(body))
                {
                    _database[body] = true;
                }
                else
                {
                    _database.Add(body, true);
                }
            }

            public void Unuse(IAvatarBody body)
            {
                if (_database.ContainsKey(body))
                {
                    _database[body] = false;
                }
            }
        }

        private Dictionary<AvatarBodyType, BodyStore> _database = new Dictionary<AvatarBodyType, BodyStore>();

        public IAvatarBody Find(AvatarBodyID id)
        {
            return _database.Values
                .Where(store => store.GetByID(id) != null)
                .Select(store => store.GetByID(id))
                .FirstOrDefault();
        }

        public IAvatarBody Get(AvatarBodyTypeArgs args)
        {
            return _database.ContainsKey(args.BodyType) ? _database[args.BodyType].Get() : null;
        }

        public void Save(AvatarBodyType bodyType, IAvatarBody avatarBody)
        {
            if (!_database.ContainsKey(bodyType))
            {
                _database.Add(bodyType, new BodyStore());
            }

            _database[bodyType].Use(avatarBody);
        }

        public void Release(IAvatarBody body)
        {
            if (!_database.ContainsKey(body.BodyType))
            {
                return;
            }

            _database[body.BodyType].Unuse(body);
        }
    }
}