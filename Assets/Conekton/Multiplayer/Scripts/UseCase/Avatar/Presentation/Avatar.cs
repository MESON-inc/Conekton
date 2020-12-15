using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;

namespace Conekton.ARMultiplayer.Avatar.Presentation
{
    public class Avatar : MonoBehaviour, IAvatar
    {
        [SerializeField] private Transform _headTrans = null;
        [SerializeField] private Transform _leftHandTrans = null;
        [SerializeField] private Transform _rightHandTrans = null;
        
        public event DestroyingAvatarEvent OnDestroyingAvatar;

        private AvatarID _avatarID = AvatarID.NoSet;

        private IAvatarController _avatarController = null;
        private bool _hasAvatarController = false;

        [Inject]
        public void Construct(AvatarID id)
        {
            _avatarID = id;

            gameObject.name = $"Avatar-{id.ID}";
        }

        private void Update()
        {
            if (_hasAvatarController)
            {
                UpdatePose();
            }
        }

        private void OnDestroy()
        {
            OnDestroyingAvatar?.Invoke(this);
        }

        private void UpdatePose()
        {
            Pose headPose = _avatarController.GetHeadPose();
            _headTrans.transform.localPosition = headPose.position;
            _headTrans.transform.localRotation = headPose.rotation;

            Pose leftPose = _avatarController.GetHandPose(AvatarPoseType.Left);
            GetTransform(AvatarPoseType.Left).transform.localPosition = leftPose.position;
            GetTransform(AvatarPoseType.Left).transform.localRotation = leftPose.rotation;

            Pose rightPose = _avatarController.GetHandPose(AvatarPoseType.Right);
            GetTransform(AvatarPoseType.Right).transform.localPosition = rightPose.position;
            GetTransform(AvatarPoseType.Right).transform.localRotation = rightPose.rotation;
        }

        #region ### for IAvatar interface ###
        AvatarID IAvatar.AvatarID => _avatarID;

        IAvatarController IAvatar.AvatarController => _avatarController;

        Transform IAvatar.Root => transform;

        Transform IAvatar.GetTransform(AvatarPoseType type) => GetTransform(type);

        void IAvatar.Destory()
        {
            if (gameObject == null)
            {
                return;
            }
            
            Debug.Log($"Will destory avatar {name}");
            Destroy(gameObject);
        }

        Pose IAvatar.GetPose(AvatarPoseType type)
        {
            return new Pose
            {
                position = GetTransform(type).position,
                rotation = GetTransform(type).rotation,
            };
        }

        Pose IAvatar.GetLocalPose(AvatarPoseType type)
        {
            return new Pose
            {
                position = GetTransform(type).localPosition,
                rotation = GetTransform(type).localRotation,
            };
        }

        void IAvatar.SetAvatarController(IAvatarController controller)
        {
            _hasAvatarController = controller != null;
            _avatarController = controller;
        }

        void IAvatar.SetWearablePack(IAvatarWearablePack pack)
        {
            WearPack(pack);
        }

        void IAvatar.SetWearable(IAvatarWearable wearable)
        {
            Wear(wearable);
        }
        #endregion ### for IAvatar interface ###

        private Transform GetTransform(AvatarPoseType type)
        {
            switch (type)
            {
                case AvatarPoseType.Head:
                    return _headTrans;

                case AvatarPoseType.Left:
                    return _leftHandTrans;

                case AvatarPoseType.Right:
                    return _rightHandTrans;
            }

            return null;
        }

        private void WearPack(IAvatarWearablePack pack)
        {
            foreach (var wearable in pack.GetWearables())
            {
                Wear(wearable);
            }
        }

        private void Wear(IAvatarWearable wearable)
        {
            Transform target = GetTransformByWearType(wearable.WearType);

            if (target != null)
            {
                wearable.TargetTransform(target);
            }
        }

        private Transform GetTransformByWearType(AvatarWearType type)
        {
            switch (type)
            {
                case AvatarWearType.Head:
                    return _headTrans;

                case AvatarWearType.Left:
                    return _leftHandTrans;

                case AvatarWearType.Right:
                    return _rightHandTrans;
            }

            return null;
        }
    }
}

