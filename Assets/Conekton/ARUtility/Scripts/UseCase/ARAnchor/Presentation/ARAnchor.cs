using UnityEngine;

using Zenject;

using Conekton.ARUtility.UseCase.ARAnchor.Domain;

namespace Conekton.ARUtility.UseCase.ARAnchor.Presentation
{
    public class ARAnchor : MonoBehaviour, IARAnchor
    {
        public class Factory : PlaceholderFactory<AnchorID, ARAnchor> { }

        private AnchorID _anchorID = AnchorID.NoSet;

        AnchorID IARAnchor.ID => _anchorID;
        Vector3 IARAnchor.Position => transform.position;
        Quaternion IARAnchor.Rotation => transform.rotation;
        Transform IARAnchor.Transform => transform;

        [Inject]
        public void Construct(AnchorID anchorID)
        {
            _anchorID = anchorID;

            name = $"{anchorID}";
        }

        void IARAnchor.SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
        }
    }
}

