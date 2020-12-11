using UnityEngine;
using Zenject;
using Conekton.ARUtility.UseCase.ARAnchor.Domain;
using Conekton.ARUtility.UseCase.ARMarkerDetector.Domain;
using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Domain;

namespace Conekton.ARUtility.UseCase.ARMarkerDetector.Infrastructure
{
    public class EditorARMarkerDetector : MonoBehaviour, IARMarkerDetector
    {
        public DetectAnchorFirstEvent OnDetectAnchorFirst { get; set; }
        public UpdateAnchorPositionEvent OnUpdateAnchorPosition { get; set; }

        [Inject] private IARAnchorService _anchorService = null;
        [Inject] private IMarkerIDSolver<int> _markerIDSolver = null;

        [SerializeField] private Vector3 _origin = Vector3.zero;
        [SerializeField] private Vector3 _euler = Vector3.zero;

        private string _idStr = "0";

        private void OnGUI()
        {
            if (GUI.Button(new Rect(70, 50, 130, 30), $"Detect Marker {_idStr}"))
            {
                DetectedAnchor();
            }

            _idStr = GUI.TextField(new Rect(10, 50, 50, 30), _idStr);
        }

        private IARAnchor CreateARAnchor()
        {
            return _anchorService.Create();
        }

        private void DetectedAnchor()
        {
            if (int.TryParse(_idStr, out int index))
            {
                IARAnchor anchor = CreateARAnchor();
                FireDetectedEvent(anchor, index);
            }
        }

        private void SetAnchorLocation(IARAnchor anchor)
        {
            anchor.SetPositionAndRotation(_origin, Quaternion.Euler(_euler));
        }

        private void FireDetectedEvent(IARAnchor anchor, int index)
        {
            SetAnchorLocation(anchor);

            string id = _markerIDSolver.Solve(index);

            OnDetectAnchorFirst?.Invoke(anchor, new ARMarkerEventData
            {
                ID = id,
                Name = $"EditorTrackableImage-[{id}]",
            });
        }
    }
}