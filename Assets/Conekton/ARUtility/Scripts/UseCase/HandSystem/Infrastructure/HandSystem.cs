using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARUtility.HandSystemUseCase.Domain;

namespace Conekton.ARUtility.HandSystemUseCase.Infrastructure
{
    public class HandSystem : IHandSystem
    {
        [Inject] private IHandProvider _provider = null;

        public event ChangeHandStateEvent ChangeHandState;

        float IHandSystem.GetFingerStrength(HandType handType, FingerType fingerType)
        {
            if (TryGetHand(handType, out IHand hand))
            {
                return hand.GetFingerStrength(fingerType);
            }
            return 0;
        }

        HandGestureType IHandSystem.GetGestureType(HandType handType)
        {
            throw new System.NotImplementedException();
        }

        (Vector3, Vector3) IHandSystem.GetPalmPositionNormal(HandType handType)
        {
            if (TryGetHand(handType, out IHand hand))
            {
                if (hand.TryGetPalmPositionAndNormal(out Vector3 position, out Vector3 normal))
                {
                    return (position, normal);
                }
            }
            return (Vector3.zero, Vector3.up);
        }

        Pose IHandSystem.GetPose(HandType handType)
        {
            if (TryGetHand(handType, out IHand hand))
            {
                return hand.GetPose();
            }
            return default;
        }

        bool IHandSystem.IsTracked(HandType handType)
        {
            if (TryGetHand(handType, out IHand hand))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Try to get a specific hand.
        /// </summary>
        /// <param name="handType">Tareget hand type that you want.</param>
        /// <param name="hand">The hand gesture object.</param>
        /// <returns>This method will return false if the hand isn't tracking.</returns>
        private bool TryGetHand(HandType handType, out IHand hand)
        {
            hand = _provider.GetHand(handType);
            return hand != null;
        }
    }
}

