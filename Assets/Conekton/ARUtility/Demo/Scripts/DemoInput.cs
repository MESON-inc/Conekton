using UnityEngine;
using UnityEngine.EventSystems;

namespace Conekton.ARUtility.Demo
{
    public class DemoInput : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private Material _material = null;

        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("On Click.");

            _material.color = Color.blue;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("On Down.");

            _material.color = Color.yellow;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("On Enter.");

            _material.color = Color.green;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("On Exit.");

            _material.color = Color.white;
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("On Up.");
        }
    }
}

