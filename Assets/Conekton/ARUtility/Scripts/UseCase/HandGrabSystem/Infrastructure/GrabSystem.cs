using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.GrabSystemUseCase.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARUtility.GrabSystemUseCase.Infrastructure
{
    public class GrabSystem : IGrabSystem
    {
        [Inject] private IGrabReopsitory _repository = null;

        void IGrabSystem.Touched(IGrabber grabber, IGrabbable grabbable)
        {
            _repository.Add(grabbable);

            grabbable.Touched(grabber);
        }

        void IGrabSystem.Untouched(IGrabber grabber, IGrabbable grabbable)
        {
            _repository.Remove(grabbable);
            
            grabbable.UnTouched(grabber);
        }

        void IGrabSystem.BeginGrab(IGrabber grabber, IGrabbable grabbable)
        {
            grabber.Grab(grabbable);
            grabbable.Begin(grabber);
        }

        void IGrabSystem.MoveGrab(IGrabber grabber, IGrabbable grabbable)
        {
            grabbable.Move(grabber);
        }

        void IGrabSystem.EndGrab(IGrabber grabber, IGrabbable grabbable)
        {
            grabber.Ungrab(grabbable);
            grabbable.End(grabber);
        }
    }
}