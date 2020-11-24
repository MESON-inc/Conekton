using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.GrabSystemUseCase.Domain;
using UnityEngine;

namespace Conekton.ARUtility.GrabSystemUseCase.Infrastructure
{
    public class GrabRepository : IGrabReopsitory
    {
        private readonly List<IGrabbable> _grabbables = new List<IGrabbable>();
        
        void IGrabReopsitory.Add(IGrabbable grabbable)
        {
            if (!_grabbables.Contains(grabbable))
            {
                _grabbables.Add(grabbable);
            }
        }

        void IGrabReopsitory.Remove(IGrabbable grabbable)
        {
            if (_grabbables.Contains(grabbable))
            {
                _grabbables.Remove(grabbable);
            }
        }

        IReadOnlyList<IGrabbable> IGrabReopsitory.GetAllGrabbables()
        {
            return _grabbables;
        }
    }
}
