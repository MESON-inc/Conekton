using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID && PLATFORM_NREAL
using NRKernal;
#endif

namespace Conekton.ARUtility.Input.Application
{
    [CreateAssetMenu(fileName = "NRInputSettings", menuName = "ARUtility/NRInputSettings")]
    public class NRInputSettings : ScriptableObject
    {
#if UNITY_ANDROID && PLATFORM_NREAL
        [SerializeField] public ControllerButton One = ControllerButton.APP;
        [SerializeField] public ControllerButton Two = ControllerButton.GRIP;
        [SerializeField] public ControllerButton Three = ControllerButton.HOME;
        [SerializeField] public ControllerButton Four = ControllerButton.TOUCHPAD_BUTTON;
#endif
    }
}

