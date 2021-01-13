using System;
using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.GrabSystemUseCase.Domain;
using Conekton.ARUtility.Input.Domain;
using UnityEngine;
using ModestTree;
using Zenject;

namespace Conekton.ARUtility.GrabSystemUseCase.Infrastructure
{
    public class RayGrabber : Grabber
    {
        [Inject] private IInputController _inputController = null;

        [SerializeField] private ControllerType _controllerType = ControllerType.Right;

        private IGrabbable _hoverGrabbable = null;

        private void Update()
        {
            CheckHover();
        }

        private void CheckHover()
        {
            Ray ray = GetRay();

            if (!Physics.Raycast(ray, out RaycastHit hit))
            {
                Unhover();
                return;
            }

            if (hit.collider.TryGetComponent(out IGrabbable grabbable))
            {
                Hover(grabbable);
            }
        }

        private void Hover(IGrabbable grabbable)
        {
            if (_hoverGrabbable == grabbable)
            {
                return;
            }
            
            Unhover();

            _hoverGrabbable = grabbable;

            TryTouch(grabbable);
        }

        private void Unhover()
        {
            if (_hoverGrabbable != null)
            {
                TryUntouch(_hoverGrabbable);
                _hoverGrabbable = null;
            }
        }

        protected override void OnUngrab(IGrabbable grabbable)
        {
            _hoverGrabbable = null;
        }

        private Ray GetRay()
        {
            return new Ray(_inputController.GetPosition(_controllerType), _inputController.GetForward(_controllerType));
        }
    }
}