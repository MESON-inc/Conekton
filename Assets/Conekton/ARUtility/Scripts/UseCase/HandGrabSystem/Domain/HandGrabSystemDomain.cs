using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conekton.ARUtility.HandGrabSystemUseCase.Domain
{
    public delegate void OnBeganGrabEvent(IGrabber grabbere, IGrabbable grabbable);
    public delegate void OnMovedGrabEvent(IGrabber grabbere, IGrabbable grabbable);
    public delegate void OnEndedGrabEvent(IGrabber grabbere, IGrabbable grabbable);
    public delegate void OnForceEndedGrabEvent(IGrabbable grabbable);
    public delegate void OnPausedGrabEvent();
    public delegate void OnResumedGrabEvent();
    
    public interface IHandGrabSystem
    {
        void Touched(IGrabber grabber, IGrabbable grabbable);
        void Untouched(IGrabber grabber, IGrabbable grabbable);
        void BeginGrab(IGrabber grabber, IGrabbable grabbable);
        void MoveGrab(IGrabber grabber, IGrabbable grabbable);
        void EndGrab(IGrabber grabber, IGrabbable grabbable);
    }

    public interface IGrabber
    {
        void Grab(IGrabbable grabbable);
        void Ungrab(IGrabbable grabbable);
    }

    public interface IGrabbable
    {
        bool IsGrabbed { get; }
        void Begin(IGrabber grabber);
        void Move(IGrabber grabber);
        void End(IGrabber grabber);
        void ForceEnd();
        void Pause();
        void Resume();
    }

    public interface IHandGrabReopsitory
    {
        void Add(IGrabbable grabbable);
        void Remove(IGrabbable grabbable);
        IGrabbable[] GetAllGrabbables();
    }
    
    public interface IHandGrabController {}
}