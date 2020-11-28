using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Conekton.ARUtility.Input.Domain;
using UnityEngine.UI;

namespace Conekton.ARUtility.Demo
{
    public class InputControllerDemo : MonoBehaviour
    {
        [Inject] private IInputController _inputController = null;

        [SerializeField] private Text _textViewLeft = null;
        [SerializeField] private Text _textViewRight = null;

        private void Update()
        {
            CheckEvents(ControllerType.Left, _textViewLeft);
            CheckEvents(ControllerType.Right, _textViewRight);
        }

        private void CheckEvents(ControllerType type, Text target)
        {
            target.text = "";
            target.text += $"{type} input controller trigger is down? {_inputController.IsTriggerDown(type)}\n";
            target.text += $"{type} input controller trigger is up? {_inputController.IsTriggerUp(type)}\n";
            target.text += $"{type} input controller is touching? {_inputController.IsTouch(type)}\n";
            target.text += $"{type} input controller touch is down? {_inputController.IsTouchDown(type)}\n";
            target.text += $"{type} input controller touch is up? {_inputController.IsTouchUp(type)}\n";
            target.text += $"{type} input controller One button is down? {_inputController.IsDown(type, ButtonType.One)}\n";
            target.text += $"{type} input controller One button is up? {_inputController.IsUp(type, ButtonType.One)}\n";
            target.text += $"{type} input controller Two button is down? {_inputController.IsDown(type, ButtonType.Two)}\n";
            target.text += $"{type} input controller Two button is up? {_inputController.IsUp(type, ButtonType.Two)}\n";
            target.text += $"{type} input controller Three button is up? {_inputController.IsUp(type, ButtonType.Three)}\n";
            target.text += $"{type} input controller Three button is down? {_inputController.IsDown(type, ButtonType.Three)}\n";
            target.text += $"{type} input controller Four button is up? {_inputController.IsUp(type, ButtonType.Four)}\n";
            target.text += $"{type} input controller Four button is down? {_inputController.IsDown(type, ButtonType.Four)}\n";
        }
    }
}