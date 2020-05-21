using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

#if UNITY_ANDROID && PLATFORM_NREAL
using NRKernal;
#elif PLATFORM_LUMIN
using UnityEngine.XR.MagicLeap;
#endif

namespace Conekton.ARUtility.EventSystem.Application
{
    /// <summary>
    /// This component provide to be able to use any canvas with NR or ML raycasters.
    /// This component will add a raycater onto a canvas by platform.
    /// 
    /// NOTE: This component has a static list member. The menber will be used to get all canvases from InputControllerView component.
    /// </summary>
    public class ARRaycaster : MonoBehaviour
    {
        private static List<Canvas> _canvases = null;

        public static IReadOnlyList<Canvas> AllCanvases
        {
            get
            {
                if (_canvases == null)
                {
                    Debug.LogWarning("You are accessing All canvases property but you haven't used any ARRaycaster component. If you want to use this property correctly, please set up the ARRaycaster component correctly.");
                    _canvases = new List<Canvas>();
                }

                return _canvases as IReadOnlyList<Canvas>;
            }
        }

        private void Awake()
        {
            Canvas canvas = GetComponent<Canvas>();

            if (canvas == null)
            {
                Debug.LogWarning("Canvas component has not found.");
                return;
            }

#if UNITY_ANDROID && PLATFORM_NREAL
            CanvasRaycastTarget raycastTarget;
            if (!gameObject.TryGetComponent(out raycastTarget))
            {
                gameObject.AddComponent<CanvasRaycastTarget>();
            }
#elif PLATFORM_LUMIN
            MLInputRaycaster raycaster;
            if (!gameObject.TryGetComponent(out raycaster))
            {
                gameObject.AddComponent<MLInputRaycaster>();
            }
#endif
        }

        private void OnEnable()
        {
            Canvas canvas;
            if (TryGetComponent(out canvas))
            {
                AddCanvas(canvas);
            }
        }

        private void OnDisable()
        {
            Canvas canvas;
            if (TryGetComponent(out canvas))
            {
                RemoveCanvas(canvas);
            }
        }

        private void AddCanvas(Canvas canvas)
        {
            if (_canvases == null)
            {
                _canvases = new List<Canvas>();
            }

            if (!_canvases.Contains(canvas))
            {
                _canvases.Add(canvas);
            }
        }

        private void RemoveCanvas(Canvas canvas)
        {
            if (_canvases == null)
            {
                _canvases = new List<Canvas>();
            }

            if (_canvases.Contains(canvas))
            {
                _canvases.Remove(canvas);
            }
        }
    }
}

