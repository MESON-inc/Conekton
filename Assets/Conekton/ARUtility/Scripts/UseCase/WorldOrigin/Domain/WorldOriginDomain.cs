using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conekton.ARUtility.UseCase.WorldOrigin.Domain
{
    public delegate void MovedEvent(IWorldOrigin origin);
    
    public interface IWorldOriginService
    {
        void UnRegister(IWorldOrigin origin);
        void Register(IWorldOrigin origin);
        IWorldOrigin GetWorldOrigin();
    }
    
    public interface IWorldOrigin
    {
        event MovedEvent OnMoved;
        Transform Transform { get; }
        void MoveToPose(Pose pose);
        void SetAnchor(IWorldAnchor anchor);
    }

    public interface IWorldAnchor
    {
        Transform Transform { get; }
    }
}