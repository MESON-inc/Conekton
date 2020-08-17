using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Conekton.ARUtility.HandSystemUseCase.Domain;
using Conekton.ARUtility.HandGrabSystemUseCase.Domain;

namespace Conekton.ARUtility.HandGrabSystemUseCase.Infrastructure
{
    public class OculusHandGrabController : MonoBehaviour, IHandGrabController
    {
        private IHandGrabSystem _grabSystem = null;
        private IHandSystem _handSystem = null;

        private IGrabber _grabber = null;
        private IHand _hand = null;

        private bool _injected = false;

        [Inject]
        private void Injection(IHandGrabSystem grabSystem, IHandSystem handSystem)
        {
            _grabSystem = grabSystem;
            _handSystem = handSystem;

            _grabber = GetComponent<IGrabber>();
            _hand = GetComponent<IHand>();
            
            _grabber.OnTouched += HandleGrabberOnOnTouched;
            _grabber.OnUntouched += HandleGrabberOnOnUntouched;

            _injected = true;
        }

        private void HandleGrabberOnOnTouched(IGrabber grabber, IGrabbable grabbable)
        {
            _grabSystem.Touched(grabber, grabbable);
        }

        private void HandleGrabberOnOnUntouched(IGrabber grabber, IGrabbable grabbable)
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

            if (_hand.GetFingerIsPinching(FingerType.Index))
            {
                Grab();
            }
        }
        #endregion ### MonoBehaviour ###

        private void Grab()
        {
            foreach (var grabbable in _grabber.GetTargetGrabbables())
            {
                _grabSystem.BeginGrab(_grabber, grabbable);
            }
        }
    }
}
