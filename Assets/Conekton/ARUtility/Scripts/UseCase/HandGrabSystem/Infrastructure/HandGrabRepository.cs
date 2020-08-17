using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.HandGrabSystemUseCase.Domain;
using UnityEngine;

namespace Conekton.ARUtility.HandGrabSystemUseCase.Infrastructure
{
    public class HandGrabRepository : IHandGrabReopsitory
    {
        private readonly List<IGrabbable> _grabbables = new List<IGrabbable>();
        
        void IHandGrabReopsitory.Add(IGrabbable grabbable)
        {
            if (!_grabbables.Contains(grabbable))
            {
                _grabbables.Add(grabbable);
            }
        }

        void IHandGrabReopsitory.Remove(IGrabbable grabbable)
        {
            if (_grabbables.Contains(grabbable))
            {
                _grabbables.Remove(grabbable);
            }
        }

        IReadOnlyList<IGrabbable> IHandGrabReopsitory.GetAllGrabbables()
        {
            return _grabbables;
        }
    }
}
