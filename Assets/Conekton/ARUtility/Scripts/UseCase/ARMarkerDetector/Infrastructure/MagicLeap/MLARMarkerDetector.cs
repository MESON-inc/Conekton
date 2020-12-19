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

        void IInitializable.Initialize()
        {
            _privilegeRequester = GetComponent<MLPrivilegeRequesterBehavior>();
            _privilegeRequester.OnPrivilegesDone += HandlePrivilegesDone;

            SetupTrackerBehaviours();
        }

        private void CreateImageTracker(MLReferenceImage reference)
        {
            GameObject go = new GameObject($"Track-{reference.Name}", typeof(MLImageTrackerBehavior));
            go.transform.SetParent(transform);
            
            MLImageTrackerBehavior tracker = go.GetComponent<MLImageTrackerBehavior>();
            tracker.image = reference.TargetTexture;
            tracker.autoUpdate = true;
            tracker.longerDimensionInSceneUnits = reference.LongerDimensionInSceneUnits;
            tracker.OnTargetFound += HandleImageTrackerBehaviorOnTargetFound;
            tracker.OnTargetLost += HandleImageTrackerBehaviorOnTargetLost;
            tracker.OnTargetUpdated += HandleImageTrackerBehaviorOnTargetUpdated;
        }

        private void SetupTrackerBehaviours()
        {
            foreach (var reference in _MLReferenceImageLibrary.DataList)
            {
                CreateImageTracker(reference);
            }
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
            if (TryGetARAnchor(target, out IARAnchor anchor))
            {
                return anchor;
            }

            anchor = _anchorService.Create();
            AddToDatabase(target, anchor);

            return anchor;
        }

        private bool TryGetARAnchor(MLImageTracker.Target target, out IARAnchor anchor)
        {
            AnchorID id;
            string name = target.TargetSettings.Name;

            if (_database.TryGetValue(name, out id))
            {
                anchor = _anchorService.Find(id);
                return true;
            }

            anchor = null;
            return false;
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
            if (!TryGetARAnchor(target, out IARAnchor anchor))
            {
                return;
            }

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