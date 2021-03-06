using System.Collections;
using System.Collections.Generic;
using Conekton.ARMultiplayer.Avatar.Domain;
using UnityEngine;

namespace Conekton.ARMultiplayer.AvatarBody.Domain
{
    public delegate void AvatarBodyFreeEvent(IAvatarBody avatarBody);

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

    public struct CreateAvatarBodyArgs
    {
        public byte BodyType;
        public AvatarBodyID BodyID;
    }

    public interface IAvatarBody
    {
        event AvatarBodyFreeEvent OnAvatarBodyFree;
        byte BodyType { get; }
        AvatarBodyID BodyID { get; }
        Transform Transform { get; }
        void Active(bool active);
        void SetAvatar(IAvatar avatar);
    }

    public interface IAvatarBodyIDGenerator
    {
        AvatarBodyID Generate();
    }

    public interface IAvatarBodySystem
    {
        IAvatarBody Create(byte bodyType);
        IAvatarBody Find(AvatarBodyID id);
        IAvatarBody GetOrCreate(byte bodyType);
        void Release(IAvatarBody body);
    }

    public interface IAvatarBodyFactory
    {
        IAvatarBody Create(CreateAvatarBodyArgs args);
    }

    public interface IAvatarBodyRepository
    {
        IAvatarBody Find(AvatarBodyID id);
        IAvatarBody Get(byte bodyType);
        void Save(byte bodyType, IAvatarBody avatarBody);
        void Release(IAvatarBody body);
    }
}