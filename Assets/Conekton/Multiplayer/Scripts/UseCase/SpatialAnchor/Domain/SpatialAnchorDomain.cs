using UnityEngine;

using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;

namespace Conekton.ARMultiplayer.SpatialAnchor.Domain
{
    /// <summary>
    /// SpatialAnchorID is a data structure.
    /// 
    /// This class will be used matching each IPCA object.
    /// </summary>
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

    /// <summary>
    /// This event will invoke when an ISpatialAnchor is created.
    /// </summary>
    /// <param name="anchor"></param>
    public delegate void CreatedAnchorEvent(ISpatialAnchor anchor);

    /// <summary>
    /// ISpatialAnchor provides ways to access an ISpatialAnchor object.
    /// </summary>
    public interface ISpatialAnchor
    {
        void AddTransform(Transform target);
        SpatialAnchorID AnchorID { get; }
        void SetPose(Pose pose);
    }

    /// <summary>
    /// ISpatialAnchorTuner is like a tuner.
    /// 
    /// The class that implmenented this interface will control an ISpatialAnchor that is connected this concrete class.
    /// </summary>
    public interface ISpatialAnchorTuner
    {
        void BindAnchor(ISpatialAnchor anchor);
    }

    /// <summary>
    /// ISpatialAnchorService is an end point to access the spatial anchor system. 
    /// </summary>
    public interface ISpatialAnchorService
    {
        event CreatedAnchorEvent OnCreatedAnchor;
        void RegisterTuner(ISpatialAnchorTuner tuner, object args);
    }

    /// <summary>
    /// ISpatialAnchorSystem provides ways to control all ISpatialAnchor objects.
    /// </summary>
    public interface ISpatialAnchorSystem
    {
        event CreatedAnchorEvent OnCreatedAnchor;
        ISpatialAnchor GetOrCreateAnchor(PlayerID playerID);
        ISpatialAnchorTuner CreateTuner();
        void RegisterTuner(ISpatialAnchorTuner tuner, PlayerID playerID);
    }

    /// <summary>
    /// ISpatialAnchorRepository is a repository.
    /// 
    /// The class that implemented this interface will store all ISpatialAnchor objects.
    /// </summary>
    public interface ISpatialAnchorRepository
    {
        ISpatialAnchor Create(SpatialAnchorID anchorID);
        ISpatialAnchor Find(SpatialAnchorID anchorID);
    }

    /// <summary>
    /// ISpatialAnchorTunerRepository is a repository.
    /// 
    /// The class that implemented this interface will store all anchor tuners.
    /// </summary>
    public interface ISpatialAnchorTunerRepository
    {
        ISpatialAnchorTuner Create();
    }
}

