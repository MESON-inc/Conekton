using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Domain;

namespace Conekton.ARUtility.UseCase.ARMarkerIDSolver.Infrastructure
{
    public class NRMarkerIDSolver : IMarkerIDSolver<int>
    {
        [Inject] private IMarkerIDRepository _idRepository = null;

        string IMarkerIDSolver<int>.Solve(int args)
        {
            return _idRepository.Solve(args);
        }
    }
}

