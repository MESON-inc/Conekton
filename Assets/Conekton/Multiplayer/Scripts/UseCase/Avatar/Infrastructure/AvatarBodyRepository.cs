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
    public class AvatarBodyRepository : IAvatarBodyRepository
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
                
                body.Active(true);
            }

            public void Unuse(IAvatarBody body)
            {
                if (_database.ContainsKey(body))
                {
                    _database[body] = false;
                }
                
                body.Active(false);
            }
        }

        private Dictionary<byte, BodyStore> _database = new Dictionary<byte, BodyStore>();

        /// <summary>
        /// Find an avatar body with AvatarBodyID
        /// </summary>
        /// <param name="id">An avatar body ID for finding.</param>
        /// <returns>Instance of an IAvatarBody</returns>
        public IAvatarBody Find(AvatarBodyID id)
        {
            return _database.Values
                .Where(store => store.GetByID(id) != null)
                .Select(store => store.GetByID(id))
                .FirstOrDefault();
        }

        /// <summary>
        /// Get an unused avatar body.
        /// </summary>
        /// <param name="bodyType">Avatar body type</param>
        /// <returns>IAvatarBody instance.</returns>
        public IAvatarBody Get(byte bodyType)
        {
            return _database.ContainsKey(bodyType) ? _database[bodyType].Get() : null;
        }

        /// <summary>
        /// Save an avatar body with body type.
        /// </summary>
        /// <param name="bodyType">Body type.</param>
        /// <param name="avatarBody">Instance of an avatar body.</param>
        public void Save(byte bodyType, IAvatarBody avatarBody)
        {
            if (!_database.ContainsKey(bodyType))
            {
                _database.Add(bodyType, new BodyStore());
            }

            _database[bodyType].Use(avatarBody);
        }

        /// <summary>
        /// Release an avatar body.
        /// </summary>
        /// <param name="body"></param>
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