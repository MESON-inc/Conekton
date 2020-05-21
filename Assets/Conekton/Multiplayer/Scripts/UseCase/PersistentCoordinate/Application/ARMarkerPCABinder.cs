using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARUtility.UseCase.ARAnchor.Domain;
using Conekton.ARUtility.UseCase.ARMarkerDetector.Domain;
using Conekton.ARMultiplayer.PersistentCoordinate.Domain;
using Conekton.ARMultiplayer.PersistentCoordinate.Infrastructure;

namespace Conekton.ARMultiplayer.PersistentCoordinate.Application
{
    /// <summary>
    /// This class provides binding between an AR marker and PCA.
    /// <see cref="ARMarkerEventData"/> is used for determining PCA ID.
    /// The ID is resolved by Marker ID solver.
    /// </summary>
    public class ARMarkerPCABinder : IInitializable
    {
        [Inject] private IPersistentCoordinateService _service = null;
        [Inject] private IARMarkerDetector _markerDetector = null;

        void IInitializable.Initialize()
        {
            _markerDetector.OnDetectAnchorFirst += HandleDetectAnchorFirst;
        }

        private void HandleDetectAnchorFirst(IARAnchor anchor, ARMarkerEventData eventData)
        {
            AddPCA(anchor.Transform, eventData.ID);
        }

        private void AddPCA(Transform target, string markerID)
        {
            IPCA pca = CreatePCA(target, markerID);
            _service.Register(pca);
        }

        private IPCA CreatePCA(Transform target, string markerID)
        {
            return new ARMarkerPCA(target, markerID, Priority.High);
        }
    }
}

