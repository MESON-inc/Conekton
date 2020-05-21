using UnityEngine;

using Zenject;

using Conekton.ARMultiplayer.SpatialAnchor.Domain;

namespace Conekton.ARMultiplayer.SpatialAnchor.Presentation
{
    public class SpatialAnchor : MonoBehaviour, ISpatialAnchor
    {
        public class SpatialAnchorFactory : PlaceholderFactory<SpatialAnchorID, SpatialAnchor> { }

        private SpatialAnchorID _anchorID = SpatialAnchorID.NoSet;

        SpatialAnchorID ISpatialAnchor.AnchorID => _anchorID;

        [Inject]
        private void Contruct(SpatialAnchorID anchorID)
        {
            _anchorID = anchorID;

            name = _anchorID.ToString();
        }

        void ISpatialAnchor.AddTransform(Transform target)
        {
            target.SetParent(transform);
        }

        void ISpatialAnchor.SetPose(Pose pose)
        {
            transform.SetPositionAndRotation(pose.position, pose.rotation);
        }
    }
}

