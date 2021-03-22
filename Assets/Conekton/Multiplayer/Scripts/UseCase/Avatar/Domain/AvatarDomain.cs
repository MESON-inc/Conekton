using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conekton.ARMultiplayer.Avatar.Domain
{
    /// <summary>
    /// This event will invoke when an avatar is being destroyed.
    /// </summary>
    /// <param name="avatar"></param>
    public delegate void DestroyingAvatarEvent(IAvatar avatar);
    
    /// <summary>
    /// AvatarID is a data structure.
    /// 
    /// This struct will be used matching each Avatar object.
    /// </summary>
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

    /// <summary>
    /// IAvatar provides ways to access an avatar pose.
    /// </summary>
    public interface IAvatar
    {
        event DestroyingAvatarEvent OnDestroyingAvatar;
        AvatarID AvatarID { get; }
        IAvatarController AvatarController { get; }
        Transform Root { get; }

        void SetAvatarController(IAvatarController controller);
        void SetWearablePack(IAvatarWearablePack pack);
        void SetWearable(IAvatarWearable wearable);

        Transform GetTransform(AvatarPoseType type);
        Pose GetRootPose();
        Pose GetPose(AvatarPoseType type);
        Pose GetLocalPose(AvatarPoseType type);
        void Destory();
    }

    /// <summary>
    /// IAvatarWearable provides ways to equipe some items to the avatar.
    /// </summary>
    public interface IAvatarWearable
    {
        AvatarWearType WearType { get; }
        void TargetTransform(Transform trans);
        void Unwear();
    }

    /// <summary>
    /// IAvatarWearablePack is just a pack of IAvatarWearable.
    /// </summary>
    public interface IAvatarWearablePack
    {
        IAvatarWearable[] GetWearables();
    }

    /// <summary>
    /// IAvatarController provides ways to control an avatar object.
    /// </summary>
    public interface IAvatarController
    {
        Pose GetRootPose();
        Pose GetHeadPose();
        Pose GetHandPose(AvatarPoseType type);
    }

    /// <summary>
    /// IAvatarService seems like an end point from a client.
    /// </summary>
    public interface IAvatarService
    {
        IAvatar Create();
        void Remove(AvatarID id);
        IAvatar Find(AvatarID id);
        IAvatar GetMain();
    }

    /// <summary>
    /// IAvatarSystem is the system.
    /// 
    /// The class that implement this interface will provide ways about IAvatar control.
    /// </summary>
    public interface IAvatarSystem
    {
        IAvatar CreateMain();
        IAvatar Create();
        IAvatar Create(AvatarID id);
        void Remove(AvatarID id);
        IAvatar Find(AvatarID id);
        IAvatar GetMain();
    }

    /// <summary>
    /// IAvatarRepository is a repository.
    /// 
    /// The class that implement this interface will store all IAvatar objects.
    /// </summary>
    public interface IAvatarRepository
    {
        IAvatar Find(AvatarID id);
        IAvatar Create(AvatarID id);
        void Remove(AvatarID id);
    }
}

