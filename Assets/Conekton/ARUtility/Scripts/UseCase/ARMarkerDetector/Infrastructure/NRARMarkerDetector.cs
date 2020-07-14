using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID && PLATFORM_NREAL

using Zenject;
using NRKernal;

using Conekton.ARUtility.UseCase.ARAnchor.Domain;
using Conekton.ARUtility.UseCase.ARMarkerDetector.Domain;
using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Domain;

namespace Conekton.ARUtility.UseCase.ARMarkerDetector.Infrastructure
{
    public class NRARMarkerDetector : IARMarkerDetector, ITickable
    {
        [Inject] private IARAnchorService _anchorService = null;
        [Inject] private IMarkerIDSolver<int> _markerIDSolver = null;

        public DetectAnchorFirstEvent OnDetectAnchorFirst { get; set; }
        public UpdateAnchorPositionEvent OnUpdateAnchorPosition { get; set; }

        private List<NRTrackableImage> _tempTrackingImages = new List<NRTrackableImage>();
        private Dictionary<int, AnchorID> _database = new Dictionary<int, AnchorID>();

        void ITickable.Tick()
        {
            if (NRFrame.SessionStatus != SessionState.Running)
            {
                return;
            }

            CheckNewDetection();
            CheckTrackingDetection();
        }

        private IARAnchor GetOrCreateARAnchor(NRTrackableImage image)
        {
            IARAnchor anchor = null;

            AnchorID id;
            int index = image.GetDataBaseIndex();

            if (!_database.TryGetValue(index, out id))
            {
                anchor = _anchorService.Find(id);
            }

            if (anchor == null)
            {
                anchor = _anchorService.Create();
                AddToDatabase(image, anchor);
            }

            return anchor;
        }

        private void AddToDatabase(NRTrackableImage image, IARAnchor anchor)
        {
            if (!_database.ContainsKey(image.GetDataBaseIndex()))
            {
                _database.Add(image.GetDataBaseIndex(), anchor.ID);
            }
        }

        private void CheckNewDetection()
        {
            NRFrame.GetTrackables(_tempTrackingImages, NRTrackableQueryFilter.New);

            if (_tempTrackingImages.Count == 0)
            {
                return;
            }

            foreach (var image in _tempTrackingImages)
            {
                if (image.GetTrackingState() == TrackingState.Tracking)
                {
                    DetectedNewAnchor(image);
                }
            }
        }

        private void CheckTrackingDetection()
        {
            NRFrame.GetTrackables(_tempTrackingImages, NRTrackableQueryFilter.All);

            if (_tempTrackingImages.Count == 0)
            {
                return;
            }

            foreach (var image in _tempTrackingImages)
            {
                if (image.GetTrackingState() == TrackingState.Tracking)
                {
                    DetectedUpdateAnchor(image);
                }
            }
        }

        private void DetectedNewAnchor(NRTrackableImage image)
        {
            Debug.Log($"Detected new anchor with id {image.GetDataBaseIndex()}");

            IARAnchor anchor = GetOrCreateARAnchor(image);
            UpdateAnchorLocation(anchor, image.GetCenterPose());

            OnDetectAnchorFirst?.Invoke(anchor, CreateEventData(image));
        }

        private void DetectedUpdateAnchor(NRTrackableImage image)
        {
            IARAnchor anchor = GetOrCreateARAnchor(image);
            UpdateAnchorLocation(anchor, image.GetCenterPose());

            OnUpdateAnchorPosition?.Invoke(anchor, CreateEventData(image));
        }

        private void UpdateAnchorLocation(IARAnchor anchor, Pose pose)
        {
            anchor.SetPositionAndRotation(pose.position, pose.rotation);
        }

        private ARMarkerEventData CreateEventData(NRTrackableImage image)
        {
            string id = _markerIDSolver.Solve(image.GetDataBaseIndex());

            return new ARMarkerEventData
            {
                ID = id,
                Name = $"NRTrackableImage-[{id}]",
            };
        }
    }
}
#endif
