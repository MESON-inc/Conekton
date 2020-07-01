using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conekton.ARUtility.UseCase.ARMarkerIDSolver.Domain
{
    /// <summary>
    /// IMarkerIDSolver provides functionalities to resolve ID from any object to the correct ID.
    /// 
    /// This interface must be implemented to resolve the ID if needed.
    /// </summary>
    /// <typeparam name="T">T depends on an object that is provided by a platform.</typeparam>
    public interface IMarkerIDSolver<T>
    {
        string Solve(T args);
    }

    /// <summary>
    /// IMarkerIDRepository provides a way to resolve any index to an ID.
    /// 
    /// The class that implement this interface can have some ID database.
    /// </summary>
    public interface IMarkerIDRepository
    {
        string Solve(int index);
    }
}

