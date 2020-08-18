using System;
using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.HandGrabSystemUseCase.Domain;
using UnityEngine;

using Conekton.ARUtility.HandGrabSystemUseCase.Application;

namespace Conekton.ARUtility.HandGrabSystemUseCase.Infrastructure
{
    [RequireComponent(typeof(GrabbableMover))]
    public class Grabbable : MonoBehaviour, IGrabbable
    {
        public event OnTouchedEvent OnTouched;
        public event OnUntouchedEvent OnUntouched;
        public event OnBeganGrabEvent OnBeganGrab;
        public event OnMovedGrabEvent OnMovedGrab;
        public event OnEndedGrabEvent OnEndedGrab;
        public event OnPausedGrabEvent OnPausedGrab;
        public event OnResumedGrabEvent OnResumedGrab;
        public event OnForceEndedGrabEvent OnForceEndedGrab;

        public bool IsGrabbed { get; private set; } = false;

        private GrabbableMover _grabbableMover = null;
        private bool _isPaused = false;

        private void Awake()
        {
            _grabbableMover = GetComponent<GrabbableMover>();
        }

        public void Touched(IGrabber grabber)
        {
            OnTouched?.Invoke(grabber, this);
        }

        public void UnTouched(IGrabber grabber)
        {
            OnUntouched?.Invoke(grabber, this);
        }

        public void Begin(IGrabber grabber)
        {
            _grabbableMover.Begin(grabber);
            OnBeganGrab?.Invoke(grabber, this);
        }

        public void Move(IGrabber grabber)
        {
            if (_isPaused)
            {
                return;
            }
            
            _grabbableMover.Move();
            OnMovedGrab?.Invoke(grabber, this);
        }

        public void End(IGrabber grabber)
        {
            _grabbableMover.End();
            OnEndedGrab?.Invoke(grabber, this);
        }

        public void ForceEnd()
        {
            _grabbableMover.End();
            OnForceEndedGrab?.Invoke(this);
        }

        public void Pause()
        {
            _isPaused = true;
            OnPausedGrab?.Invoke();
        }

        public void Resume()
        {
            _isPaused = false;
            OnResumedGrab?.Invoke();
        }
    }
}
