using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARUtility.UseCase.ARAnchor.Domain;

namespace Conekton.ARUtility.UseCase.ARMarkerDetector.Domain
{
    public delegate void DetectAnchorFirstEvent(IARAnchor anchor, ARMarkerEventData eventData);
    public delegate void UpdateAnchorPositionEvent(IARAnchor anchor, ARMarkerEventData eventData);

    public struct ARMarkerEventData
    {
        public string ID;
        public string Name;
    }

    public interface IARMarkerDetector
    {
        DetectAnchorFirstEvent OnDetectAnchorFirst { get; set; }
        UpdateAnchorPositionEvent OnUpdateAnchorPosition { get; set; }
    }
}

