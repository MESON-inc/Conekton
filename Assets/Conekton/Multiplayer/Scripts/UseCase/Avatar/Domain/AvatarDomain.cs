using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conekton.ARMultiplayer.Avatar.Domain
{
    public struct AvatarID
    {
        static public AvatarID NoSet => new AvatarID { ID = -1, };

        public int ID;

        static public bool operator ==(AvatarID a, AvatarID b)
        {
            return a.ID == b.ID;
        }

        static public bool operator !=(AvatarID a, AvatarID b)
        {
            return a.ID != b.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return (AvatarID)obj == this;
        }

        public override string ToString()
        {
            return $"Avatar ID - [{ID}]";
        }
    }

    public enum AvatarPoseType
    {
        None,
        Head,
        Left,
        Right,
    }

    public enum AvatarWearType
    {
        None,
        Head,
        Left,
        Right,
    }

    public interface IAvatar
    {
        AvatarID AvatarID { get; }
        IAvatarController AvatarController { get; }
        Transform Root { get; }

        void SetAvatarController(IAvatarController controller);
        void SetWearablePack(IAvatarWearablePack pack);
        void SetWearable(IAvatarWearable wearable);

        Transform GetTransform(AvatarPoseType type);
        Pose GetPose(AvatarPoseType type);
        Pose GetLocalPose(AvatarPoseType type);
        void Destory();
    }

    public interface IAvatarWearable
    {
        AvatarWearType WearType { get; }
        void TargetTransform(Transform trans);
        void Unwear();
    }

    public interface IAvatarWearablePack
    {
        IAvatarWearable[] GetWearables();
    }

    public interface IAvatarController
    {
        Pose GetHeadPose();
        Pose GetHandPose(AvatarPoseType type);
    }

    public interface IAvatarService
    {
        IAvatar Create();
        void Remove(AvatarID id);
        IAvatar Find(AvatarID id);
        IAvatar GetMain();
    }

    public interface IAvatarSystem
    {
        IAvatar CreateMain();
        IAvatar Create();
        IAvatar Create(AvatarID id);
        void Remove(AvatarID id);
        IAvatar Find(AvatarID id);
        IAvatar GetMain();
    }

    public interface IAvatarRepository
    {
        IAvatar Find(AvatarID id);
        IAvatar Create(AvatarID id);
        void Remove(AvatarID id);
    }
}

