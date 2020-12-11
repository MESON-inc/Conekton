using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conekton.ARUtility.UseCase.WorldOrigin.Domain
{
    public delegate void MovedEvent(IWorldOrigin origin);
    
    /// <summary>
    /// IWorldOrigin interface represents a content origin pose.
    ///
    /// All of networked objects are based on this origin.
    /// </summary>
    public interface IWorldOrigin
    {
        event MovedEvent OnMoved;
        Transform Transform { get; }
        void AddAnchor(IWorldAnchor anchor);
        void Clear();
    }

    /// <summary>
    /// IworldAnchor interface represents an anchor based on something.
    /// It depends on purpose to use.
    /// </summary>
    public interface IWorldAnchor
    {
        Pose RelativePose { get; }
    }
}