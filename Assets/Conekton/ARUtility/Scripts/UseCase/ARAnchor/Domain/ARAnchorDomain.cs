using UnityEngine;

namespace Conekton.ARUtility.UseCase.ARAnchor.Domain
{
    /// <summary>
    /// Anchor ID data structure.
    /// 
    /// This struct will be used matching anchor objects.
    /// </summary>
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

    /// <summary>
    /// IARAnchor provides ways to asscess / control the object pose.
    /// </summary>
    public interface IARAnchor
    {
        AnchorID ID { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Transform Transform { get; }
        void SetPositionAndRotation(Vector3 position, Quaternion rotation);
    }

    /// <summary>
    /// IARAnchorService seems like an end point from a client to this system.
    /// </summary>
    public interface IARAnchorService
    {
        IARAnchor Create();
        IARAnchor Find(AnchorID anchorID);
    }

    /// <summary>
    /// IARAnchorSystem is the system.
    /// 
    /// The class that implement this interface will provide ways that are Create and Find.
    /// </summary>
    public interface IARAnchorSystem
    {
        IARAnchor Create();
        IARAnchor Find(AnchorID anchorID);
    }

    /// <summary>
    /// IARAnchorRepository is a repository to store all IARAnchor objects.
    /// </summary>
    public interface IARAnchorRepository
    {
        IARAnchor Create(AnchorID anchorID);
        IARAnchor Find(AnchorID anchorID);
    }
}

