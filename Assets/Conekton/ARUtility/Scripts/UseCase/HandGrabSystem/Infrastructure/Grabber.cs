using System;
using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.GrabSystemUseCase.Domain;
using UnityEngine;
using ModestTree;

namespace Conekton.ARUtility.GrabSystemUseCase.Infrastructure
{
    public class Grabber : MonoBehaviour, IGrabber
    {
        public event OnTouchedEvent OnTouched;
        public event OnUntouchedEvent OnUntouched;

        private readonly List<IGrabbable> _targetGrabbables = new List<IGrabbable>();
        private readonly List<IGrabbable> _grabbedGrabbables = new List<IGrabbable>();
        private readonly List<IGrabbable> _removeTarget = new List<IGrabbable>();

        #region ### MonoBehaviour ###
        private void LateUpdate()
        {
            _targetGrabbables.RemoveAll(g => _removeTarget.Contains(g));
            _removeTarget.Clear();
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

            if (!_grabbedGrabbables.Contains(grabbable))
            {
                _grabbedGrabbables.Add(grabbable);
            }
            
            OnGrab(grabbable);
        }

        public void Ungrab(IGrabbable grabbable)
        {
            IsGrabbed = false;

            if (_grabbedGrabbables.Contains(grabbable))
            {
                _grabbedGrabbables.Remove(grabbable);
            }

            OnUngrab(grabbable);
        }
        
        protected virtual void OnGrab(IGrabbable grabbable) { }
        protected virtual void OnUngrab(IGrabbable grabbable) { }

        public bool TryTouch(IGrabbable grabbable)
        {
            if (!CanTouch(grabbable))
            {
                return false;
            }

            AddGrabbable(grabbable);
            OnTouched?.Invoke(this, grabbable);

            return true;
        }

        public bool TryUntouch(IGrabbable grabbable)
        {
            if (!CanUntouch(grabbable))
            {
                return false;
            }

            RemoveGrabbable(grabbable);
            OnUntouched?.Invoke(this, grabbable);

            return true;
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
                if (!_removeTarget.Contains(grabbable))
                {
                    _removeTarget.Add(grabbable);
                }
            }
        }

        private bool CanTouch(IGrabbable grabbable)
        {
            return !_targetGrabbables.Contains(grabbable);
        }

        private bool CanUntouch(IGrabbable grabbable)
        {
            return !_grabbedGrabbables.Contains(grabbable);
        }
    }
}