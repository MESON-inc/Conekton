using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.UseCase.WorldOrigin.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARUtility.UseCase.WorldOrigin.Infrastructure
{
    /// <summary>
    /// This class just pass a marker to the origin.
    /// </summary>
    public class WorldMarkerController : IWorldMarkerController
    {
        [Inject] private IWorldOrigin _worldOrigin = null;
        
        public void AddMarker(IWorldMarker marker, WorldMarkerArgs args = null)
        {
            _worldOrigin.AddMarker(marker);
        }
    }
}