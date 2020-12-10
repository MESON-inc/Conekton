using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conekton.ARUtility.UseCase.WorldOrigin.Domain
{
    public delegate void MovedEvent(IWorldOrigin origin);
    
    public interface IWorldOrigin
    {
        event MovedEvent OnMoved;
        Transform Transform { get; }
        void AddAnchor(IWorldAnchor anchor);
        void Clear();
    }

    public interface IWorldAnchor
    {
        Pose RelativePose { get; }
    }
}