﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.AvatarBody.Domain;

namespace Conekton.ARMultiplayer.AvatarBody.Presentation
{
    public class AvatarBody : MonoBehaviour, IAvatarBody
    {
        public AvatarBodyType BodyType => _bodyType;
        public AvatarBodyID BodyID => _bodyID;
        public Transform Transform => transform;

        private AvatarBodyType _bodyType = AvatarBodyType.Default;
        private AvatarBodyID _bodyID = default;
        private IAvatar _avatar = null;

        [Inject]
        public void Construct(AvatarBodyTypeArgs args)
        {
            _bodyType = args.BodyType;
            _bodyID = args.BodyID;
        }
        
        public void SetAvatar(IAvatar avatar)
        {
            _avatar = avatar;
        }

        public void SetAsMain(bool asMain)
        {
            //
        }

        public void Release()
        {
            //
        }
    }
}
