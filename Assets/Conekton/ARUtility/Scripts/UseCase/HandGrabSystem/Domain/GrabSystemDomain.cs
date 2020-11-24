using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conekton.ARUtility.GrabSystemUseCase.Domain
{
    public delegate void OnTouchedEvent(IGrabber grabber, IGrabbable grabbable);
    public delegate void OnUntouchedEvent(IGrabber grabber, IGrabbable grabbable);
    public delegate void OnBeganGrabEvent(IGrabber grabbere, IGrabbable grabbable);
    public delegate void OnMovedGrabEvent(IGrabber grabbere, IGrabbable grabbable);
    public delegate void OnEndedGrabEvent(IGrabber grabbere, IGrabbable grabbable);
    public delegate void OnForceEndedGrabEvent(IGrabbable grabbable);
    public delegate void OnPausedGrabEvent();
    public delegate void OnResumedGrabEvent();
    public delegate void OnThrownEvent(IGrabbable grabbable);
    
    public interface IGrabSystem
    {
        void Touched(IGrabber grabber, IGrabbable grabbable);
        void Untouched(IGrabber grabber, IGrabbable grabbable);
        void BeginGrab(IGrabber grabber, IGrabbable grabbable);
        void MoveGrab(IGrabber grabber, IGrabbable grabbable);
        void EndGrab(IGrabber grabber, IGrabbable grabbable);
    }

    public interface IGrabber
    {
        event OnTouchedEvent OnTouched;
        event OnUntouchedEvent OnUntouched;
        bool IsGrabbed { get; }
        void Grab(IGrabbable grabbable);
        void Ungrab(IGrabbable grabbable);
        IReadOnlyList<IGrabbable> GetTargetGrabbables();
        Pose GetPose();
    }

    public interface IGrabbable
    {
        event OnTouchedEvent OnTouched;
        event OnUntouchedEvent OnUntouched;
        event OnBeganGrabEvent OnBeganGrab;
        event OnMovedGrabEvent OnMovedGrab;
        event OnEndedGrabEvent OnEndedGrab;
        event OnPausedGrabEvent OnPausedGrab;
        event OnResumedGrabEvent OnResumedGrab;
        event OnForceEndedGrabEvent OnForceEndedGrab;
        bool IsGrabbed { get; }
        void Touched(IGrabber grabber);
        void UnTouched(IGrabber grabber);
        void Begin(IGrabber grabber);
        void Move(IGrabber grabber);
        void End(IGrabber grabber);
        void ForceEnd();
        void Pause();
        void Resume();
    }

    public interface IGrabReopsitory
    {
        void Add(IGrabbable grabbable);
        void Remove(IGrabbable grabbable);
        IReadOnlyList<IGrabbable> GetAllGrabbables();
    }
    
    public interface IGrabController {}
}