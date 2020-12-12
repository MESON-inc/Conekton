using System;
using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.UseCase.ARAnchor.Domain;
using Conekton.ARUtility.UseCase.ARMarkerDetector.Domain;
using Conekton.ARUtility.UseCase.WorldOrigin.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARUtility.UseCase.WorldOrigin.Infrastructure
{
    public class ARMarkerWorldMarker : MonoBehaviour, IWorldMarker
    {
        [Inject] private IARMarkerDetector _markerDetector = null;
        [Inject] private IWorldMarkerController _markerController = null;

        [SerializeField] private string _targetName = "";
        [SerializeField] private Transform _target = null;

        public Pose RelativePose => new Pose(_target.position, _target.rotation);

        private IARAnchor _arAnchor = null;
        private bool _hasARAnchor = false;

        private void Awake()
        {
            _markerDetector.OnUpdateAnchorPosition += HandleOnUpdateAnchorPosition;
        }

        private void Update()
        {
            if (!_hasARAnchor)
            {
                return;
            }

            if (_arAnchor.Transform.hasChanged)
            {
                UpdatePose();
            }
        }

        private void UpdatePose()
        {
            transform.SetPositionAndRotation(_arAnchor.Position, _arAnchor.Rotation);
        }

        private void HandleOnUpdateAnchorPosition(IARAnchor anchor, ARMarkerEventData eventdata)
        {
            if (eventdata.ID != _targetName)
            {
                return;
            }

            if (anchor == null)
            {
                return;
            }
            
            _hasARAnchor = true;
            _arAnchor = anchor;
            
            UpdatePose();
            
            _markerController.AddMarker(this);
            
            _markerDetector.OnUpdateAnchorPosition -= HandleOnUpdateAnchorPosition;
        }
    }
}