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
        [Inject] private IAvatarBodyRepository _avatarBodyRepository = null;
        [Inject] private AvatarBodyFactory _avatarBodyFactory = null;

        private int _avatarBodyIDIndex = -1;
        
        public IAvatarBody Create(AvatarBodyTypeArgs args)
        {
            AvatarBodyID id = (this as IAvatarBodyIDGenerator).Generate();

            args.BodyID = id;
            IAvatarBody body = _avatarBodyFactory.Create(args);
            body.Transform.name = $"AvatarBody - [{id.ID.ToString()}]";
            body.Transform.SetParent(null);
            
            _avatarBodyRepository.Save(args.BodyType, body);

            return body;
        }

        public IAvatarBody Find(AvatarBodyID id)
        {
            return _avatarBodyRepository.Find(id);
        }

        AvatarBodyID IAvatarBodyIDGenerator.Generate()
        {
            return new AvatarBodyID(++_avatarBodyIDIndex);
        }
    }
}