#if PLATFORM_LUMIN
using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Application;
using UnityEngine;

using Zenject;

using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Domain;

namespace Conekton.ARUtility.UseCase.ARMarkerIDSolver.Infrastructure
{
    public class MLMarkerIDSolver : IMarkerIDSolver<string>
    {
        [Inject] private IMarkerIDRepository _idRepository = null;
        [Inject] private MLReferenceImageLibrary _MLReferenceImageLibrary = null;

        string IMarkerIDSolver<string>.Solve(string args)
        {
            int idx = _MLReferenceImageLibrary.IndexOf(args);
            return _idRepository.Solve(idx);
        }
    }
}
#endif
