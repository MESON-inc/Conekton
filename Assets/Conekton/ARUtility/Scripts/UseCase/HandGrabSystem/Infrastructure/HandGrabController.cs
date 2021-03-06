﻿using System;
using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.GrabSystemUseCase.Domain;
using UnityEngine;
using Zenject;
using Conekton.ARUtility.HandSystemUseCase.Domain;

namespace Conekton.ARUtility.GrabSystemUseCase.Infrastructure
{
    public class HandGrabController : MonoBehaviour, IGrabController
    {
        private IGrabSystem _grabSystem = null;

        private IGrabber _grabber = null;
        private IHand _hand = null;

        private bool _injected = false;

        [Inject]
        private void Injection(IGrabSystem grabSystem)
        {
            _grabSystem = grabSystem;

            _grabber = GetComponent<IGrabber>();
            _hand = GetComponent<IHand>();

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

            CheckGrabState();
        }

        #endregion ### MonoBehaviour ###

        private void CheckGrabState()
        {
            if (_grabber.IsGrabbed)
            {
                if (!IsPinching())
                {
                    Ungrab();
                }
                else
                {
                    if (_hand.IsTracked)
                    {
                        Move();
                    }
                }
            }
            else
            {
                if (IsPinching())
                {
                    Grab();
                }
            }
        }

        private bool IsPinching()
        {
            return _hand.GetFingerStrength(FingerType.Index) > 0.5f;
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