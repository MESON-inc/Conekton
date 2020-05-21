using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if (UNITY_IOS || UNITY_ANDROID) && !(PLATFORM_NREAL || PLATFORM_OCULUS)
using UnityEngine.XR.ARFoundation;

namespace Conekton.ARUtility.UseCase.ARMarkerDetector.Application
{
    /// <summary>
    /// This class provide to get <see cref="ARTrackedImageManager"/>.
    /// But this is wrong dependency because <see cref="GetManager"/> method use <see cref="GameObject.FindObjectOfType"/>.
    /// Why this class use this method?
    /// <see cref="ARTrackedImageManager"/> have to be with <see cref="ARSessionOrigin"/>. The component attached on a Player prefab.
    /// We have to find an instance by FindObjectOfType or something like these methods.
    /// </summary>
    public class ARTrackedImageManagerProvider
    {
        public ARTrackedImageManager GetManager()
        {
            ARTrackedImageManager manager = GameObject.FindObjectOfType<ARTrackedImageManager>();

            if (manager == null)
            {
                Debug.LogError($"{nameof(ARTrackedImageManager)} is not found. Please check it out the manager attached to a object that has Session-Origin component in a player.");
            }

            return manager;
        }
    }
}
#endif
