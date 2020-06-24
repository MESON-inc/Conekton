using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID && PLATFORM_NREAL
using NRKernal;
#endif

namespace Conekton.ARUtility.Input.Application
{
    [CreateAssetMenu(fileName = "EditorInputSettings", menuName = "ARUtility/EditorInputSettings")]
    public class EditorInputSettings : ScriptableObject
    {
        [SerializeField] public KeyCode One = KeyCode.Alpha1;
        [SerializeField] public KeyCode Two = KeyCode.Alpha2;
        [SerializeField] public KeyCode Three = KeyCode.Alpha3;
        [SerializeField] public KeyCode Four = KeyCode.Alpha4;
    }
}

