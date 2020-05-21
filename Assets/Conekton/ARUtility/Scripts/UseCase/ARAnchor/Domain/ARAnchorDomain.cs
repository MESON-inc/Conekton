using UnityEngine;

namespace Conekton.ARUtility.UseCase.ARAnchor.Domain
{
    public struct AnchorID
    {
        static public AnchorID NoSet => new AnchorID { ID = -1, };

        public int ID;

        static public bool operator ==(AnchorID a, AnchorID b)
        {
            return a.ID == b.ID;
        }

        static public bool operator !=(AnchorID a, AnchorID b)
        {
            return a.ID != b.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public override bool Equals(object obj)
        {
            return (AnchorID)obj == this;
        }

        public override string ToString()
        {
            return $"AR Anchor ID - [{ID}]";
        }
    }

    public interface IARAnchor
    {
        AnchorID ID { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Transform Transform { get; }
        void SetPositionAndRotation(Vector3 position, Quaternion rotation);
    }

    public interface IARAnchorService
    {
        IARAnchor Create();
        IARAnchor Find(AnchorID anchorID);
    }

    public interface IARAnchorSystem
    {
        IARAnchor Create();
        IARAnchor Find(AnchorID anchorID);
    }

    public interface IARAnchorRepository
    {
        IARAnchor Create(AnchorID anchorID);
        IARAnchor Find(AnchorID anchorID);
    }
}

