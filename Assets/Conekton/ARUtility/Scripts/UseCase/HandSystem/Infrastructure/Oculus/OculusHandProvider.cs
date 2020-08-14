using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARUtility.HandSystemUseCase.Domain;

namespace Conekton.ARUtility.HandSystemUseCase.Infrastructure
{
    public class OculusHandProvider : MonoBehaviour
#if UNITY_ANDROID && PLATFORM_OCULUS
        , IHandProvider
#endif
    {
        private IHand[] _hands = null;

        private void Awake()
        {
            OVRHand[] ovrHands = FindObjectsOfType<OVRHand>();

            _hands = new IHand[ovrHands.Length];
            for (int i = 0; i < _hands.Length; i++)
            {
                _hands[i] = ovrHands[i].GetComponent<IHand>();
            }
        }

#if UNITY_ANDROID && PLATFORM_OCULUS
        IHand[] IHandProvider.GetAllHands()
        {
            return _hands;
        }

        IHand IHandProvider.GetHand(HandType handType)
        {
            foreach (var h in _hands)
            {
                if (h.GetHandType() == handType)
                {
                    return h;
                }
            }

            return null;
        }
#endif
    }
}
