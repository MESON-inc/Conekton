using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conekton.ARUtility.UseCase.WorldOrigin.Domain
{
    public delegate void MovedEvent(IWorldOrigin origin);

    /// <summary>
    /// This class will be used an argument if a system needs any arguments.
    /// </summary>
    public class WorldMarkerArgs : EventArgs { }
    
    /// <summary>
    /// IWorldOrigin interface represents a content origin pose.
    ///
    /// All of networked objects are based on this origin.
    /// </summary>
    public interface IWorldOrigin
    {
        event MovedEvent OnMoved;
        Transform Transform { get; }
        void AddMarker(IWorldMarker marker);
        void Clear();
    }

    /// <summary>
    /// IWorldAnchorController controls all of IWorldMarkers.
    /// </summary>
    public interface IWorldMarkerController
    {
        void AddMarker(IWorldMarker marker, WorldMarkerArgs args = null);
    }

    /// <summary>
    /// IWorldAnchor interface is used an anchor for an IWorldOrigin.
    /// This is supposed an anchor following the origin.
    /// </summary>
    public interface IWorldAnchor
    {
        void SetOrigin(IWorldOrigin origin);
    }

    /// <summary>
    /// IWorldMarker interface represents an anchor based on something.
    /// It depends on purpose to use.
    /// </summary>
    public interface IWorldMarker
    {
        Pose RelativePose { get; }
    }
}