using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARUtility.HandGrabSystemUseCase.Domain;

namespace Conekton.ARUtility.HandGrabSystemUseCase.Infrastructure
{
    public class OculusHandGrabber : MonoBehaviour, IGrabber
    {
        public event OnTouchedEvent OnTouched;
        public event OnUntouchedEvent OnUntouched;
        
        private readonly List<IGrabbable> _targetGrabbables = new List<IGrabbable>();

        #region ### MonoBehaviour ###
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IGrabbable grabbable))
            {
                AddGrabbable(grabbable);
                OnTouched?.Invoke(this, grabbable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IGrabbable grabbable))
            {
                RemoveGrabbable(grabbable);
                OnUntouched?.Invoke(this, grabbable);
            }
        }
        #endregion ### MonoBehaviour ###

        #region ### IGrabber interface ###
        public bool IsGrabbed { get; private set; } = false;

        public IReadOnlyList<IGrabbable> GetTargetGrabbables()
        {
            return _targetGrabbables;
        }

        public void Grab(IGrabbable grabbable)
        {
            IsGrabbed = true;
        }

        public void Ungrab(IGrabbable grabbable)
        {
            IsGrabbed = false;
        }

        public Pose GetPose()
        {
            Transform trans = transform;
            return new Pose(trans.position, trans.rotation);
        }
        #endregion ### IGrabber interface ###
        
        private void AddGrabbable(IGrabbable grabbable)
        {
            if (!_targetGrabbables.Contains(grabbable))
            {
                _targetGrabbables.Add(grabbable);
            }
        }

        private void RemoveGrabbable(IGrabbable grabbable)
        {
            if (_targetGrabbables.Contains(grabbable))
            {
                _targetGrabbables.Remove(grabbable);
            }
        }
    }
}
