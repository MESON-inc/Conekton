using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.HandGrabSystemUseCase.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARUtility.HandGrabSystemUseCase.Infrastructure
{
    public class HandGrabSystem : IHandGrabSystem
    {
        [Inject] private IHandGrabReopsitory _repository = null;

        void IHandGrabSystem.Touched(IGrabber grabber, IGrabbable grabbable)
        {
            _repository.Add(grabbable);

            grabbable.Touched(grabber);
        }

        void IHandGrabSystem.Untouched(IGrabber grabber, IGrabbable grabbable)
        {
            _repository.Remove(grabbable);
            
            grabbable.UnTouched(grabber);
        }

        void IHandGrabSystem.BeginGrab(IGrabber grabber, IGrabbable grabbable)
        {
            throw new System.NotImplementedException();
        }

        void IHandGrabSystem.MoveGrab(IGrabber grabber, IGrabbable grabbable)
        {
            throw new System.NotImplementedException();
        }

        void IHandGrabSystem.EndGrab(IGrabber grabber, IGrabbable grabbable)
        {
            throw new System.NotImplementedException();
        }
    }
}