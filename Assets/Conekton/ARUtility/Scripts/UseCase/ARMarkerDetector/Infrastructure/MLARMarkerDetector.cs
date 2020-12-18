#if PLATFORM_LUMIN
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


using MagicLeap.Core;
using UnityEngine.XR.MagicLeap;

using Zenject;

using Conekton.ARUtility.UseCase.ARAnchor.Domain;
using Conekton.ARUtility.UseCase.ARMarkerDetector.Domain;
using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Application;
using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Domain;
using MagicLeap.Core.StarterKit;

namespace Conekton.ARUtility.UseCase.ARMarkerDetector.Infrastructure
{
    [RequireComponent(typeof(MLPrivilegeRequesterBehavior))]
    public class MLARMarkerDetector : MonoBehaviour, IARMarkerDetector, IInitializable
    {
        [Inject] private IARAnchorService _anchorService = null;
        [Inject] private IMarkerIDSolver<string> _markerIDSolver = null;
        [Inject] private MLReferenceImageLibrary _MLReferenceImageLibrary = null;

        public DetectAnchorFirstEvent OnDetectAnchorFirst { get; set; }
        public UpdateAnchorPositionEvent OnUpdateAnchorPosition { get; set; }

        private Dictionary<string, AnchorID> _database = new Dictionary<string, AnchorID>();
        private MLPrivilegeRequesterBehavior _privilegeRequester = null;
        private MLImageTrackerBehavior _imageTrackerBehavior = null;

        void IInitializable.Initialize()
        {
            _privilegeRequester = GetComponent<MLPrivilegeRequesterBehavior>();
            _privilegeRequester.OnPrivilegesDone += HandlePrivilegesDone;

            foreach (var data in _MLReferenceImageLibrary.DataList)
            {
                MLImageTrackerBehavior tracker = gameObject.AddComponent<MLImageTrackerBehavior>();
                tracker.image = data.TargetTexture;
                tracker.autoUpdate = true;
            }

            _imageTrackerBehavior = GetComponent<MLImageTrackerBehavior>();
            _imageTrackerBehavior.OnTargetFound += HandleImageTrackerBehaviorOnTargetFound;
            _imageTrackerBehavior.OnTargetLost += HandleImageTrackerBehaviorOnTargetLost;
            _imageTrackerBehavior.OnTargetUpdated += HandleImageTrackerBehaviorOnTargetUpdated;
        }

        private void HandleImageTrackerBehaviorOnTargetFound(MLImageTracker.Target target, MLImageTracker.Target.Result result)
        {
            DetectedNewAnchor(target, result);
        }

        private void HandleImageTrackerBehaviorOnTargetUpdated(MLImageTracker.Target target, MLImageTracker.Target.Result result)
        {
            DetectedUpdateAnchor(target, result);
        }

        private void HandleImageTrackerBehaviorOnTargetLost(MLImageTracker.Target target, MLImageTracker.Target.Result result)
        {
            // do nothing.
        }

        private void HandlePrivilegesDone(MLResult result)
        {
            if (!result.IsOk)
            {
                Debug.Log("Error: Privilege request failed.");
                return;
            }

            Debug.Log("Success: Privilege granted.");
        }
        
        private IARAnchor GetOrCreateARAnchor(MLImageTracker.Target target)
        {
            IARAnchor anchor = null;

            AnchorID id;
            string name = target.TargetSettings.Name;

            if (!_database.TryGetValue(name, out id))
            {
                anchor = _anchorService.Find(id);
            }

            if (anchor == null)
            {
                anchor = _anchorService.Create();
                AddToDatabase(target, anchor);
            }

            return anchor;
        }

        private void AddToDatabase(MLImageTracker.Target target, IARAnchor anchor)
        {
            if (!_database.ContainsKey(target.TargetSettings.Name))
            {
                _database.Add(target.TargetSettings.Name, anchor.ID);
            }
        }

        private void DetectedNewAnchor(MLImageTracker.Target target, MLImageTracker.Target.Result result)
        {
            string name = target.TargetSettings.Name;
            
            Debug.Log($"Detected new anchor with name {name}");

            IARAnchor anchor = GetOrCreateARAnchor(target);
            UpdateAnchorLocation(anchor, result.Position, result.Rotation);

            OnDetectAnchorFirst?.Invoke(anchor, CreateEventData(target));
        }

        private void DetectedUpdateAnchor(MLImageTracker.Target target, MLImageTracker.Target.Result result)
        {
            IARAnchor anchor = GetOrCreateARAnchor(target);
            
            UpdateAnchorLocation(anchor, result.Position, result.Rotation);

            OnUpdateAnchorPosition?.Invoke(anchor, CreateEventData(target));
        }

        private void UpdateAnchorLocation(IARAnchor anchor, Vector3 position, Quaternion rotation)
        {
            anchor.SetPositionAndRotation(position, rotation);
        }

        private ARMarkerEventData CreateEventData(MLImageTracker.Target target)
        {
            string id = _markerIDSolver.Solve(target.TargetSettings.Name);

            return new ARMarkerEventData
            {
                ID = id,
                Name = $"NRTrackableImage-[{id}]",
            };
        }
    }
}
#endif
