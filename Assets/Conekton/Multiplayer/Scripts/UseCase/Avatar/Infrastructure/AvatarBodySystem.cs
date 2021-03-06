using System.Collections;
using System.Collections.Generic;
using Conekton.ARMultiplayer.AvatarBody.Application;
using Conekton.ARMultiplayer.AvatarBody.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARMultiplayer.AvatarBody.Infrastructure
{
    public class AvatarBodySystem : IAvatarBodySystem, IAvatarBodyIDGenerator
    {
        [Inject] private IAvatarBodyRepository _avatarBodyRepository = null;
        [Inject] private AvatarBodyFactory _avatarBodyFactory = null;

        private int _avatarBodyIDIndex = -1;

        public IAvatarBody Create(byte bodyType)
        {
            AvatarBodyID id = (this as IAvatarBodyIDGenerator).Generate();

            CreateAvatarBodyArgs args = new CreateAvatarBodyArgs
            {
                BodyType = bodyType,
                BodyID = id,
            };

            IAvatarBody body = _avatarBodyFactory.Create(args);

            body.Transform.name = $"AvatarBody({args.BodyType.ToString()}) - [{id.ID.ToString()}]";
            body.Transform.SetParent(null);
            body.OnAvatarBodyFree += HandleOnAvatarBodyFree;

            return body;
        }

        private void HandleOnAvatarBodyFree(IAvatarBody avatarBody)
        {
            Release(avatarBody);
        }

        public IAvatarBody Find(AvatarBodyID id)
        {
            return _avatarBodyRepository.Find(id);
        }

        public IAvatarBody GetOrCreate(byte bodyType)
        {
            IAvatarBody body = _avatarBodyRepository.Get(bodyType);
        
            if (body == null)
            {
                body = Create(bodyType);
            }
        
            _avatarBodyRepository.Save(bodyType, body);
        
            return body;
        }

        public void Release(IAvatarBody body)
        {
            if (body == null)
            {
                return;
            }

            if (body is MonoBehaviour missingCheck)
            {
                if (missingCheck == null)
                {
                    return;
                }
            }

            body.Active(false);
            _avatarBodyRepository.Release(body);
        }

        AvatarBodyID IAvatarBodyIDGenerator.Generate()
        {
            return new AvatarBodyID(++_avatarBodyIDIndex);
        }
    }
}