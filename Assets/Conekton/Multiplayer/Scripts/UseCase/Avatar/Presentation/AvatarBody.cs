using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARMultiplayer.Avatar.Domain;
using Conekton.ARMultiplayer.AvatarBody.Domain;

namespace Conekton.ARMultiplayer.AvatarBody.Presentation
{
    public class AvatarBody : MonoBehaviour, IAvatarBody
    {
        [SerializeField] private Transform _headTrans = null;
        [SerializeField] private Transform _leftHandTrans = null;
        [SerializeField] private Transform _rightHandTrans = null;
        
        public event AvatarBodyFreeEvent OnAvatarBodyFree;
        
        public AvatarBodyType BodyType => _bodyType;
        public AvatarBodyID BodyID => _bodyID;
        public Transform Transform => transform;

        private AvatarBodyType _bodyType = AvatarBodyType.A;
        private AvatarBodyID _bodyID = default;
        private IAvatar _avatar = null;
        private bool _hasAvatar = false;

        private void Update()
        {
            UpdatePose();
        }

        private void UpdatePose()
        {
            if (!_hasAvatar)
            {
                return;
            }

            Pose rootPose = _avatar.GetRootPose();
            transform.SetPositionAndRotation(rootPose.position, rootPose.rotation);

            Pose headPose = _avatar.GetPose(AvatarPoseType.Head);
            _headTrans.SetPositionAndRotation(headPose.position, headPose.rotation);
            
            Pose leftPose = _avatar.GetPose(AvatarPoseType.Left);
            _leftHandTrans.SetPositionAndRotation(leftPose.position, leftPose.rotation);
            
            Pose rightPose = _avatar.GetPose(AvatarPoseType.Right);
            _rightHandTrans.SetPositionAndRotation(rightPose.position, rightPose.rotation);
        }

        [Inject]
        public void Construct(AvatarBodyTypeArgs args)
        {
            _bodyType = args.BodyType;
            _bodyID = args.BodyID;
        }

        public void Active(bool active)
        {
            gameObject.SetActive(active);
        }

        public void SetAvatar(IAvatar avatar)
        {
            if (_avatar != null)
            {
                _avatar.OnDestroyingAvatar -= HandleAvatarOnDestroying;
            }
            
            _avatar = avatar;

            if (_avatar != null)
            {
                _avatar.OnDestroyingAvatar += HandleAvatarOnDestroying;
            }
            
            _hasAvatar = avatar != null;
        }

        private void HandleAvatarOnDestroying(IAvatar avatar)
        {
            SetAvatar(null);
            OnAvatarBodyFree?.Invoke(this);
        }
    }
}

