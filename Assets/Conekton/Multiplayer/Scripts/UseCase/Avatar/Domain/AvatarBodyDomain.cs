using System.Collections;
using System.Collections.Generic;
using Conekton.ARMultiplayer.Avatar.Domain;
using UnityEngine;

namespace Conekton.ARMultiplayer.AvatarBody.Domain
{
    public struct AvatarBodyID
    {
        private int _id;
        public int ID => _id;

        public static bool operator ==(AvatarBodyID a, AvatarBodyID b)
        {
            return a._id == b._id;
        }
        
        public static bool operator !=(AvatarBodyID a, AvatarBodyID b)
        {
            return a._id != b._id;
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public override bool Equals(object obj)
        {
            return (AvatarBodyID)obj == this;
        }

        public override string ToString()
        {
            return $"Avatar Body ID - [{_id.ToString()}]";
        }

        public AvatarBodyID(int id)
        {
            _id = id;
        }
    }
    
    public enum AvatarBodyType
    {
        Default,
    }

    public class AvatarBodyTypeArgs
    {
        public AvatarBodyType BodyType { get; set; } = AvatarBodyType.Default;
        public AvatarBodyID BodyID { get; set; }
    }
    
    public interface IAvatarBody
    {
        AvatarBodyType BodyType { get; }
        AvatarBodyID BodyID { get; }
        Transform Transform { get; }
        void Active(bool active);
        void SetAvatar(IAvatar avatar);
    }

    public interface IAvatarBodyIDGenerator
    {
        AvatarBodyID Generate();
    }
    
    public interface IAvatarBodySystem<TAvatarType> where TAvatarType : AvatarBodyTypeArgs
    {
        IAvatarBody Create(TAvatarType args);
        IAvatarBody Find(AvatarBodyID id);
        IAvatarBody Get(TAvatarType args);
        void Release(IAvatarBody body);
    }
    
    public interface IAvatarBodyRepository<TAvatarType>
    {
        IAvatarBody Find(AvatarBodyID id);
        IAvatarBody Get(TAvatarType args);
        void Save(AvatarBodyType bodyType, IAvatarBody avatarBody);
        void Release(IAvatarBody body);
    }
}
