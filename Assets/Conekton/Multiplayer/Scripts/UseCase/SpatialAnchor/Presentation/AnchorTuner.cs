using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARMultiplayer.SpatialAnchor.Domain;

namespace Conekton.ARMultiplayer.SpatialAnchor.Infrastructure
{
    public class AnchorTuner : MonoBehaviour, ISpatialAnchorTuner
    {
        private ISpatialAnchor _anchor = null;

        void ISpatialAnchorTuner.BindAnchor(ISpatialAnchor anchor)
        {
            _anchor = anchor;
        }

        private void Update()
        {
            _anchor.SetPose(new Pose { position = transform.position, rotation = transform.rotation, });
        }
    }
}

