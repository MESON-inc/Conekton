using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if (UNITY_IOS || UNITY_ANDROID) && !(PLATFORM_NREAL || PLATFORM_OCULUS)
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

using Zenject;

using Conekton.ARUtility.UseCase.ARAnchor.Domain;
using Conekton.ARUtility.UseCase.ARMarkerDetector.Application;
using Conekton.ARUtility.UseCase.ARMarkerDetector.Domain;
using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Domain;

namespace Conekton.ARUtility.UseCase.ARMarkerDetector.Infrastructure
{
    public class MobileARMarkerDetector : IARMarkerDetector, IInitializable
    {
        public DetectAnchorFirstEvent OnDetectAnchorFirst { get; set; }
        public UpdateAnchorPositionEvent OnUpdateAnchorPosition { get; set; }

        [Inject] private IARAnchorService _anchorService = null;
        [Inject] private ARTrackedImageManagerProvider _arTrackedImageManagerProvider = null;
        [Inject] private IMarkerIDSolver<XRReferenceImage> _markerIDSolver = null;

        private ARTrackedImageManager _arTrackedImageManager = null;
        private Dictionary<string, AnchorID> _database = new Dictionary<string, AnchorID>();

        void IInitializable.Initialize()
        {
            _arTrackedImageManager = _arTrackedImageManagerProvider.GetManager();
            _arTrackedImageManager.trackedImagesChanged += HandleTrackedImagesChanged;
        }

        private void HandleTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
        {
            CheckDetection(args);
        }

        private IARAnchor GetOrCreateARAnchor(ARTrackedImage image)
        {
            IARAnchor anchor = null;

            AnchorID id;
            if (!_database.TryGetValue(image.name, out id))
            {
                anchor = _anchorService.Find(id);
            }

            if (anchor == null)
            {
                anchor = _anchorService.Create();
                AddIDToDatabase(image.name, anchor.ID);
            }

            return anchor;
        }

        private void CheckDetection(ARTrackedImagesChangedEventArgs args)
        {
            CheckNewDetection(args);
            CheckUpdateDetection(args);
        }

        private void CheckNewDetection(ARTrackedImagesChangedEventArgs args)
        {
            if (args.added.Count == 0)
            {
                return;
            }

            foreach (var image in args.added)
            {
                DetectedNewImage(image);
            }
        }

        private void CheckUpdateDetection(ARTrackedImagesChangedEventArgs args)
        {
            if (args.updated.Count == 0)
            {
                return;
            }

            foreach (var image in args.updated)
            {
                DetectedUpdatedImage(image);
            }
        }

        private void AddIDToDatabase(string name, AnchorID id)
        {
            if (!_database.ContainsKey(name))
            {
                _database.Add(name, id);
            }
        }

        private void DetectedNewImage(ARTrackedImage image)
        {
            if (_database.ContainsKey(image.name))
            {
                return;
            }

            Debug.Log($"Detected new image {image.name}");

            IARAnchor anchor = GetOrCreateARAnchor(image);
            UpdateAnchorLocation(anchor, image.transform);

            OnDetectAnchorFirst?.Invoke(anchor, CreateEventData(image));
        }

        private void DetectedUpdatedImage(ARTrackedImage image)
        {
            if (!_database.ContainsKey(image.name))
            {
                return;
            }

            IARAnchor anchor = GetOrCreateARAnchor(image);
            UpdateAnchorLocation(anchor, image.transform);

            OnUpdateAnchorPosition?.Invoke(anchor, CreateEventData(image));
        }

        private void UpdateAnchorLocation(IARAnchor anchor, Transform target)
        {
            anchor.SetPositionAndRotation(target.position, target.rotation);
        }

        private ARMarkerEventData CreateEventData(ARTrackedImage image)
        {
            string id = _markerIDSolver.Solve(image.referenceImage);

            return new ARMarkerEventData
            {
                ID = id,
                Name = $"ARTrackedImage-[{id}]",
            };
        }
    }
}
#endif
