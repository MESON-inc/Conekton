using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Conekton.Utility
{
    public static class GameObjectUtility
    {
        public static T FindObjectOfTypeInScene<T>()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                foreach (var g in scene.GetRootGameObjects())
                {
                    T comp = GetComponentRecursive<T>(g.transform);
                    
                    if (comp != null)
                    {
                        return comp;
                    }
                }
            }

            return default;
        }

        private static T GetComponentRecursive<T>(Transform target)
        {
            T comp = target.GetComponent<T>();
            if (comp != null)
            {
                return comp;
            }

            foreach (Transform child in target)
            {
                comp = GetComponentRecursive<T>(child);
                if (comp != null)
                {
                    return comp;
                }
            }

            return default;
        }
    }
}