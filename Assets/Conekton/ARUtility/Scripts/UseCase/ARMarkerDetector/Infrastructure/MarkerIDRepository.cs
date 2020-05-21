using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Domain;
using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Application;

namespace Conekton.ARUtility.UseCase.ARMarkerIDSolver.Infrastructure
{
    public class MarkerIDRepository : IMarkerIDRepository
    {
        [Inject] private MarkerIDDatabase _markerIDDatabase = null;

        string IMarkerIDRepository.Solve(int index)
        {
            if (_markerIDDatabase.TryGetItemByIndex(index, out MarkerIDDatabaseItem item))
            {
                return item.ID;
            }

            return "";
        }
    }
}

