using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.HandGrabSystemUseCase.Domain;
using UnityEngine;

namespace Conekton.ARUtility.HandGrabSystemUseCase.Infrastructure
{
    public class Grabbable : MonoBehaviour, IGrabbable
    {
        public event OnTouchedEvent OnTouched;
        public event OnUntouchedEvent OnUntouched;
        
        public bool IsGrabbed { get; private set; } = false;

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
            throw new System.NotImplementedException();
        }

        public void Move(IGrabber grabber)
        {
            throw new System.NotImplementedException();
        }

        public void End(IGrabber grabber)
        {
            throw new System.NotImplementedException();
        }

        public void ForceEnd()
        {
            throw new System.NotImplementedException();
        }

        public void Pause()
        {
            throw new System.NotImplementedException();
        }

        public void Resume()
        {
            throw new System.NotImplementedException();
        }
    }
}
