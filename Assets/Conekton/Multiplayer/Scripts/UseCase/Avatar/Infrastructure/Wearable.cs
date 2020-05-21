using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARMultiplayer.Avatar.Domain;

namespace Conekton.ARMultiplayer.Avatar.Infrastructure
{
    public class Wearable : MonoBehaviour, IAvatarWearable
    {
        [SerializeField] private Vector3 _offsetPosition = Vector3.zero;
        [SerializeField] private Vector3 _offsetRotation = Vector3.zero;
        [SerializeField] private AvatarWearType _wearType = AvatarWearType.None;

        AvatarWearType IAvatarWearable.WearType => _wearType;

        public void SetWearType(AvatarWearType type)
        {
            _wearType = type;
        }

        void IAvatarWearable.TargetTransform(Transform trans)
        {
            transform.SetParent(trans);
            transform.localPosition = _offsetPosition;
            transform.localRotation = Quaternion.Euler(_offsetRotation);
        }
        
        void IAvatarWearable.Unwear()
        {
            Destroy(gameObject);
        }
    }
}

