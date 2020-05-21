using UnityEngine;

using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;

namespace Conekton.ARMultiplayer.SpatialAnchor.Domain
{
    public struct SpatialAnchorID
    {
        static public SpatialAnchorID NoSet => new SpatialAnchorID { ID = -1, };

        public int ID;
        public PlayerID PlayerID;

        static public bool operator ==(SpatialAnchorID a, SpatialAnchorID b)
        {
            return a.ID == b.ID;
        }

        static public bool operator !=(SpatialAnchorID a, SpatialAnchorID b)
        {
            return a.ID != b.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public override bool Equals(object obj)
        {
            return (SpatialAnchorID)obj == this;
        }

        public override string ToString()
        {
            return $"Spatial Anchor ID - [{ID}]";
        }
    }

    public delegate void CreatedAnchorEvent(ISpatialAnchor anchor);

    public interface ISpatialAnchor
    {
        void AddTransform(Transform target);
        SpatialAnchorID AnchorID { get; }
        void SetPose(Pose pose);
    }

    public interface ISpatialAnchorTuner
    {
        void BindAnchor(ISpatialAnchor anchor);
    }

    public interface ISpatialAnchorService
    {
        event CreatedAnchorEvent OnCreatedAnchor;
        void RegisterTuner(ISpatialAnchorTuner tuner, object args);
    }

    public interface ISpatialAnchorSystem
    {
        event CreatedAnchorEvent OnCreatedAnchor;
        ISpatialAnchor GetOrCreateAnchor(PlayerID playerID);
        ISpatialAnchorTuner CreateTuner();
        void RegisterTuner(ISpatialAnchorTuner tuner, PlayerID playerID);
    }

    public interface ISpatialAnchorRepository
    {
        ISpatialAnchor Create(SpatialAnchorID anchorID);
        ISpatialAnchor Find(SpatialAnchorID anchorID);
    }

    public interface ISpatialAnchorTunerRepository
    {
        ISpatialAnchorTuner Create();
    }
}

