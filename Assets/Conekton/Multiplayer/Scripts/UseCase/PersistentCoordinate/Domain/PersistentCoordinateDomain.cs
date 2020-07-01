using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Conekton.ARMultiplayer.PersistentCoordinate.Domain
{
    public enum Priority
    {
        None = 0,
        Low = 1,
        Middle = 10,
        High = 100,
    }

    /// <summary>
    /// This is a struct to use PCA's ID.
    /// </summary>
    public struct PCAID
    {
        static public bool IsNotSet(PCAID id)
        {
            if (id == default)
            {
                return true;
            }

            return false;
        }

        public string ID;

        static public bool operator ==(PCAID a, PCAID b)
        {
            return a.ID == b.ID;
        }

        static public bool operator !=(PCAID a, PCAID b)
        {
            return a.ID != b.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (PCAID)obj == this;
        }

        public override string ToString()
        {
            return $"PCA_ID:{ID}";
        }
    }

    /// <summary>
    /// IPCA is abstructed of some fixed anchor in a space.
    /// </summary>
    public interface IPCA
    {
        PCAID ID { get; set; }
        string UniqueID { get; }
        Priority Priority { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
    }

    /// <summary>
    /// IPersistentCoordinateService is an end point from a client.
    /// </summary>
    public interface IPersistentCoordinateService
    {
        IReadOnlyCollection<IPCA> GetAllPCA();
        IPCA[] GetNearPCA(Vector3 position, int count);
        void Register(IPCA pca);
        void Unregister(IPCA pca);
    }

    /// <summary>
    /// IPersistentCoordinateSystem provides ways to access all IPCAs.
    /// </summary>
    public interface IPersistentCoordinateSystem
    {
        IReadOnlyCollection<IPCA> GetAllPCA();
        IPCA[] GetNearPCA(Vector3 position, int count);
        void Register(IPCA pca);
        void Unregister(IPCA pca);
    }

    /// <summary>
    /// IPersistentCoordinateRepository is a repository.
    /// 
    /// The class that implemented this interface will store all IPCA objects.
    /// </summary>
    public interface IPersistentCoordinateRepository
    {
        IReadOnlyCollection<IPCA> GetAllPCA();
        void Register(IPCA pca);
        void Unregister(IPCA pca);
    }
}

