using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.UseCase.ARAnchor.Domain;
using Conekton.ARUtility.UseCase.ARMarkerDetector.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARUtility.Demo
{
    public class MarkerDetectTest : MonoBehaviour
    {
        [Inject] private IARMarkerDetector _markerDetector = null;
        
        private void Start()
        {
            _markerDetector.OnDetectAnchorFirst += HandleDetectAnchorFirst;
            _markerDetector.OnUpdateAnchorPosition += HandleUpdateAnchorPosition;
        }

        private void HandleUpdateAnchorPosition(IARAnchor anchor, ARMarkerEventData eventdata)
        {
            transform.SetPositionAndRotation(anchor.Position, anchor.Rotation);
        }

        private void HandleDetectAnchorFirst(IARAnchor anchor, ARMarkerEventData eventdata)
        {
            transform.SetPositionAndRotation(anchor.Position, anchor.Rotation);
        }
    }
}
