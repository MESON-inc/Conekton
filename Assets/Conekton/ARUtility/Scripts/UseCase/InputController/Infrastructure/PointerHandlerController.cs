using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Zenject;

using Conekton.ARUtility.Input.Domain;

namespace Conekton.ARUtility.Input.Infrastructure
{
    public class PointerHandlerController : ITickable
    {
        [Inject] private IInputController _inputController = null;

        private RaycastHit[] _raycastResults = new RaycastHit[50];
        private GameObject _lastPointedObject = null;
        private GameObject _triggerDownTarget = null;
        private GameObject _clickTarget = null;
        private bool _needsSendClickEvent = false;

        private ControllerType _controllerType = ControllerType.Right;

        void ITickable.Tick()
        {
            CheckSendClickEvent();
            CheckPointerHandlers();
        }

        private void CheckPointerHandlers()
        {
            Ray ray = GetRay();

            int count = Physics.RaycastNonAlloc(ray, _raycastResults);

            if (count == 0)
            {
                HandleNoTarget();
            }
            else
            {
                RaycastHit result = GetNearestResult(count);
                HandleTarget(result.collider.gameObject);
            }
        }

        private void HandleTarget(GameObject target)
        {
            if (_lastPointedObject != target)
            {
                SendExitEvent(_lastPointedObject);
                SendEnterEvent(target);
            }

            _lastPointedObject = target;

            CheckTriggerDown(target);
            CheckTriggerUp(target);
        }

        private void HandleNoTarget()
        {
            if (_lastPointedObject != null)
            {
                SendExitEvent(_lastPointedObject);
            }

            _lastPointedObject = null;

            CheckTriggerUp(null);
        }

        private void CheckSendClickEvent()
        {
            if (_needsSendClickEvent)
            {
                if (_clickTarget != null)
                {
                    SendClickEvent(_clickTarget);
                    _clickTarget = null;
                }

                _needsSendClickEvent = false;
            }
        }

        private void CheckTriggerDown(GameObject target)
        {
            if (!_inputController.IsTriggerDown(_controllerType))
            {
                return;
            }

            if (target != null)
            {
                SendTriggerDownEvent(target);
            }

            _triggerDownTarget = target;
        }

        private void CheckTriggerUp(GameObject target)
        {
            if (!_inputController.IsTriggerUp(_controllerType))
            {
                return;
            }

            if (target != null)
            {
                SendTriggerUpEvent(target);

                if (_triggerDownTarget == target)
                {
                    _needsSendClickEvent = true;
                    _clickTarget = target;
                }
            }

            _triggerDownTarget = null;
        }

        private void SendClickEvent(GameObject target)
        {
            ExecuteEvents.Execute<IPointerClickHandler>(
                target: target,
                eventData: null,
                functor: (receiveTarget, y) => receiveTarget.OnPointerClick(null)
            );
        }

        private void SendTriggerDownEvent(GameObject target)
        {
            ExecuteEvents.Execute<IPointerDownHandler>(
                target: target,
                eventData: null,
                functor: (receiveTarget, y) => receiveTarget.OnPointerDown(null)
            );
        }

        private void SendTriggerUpEvent(GameObject target)
        {
            ExecuteEvents.Execute<IPointerUpHandler>(
                target: target,
                eventData: null,
                functor: (receiveTarget, y) => receiveTarget.OnPointerUp(null)
            );
        }

        private void SendExitEvent(GameObject target)
        {
            ExecuteEvents.Execute<IPointerExitHandler>(
                target: target,
                eventData: null,
                functor: (receiveTarget, y) => receiveTarget.OnPointerExit(null)
            );
        }

        private void SendEnterEvent(GameObject target)
        {
            ExecuteEvents.Execute<IPointerEnterHandler>(
                target: target,
                eventData: null,
                functor: (receiveTarget, y) => receiveTarget.OnPointerEnter(null)
            );
        }

        private RaycastHit GetNearestResult(int count)
        {
            float minDist = float.MaxValue;
            int resultIdx = 0;

            for (int i = 0; i < count; i++)
            {
                if (minDist > _raycastResults[i].distance)
                {
                    resultIdx = i;
                    minDist = _raycastResults[i].distance;
                }
            }

            return _raycastResults[resultIdx];
        }

        private Ray GetRay()
        {
            return new Ray(_inputController.GetPosition(_controllerType), _inputController.GetForward(_controllerType));
        }
    }
}

