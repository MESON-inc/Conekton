using System;
using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.GrabSystemUseCase.Domain;
using UnityEngine;
using Zenject;
using Conekton.ARUtility.HandSystemUseCase.Domain;
using Conekton.ARUtility.Input.Domain;

namespace Conekton.ARUtility.GrabSystemUseCase.Infrastructure
{
    public class OculusControllerGrabController : MonoBehaviour, IGrabController
    {
        private IGrabSystem _grabSystem = null;
        private IGrabber _grabber = null;
        private IInputController _inputController = null;

        private bool _injected = false;
        private bool _isTrigger = false;

        [Inject]
        private void Injection(IGrabSystem grabSystem, IInputController inputController)
        {
            _grabSystem = grabSystem;
            _inputController = inputController;

            _grabber = GetComponent<IGrabber>();
            _grabber.OnTouched += HandleGrabberOnTouched;
            _grabber.OnUntouched += HandleGrabberOnUntouched;

            _injected = true;
        }

        private void HandleGrabberOnTouched(IGrabber grabber, IGrabbable grabbable)
        {
            _grabSystem.Touched(grabber, grabbable);
        }

        private void HandleGrabberOnUntouched(IGrabber grabber, IGrabbable grabbable)
        {
            _grabSystem.Untouched(grabber, grabbable);
        }

        #region ### MonoBehaviour ###

        private void Update()
        {
            if (!_injected)
            {
                return;
            }

            CheckTrigger();
            CheckGrabState();
        }

        #endregion ### MonoBehaviour ###

        private void CheckTrigger()
        {
            if (_inputController.IsTriggerDown)
            {
                _isTrigger = true;
            }

            if (_inputController.IsTriggerUp)
            {
                _isTrigger = false;
            }
        }

        private void CheckGrabState()
        {
            if (_grabber.IsGrabbed)
            {
                if (!IsTriggerd())
                {
                    Ungrab();
                }
                else
                {
                    Move();
                }
            }
            else
            {
                if (IsTriggerd())
                {
                    Grab();
                }
            }
        }

        private bool IsTriggerd()
        {
            return _isTrigger;
        }

        private void Grab()
        {
            foreach (var grabbable in _grabber.GetTargetGrabbables())
            {
                _grabSystem.BeginGrab(_grabber, grabbable);
            }
        }

        private void Move()
        {
            foreach (var grabbable in _grabber.GetTargetGrabbables())
            {
                _grabSystem.MoveGrab(_grabber, grabbable);
            }
        }

        private void Ungrab()
        {
            foreach (var grabbable in _grabber.GetTargetGrabbables())
            {
                _grabSystem.EndGrab(_grabber, grabbable);
            }
        }
    }
}