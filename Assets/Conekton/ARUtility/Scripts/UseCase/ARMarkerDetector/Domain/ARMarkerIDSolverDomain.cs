using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conekton.ARUtility.UseCase.ARMarkerIDSolver.Domain
{
    public interface IMarkerIDSolver<T>
    {
        string Solve(T args);
    }

    public interface IMarkerIDRepository
    {
        string Solve(int index);
    }
}

