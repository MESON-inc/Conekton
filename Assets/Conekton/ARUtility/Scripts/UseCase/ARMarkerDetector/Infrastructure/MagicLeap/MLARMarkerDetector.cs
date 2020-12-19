#if PLATFORM_LUMIN
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicLeap.Core;
using UnityEngine.XR.MagicLeap;
using Zenject;
using Conekton.ARUtility.UseCase.ARAnchor.Domain;
using Conekton.ARUtility.UseCase.ARMarkerDetector.Domain;
using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Application;
using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Domain;

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
            GameObject go = new GameObject($"Track-{reference.Name}", typeof(MLMarkerTracker));
            go.transform.SetParent(transform);
            
            MLMarkerTracker tracker = go.GetComponent<MLMarkerTracker>();
            tracker.Setup(reference);
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

        private void HandleImageTrackerBehaviorOnTargetFound(MLMarkerTrackerArgs args)
        {
            DetectedNewAnchor(args);
        }

        private void HandleImageTrackerBehaviorOnTargetUpdated(MLMarkerTrackerArgs args)
        {
            DetectedUpdateAnchor(args);
        }

        private void HandleImageTrackerBehaviorOnTargetLost(MLMarkerTrackerArgs args)
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

        private IARAnchor GetOrCreateARAnchor(MLMarkerTrackerArgs args)
        {
            if (TryGetARAnchor(args, out IARAnchor anchor))
            {
                return anchor;
            }

            anchor = _anchorService.Create();
            AddToDatabase(args, anchor);

            return anchor;
        }

        private bool TryGetARAnchor(MLMarkerTrackerArgs args, out IARAnchor anchor)
        {
            AnchorID anchorID;
            string markerID = GetMarkerID(args);

            if (_database.TryGetValue(markerID, out anchorID))
            {
                anchor = _anchorService.Find(anchorID);
                return true;
            }

            anchor = null;
            return false;
        }

        private void AddToDatabase(MLMarkerTrackerArgs args, IARAnchor anchor)
        {
            string markerID = GetMarkerID(args);
            if (!_database.ContainsKey(markerID))
            {
                _database.Add(markerID, anchor.ID);
            }
        }

        private void DetectedNewAnchor(MLMarkerTrackerArgs args)
        {
            string markerID = GetMarkerID(args);

            Debug.Log($"Detected new marker [{markerID}]");

            IARAnchor anchor = GetOrCreateARAnchor(args);
            UpdateAnchorLocation(anchor, args.Result.Position, args.Result.Rotation);

            OnDetectAnchorFirst?.Invoke(anchor, CreateEventData(args));
        }

        private void DetectedUpdateAnchor(MLMarkerTrackerArgs args)
        {
            if (!TryGetARAnchor(args, out IARAnchor anchor))
            {
                return;
            }

            UpdateAnchorLocation(anchor, args.Result.Position, args.Result.Rotation);

            OnUpdateAnchorPosition?.Invoke(anchor, CreateEventData(args));
        }

        private void UpdateAnchorLocation(IARAnchor anchor, Vector3 position, Quaternion rotation)
        {
            anchor.SetPositionAndRotation(position, rotation);
        }

        private string GetMarkerID(MLMarkerTrackerArgs args)
        {
            return args.Reference.ID;
        }

        private ARMarkerEventData CreateEventData(MLMarkerTrackerArgs args)
        {
            string id = GetMarkerID(args);

            return new ARMarkerEventData
            {
                ID = id,
                Name = $"NRTrackableImage-[{id}]",
            };
        }
    }
}
#endif