#if PLATFORM_LUMIN
using System;
using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Application;
using MagicLeap.Core;
using MagicLeap.Core.StarterKit;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace Conekton.ARUtility.UseCase.ARMarkerDetector.Infrastructure
{
    public struct MLMarkerTrackerArgs
    {
        public MLReferenceImage Reference;
        public MLImageTracker.Target.Result Result;
    }
    
    public class MLMarkerTracker : MonoBehaviour
    {
        public event Action<MLMarkerTrackerArgs> OnTargetFound;
        public event Action<MLMarkerTrackerArgs> OnTargetUpdated;
        public event Action<MLMarkerTrackerArgs> OnTargetLost;
        
        private MLImageTrackerBehavior _tracker = null;
        private MLReferenceImage _reference = default;
        
        public void Setup(MLReferenceImage reference)
        {
            _reference = reference;
            
            MLImageTrackerBehavior tracker = gameObject.AddComponent<MLImageTrackerBehavior>();
            tracker.image = reference.TargetTexture;
            tracker.autoUpdate = true;
            tracker.longerDimensionInSceneUnits = reference.LongerDimensionInSceneUnits;
            tracker.OnTargetFound += HandleTrackerOnTargetFound;
            tracker.OnTargetUpdated += HandleTrackerOnTargetUpdated;
            tracker.OnTargetLost += HandleTrackerOnTargetLost;
        }

        private void HandleTrackerOnTargetFound(MLImageTracker.Target target, MLImageTracker.Target.Result result)
        {
            OnTargetFound?.Invoke(new MLMarkerTrackerArgs
            {
                Reference = _reference,
                Result = result,
            });
        }
        
        private void HandleTrackerOnTargetUpdated(MLImageTracker.Target target, MLImageTracker.Target.Result result)
        {
            OnTargetUpdated?.Invoke(new MLMarkerTrackerArgs
            {
                Reference = _reference,
                Result = result,
            });
        }

        private void HandleTrackerOnTargetLost(MLImageTracker.Target target, MLImageTracker.Target.Result result)
        {
            OnTargetLost?.Invoke(new MLMarkerTrackerArgs
            {
                Reference = _reference,
                Result = result,
            });
        }
    }
}
#endif