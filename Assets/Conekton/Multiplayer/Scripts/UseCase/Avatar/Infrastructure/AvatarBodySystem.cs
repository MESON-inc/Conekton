using System.Collections;
using System.Collections.Generic;
using Conekton.ARMultiplayer.AvatarBody.Application;
using Conekton.ARMultiplayer.AvatarBody.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARMultiplayer.AvatarBody.Infrastructure
{
    public class AvatarBodySystem : IAvatarBodySystem<AvatarBodyTypeArgs>, IAvatarBodyIDGenerator
    {
        [Inject] private IAvatarBodyRepository<AvatarBodyTypeArgs> _avatarBodyRepository = null;
        [Inject] private AvatarBodyFactory _avatarBodyFactory = null;

        private int _avatarBodyIDIndex = -1;

        public IAvatarBody Create(AvatarBodyTypeArgs args)
        {
            AvatarBodyID id = (this as IAvatarBodyIDGenerator).Generate();

            args.BodyID = id;
            IAvatarBody body = _avatarBodyFactory.Create(args);
            body.Transform.name = $"AvatarBody - [{id.ID.ToString()}]";
            body.Transform.SetParent(null);

            return body;
        }

        public IAvatarBody Find(AvatarBodyID id)
        {
            return _avatarBodyRepository.Find(id);
        }

        public IAvatarBody Get(AvatarBodyTypeArgs args)
        {
            IAvatarBody body = _avatarBodyRepository.Get(args);

            if (body == null)
            {
                body = Create(args);
            }

            body.Active(true);
            _avatarBodyRepository.Save(args.BodyType, body);

            return body;
        }

        public void Release(IAvatarBody body)
        {
            body.Active(false);
            _avatarBodyRepository.Release(body);
        }

        AvatarBodyID IAvatarBodyIDGenerator.Generate()
        {
            return new AvatarBodyID(++_avatarBodyIDIndex);
        }
    }
}