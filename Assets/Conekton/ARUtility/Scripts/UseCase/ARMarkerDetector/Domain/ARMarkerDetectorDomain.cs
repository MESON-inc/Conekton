using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARUtility.UseCase.ARAnchor.Domain;

namespace Conekton.ARUtility.UseCase.ARMarkerDetector.Domain
{
    /// <summary>
    /// This event will be invoked when an AR anchor has detected.
    /// </summary>
    /// <param name="anchor">Reference to an IARAnchor object</param>
    /// <param name="eventData">An ARMarkerEventData</param>
    public delegate void DetectAnchorFirstEvent(IARAnchor anchor, ARMarkerEventData eventData);

    /// <summary>
    /// This event will be invoked when an AR anchor has updated.
    /// </summary>
    /// <param name="anchor"></param>
    /// <param name="eventData"></param>
    public delegate void UpdateAnchorPositionEvent(IARAnchor anchor, ARMarkerEventData eventData);

    /// <summary>
    /// This represent a data of an ARAnchor.
    /// 
    /// ID and Name will be provided by a platform.
    /// </summary>
    public struct ARMarkerEventData
    {
        public string ID;
        public string Name;
    }

    /// <summary>
    /// IARMarkerDetecotr provides to ways to register each event.
    /// </summary>
    public interface IARMarkerDetector
    {
        DetectAnchorFirstEvent OnDetectAnchorFirst { get; set; }
        UpdateAnchorPositionEvent OnUpdateAnchorPosition { get; set; }
    }
}

