using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private Transform _targetTrans = null;

        private Dictionary<string, IARAnchor> _database = new Dictionary<string, IARAnchor>();

        private string _idStr = "0";

        private void OnGUI()
        {
            _idStr = GUI.TextField(new Rect(10, 50, 50, 30), _idStr);

            if (GUI.Button(new Rect(70, 50, 130, 30), $"Detect Marker {_idStr}"))
            {
                DetectedAnchor();
            }

            if (GUI.Button(new Rect(70, 90, 130, 30), $"Update Marker {_idStr}"))
            {
                UpdateAnchor();
            }
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
                string id = _markerIDSolver.Solve(index);
                _database.Add(id, anchor);
                FireDetectedEvent(anchor, id);
            }
        }

        private void UpdateAnchor()
        {
            if (int.TryParse(_idStr, out int index))
            {
                string id = _markerIDSolver.Solve(index);
                FireUpdateEvent(id);
            }
        }

        private void SetAnchorLocation(IARAnchor anchor)
        {
            anchor.SetPositionAndRotation(_targetTrans.position, _targetTrans.rotation);
        }

        private void FireDetectedEvent(IARAnchor anchor, string id)
        {
            Debug.Log($"Detected an anchor with {id}");

            SetAnchorLocation(anchor);

            OnDetectAnchorFirst?.Invoke(anchor, new ARMarkerEventData
            {
                ID = id,
                Name = $"EditorTrackableImage-[{id}]",
            });
        }

        private void FireUpdateEvent(string id)
        {
            Debug.Log($"Updated an anchor with {id}");

            if (_database.TryGetValue(id, out IARAnchor anchor))
            {
                SetAnchorLocation(anchor);

                OnUpdateAnchorPosition?.Invoke(anchor, new ARMarkerEventData
                {
                    ID = id,
                    Name = $"EditorTrackableImage-[{id}]",
                });
            }
        }
    }
}