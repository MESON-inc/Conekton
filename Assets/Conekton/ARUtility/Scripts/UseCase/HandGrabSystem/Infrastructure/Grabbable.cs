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
        public event OnBeganGrabEvent OnBeganGrab;
        public event OnMovedGrabEvent OnMovedGrab;
        public event OnEndedGrabEvent OnEndedGrab;
        public event OnPausedGrabEvent OnPausedGrab;
        public event OnResumedGrabEvent OnResumedGrab;
        public event OnForceEndedGrabEvent OnForceEndedGrab;
        
        public bool IsGrabbed { get; private set; } = false;
        
        private Vector3 _offsetPosition = Vector3.zero;
        private Quaternion _offsetRotation = Quaternion.identity;
        private Quaternion _grabberRotInv = Quaternion.identity;

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
            OnBeganGrab?.Invoke(grabber, this);

            Transform trans = transform;
            Pose pose = grabber.GetPose();
            _grabberRotInv = Quaternion.Inverse(pose.rotation);
            _offsetPosition = trans.position - pose.position;
            _offsetRotation = _grabberRotInv * trans.rotation;
        }

        public void Move(IGrabber grabber)
        {
            OnMovedGrab?.Invoke(grabber, this);

            Pose pose = grabber.GetPose();
            Quaternion rot = pose.rotation * _offsetRotation;
            Vector3 pos = pose.position + (pose.rotation * _grabberRotInv * _offsetPosition);
            
            transform.SetPositionAndRotation(pos, rot);
        }

        public void End(IGrabber grabber)
        {
            OnEndedGrab?.Invoke(grabber, this);
        }

        public void ForceEnd()
        {
            OnForceEndedGrab?.Invoke(this);
        }

        public void Pause()
        {
            OnPausedGrab?.Invoke();
        }

        public void Resume()
        {
            OnResumedGrab?.Invoke();
        }
    }
}
