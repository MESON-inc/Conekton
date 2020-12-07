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
        private Dictionary<AvatarBodyType, List<IAvatarBody>> _database = new Dictionary<AvatarBodyType, List<IAvatarBody>>();
        private List<IAvatarBody> _workList = new List<IAvatarBody>();
        
        public IAvatarBody Find(AvatarBodyID id)
        {
            _workList.Clear();
            return _database.Values
                .Aggregate(_workList, (a, b) =>
                {
                    a.AddRange(b);
                    return a;
                })
                .FirstOrDefault(body => body.BodyID == id);
        }

        public void Save(AvatarBodyType bodyType, IAvatarBody avatarBody)
        {
            IAvatarBody body = Find(avatarBody.BodyID);
            if (body != null)
            {
                return;
            }

            if (!_database.ContainsKey(bodyType))
            {
                _database.Add(bodyType, new List<IAvatarBody>());
            }
            
            _database[bodyType].Add(avatarBody);
        }
    }
}