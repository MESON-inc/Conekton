using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARMultiplayer.Avatar.Domain;

namespace Conekton.ARMultiplayer.Avatar.Infrastructure
{
    public class WearablePack : IAvatarWearablePack
    {
        private IAvatarWearable[] _wearables = null;

        public WearablePack(IAvatarWearable[] wearables)
        {
            _wearables = wearables;
        }

        IAvatarWearable[] IAvatarWearablePack.GetWearables() => _wearables;
    }
}

