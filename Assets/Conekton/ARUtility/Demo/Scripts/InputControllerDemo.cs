using UnityEngine;
using UnityEngine.EventSystems;

using Zenject;

using Conekton.ARUtility.Input.Domain;

namespace Conekton.ARUtility.Demo
{
    public class InputControllerDemo : MonoBehaviour
    {
        [Inject] private IInputController _inputController = null;

        private void Update()
        {
            if (_inputController.IsTriggerDown)
            {
                Debug.Log("Input controller trigger is down.");
            }

            if (_inputController.IsTriggerUp)
            {
                Debug.Log("Input controller trigger is up.");
            }

            if (_inputController.IsTouchDown)
            {
                Debug.Log("Input controller touch is down.");
            }

            if (_inputController.IsTouchUp)
            {
                Debug.Log("Input controller touch is up.");
            }

            if (_inputController.IsTouch)
            {
                Debug.Log("Input controller is touching.");
            }

            if (_inputController.IsDown(ButtonType.One))
            {
                Debug.Log("Input controller One button is down.");
            }

            if (_inputController.IsDown(ButtonType.Two))
            {
                Debug.Log("Input controller Two button is down.");
            }

            if (_inputController.IsDown(ButtonType.Three))
            {
                Debug.Log("Input controller Three button is down.");
            }

            if (_inputController.IsDown(ButtonType.Four))
            {
                Debug.Log("Input controller Four button is down.");
            }

            if (_inputController.IsUp(ButtonType.One))
            {
                Debug.Log("Input controller One button is up.");
            }

            if (_inputController.IsUp(ButtonType.Two))
            {
                Debug.Log("Input controller Two button is up.");
            }

            if (_inputController.IsUp(ButtonType.Three))
            {
                Debug.Log("Input controller Three button is up.");
            }

            if (_inputController.IsUp(ButtonType.Four))
            {
                Debug.Log("Input controller Four button is up.");
            }
        }
    }
}

