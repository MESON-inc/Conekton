using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if PLATFORM_LUMIN
using UnityEngine.XR.MagicLeap;
#endif

using Conekton.ARUtility.Player.Domain;

namespace Conekton.ARUtility.Player.Infrastructure
{
    public class MLPlayer : MonoBehaviour
#if PLATFORM_LUMIN
        , IPlayer
#endif
    {
        [SerializeField] private GameObject _cameraRig = null;
        [SerializeField] private Camera _camera = null;

#if PLATFORM_LUMIN
        private Transform Root => transform;

        Transform IPlayer.Root => Root;
        GameObject IPlayer.CameraRig => _cameraRig;
        Camera IPlayer.MainCamera => _camera;
        Vector3 IPlayer.Position => _cameraRig.transform.position;
        Vector3 IPlayer.Forward => _cameraRig.transform.forward;
        Quaternion IPlayer.Rotation => _cameraRig.transform.rotation;
        Pose IPlayer.GetHumanPose(HumanPoseType type) => GetHumanPose(type);
        Pose IPlayer.GetHumanLocalPose(HumanPoseType type) => GetHumanLocalPose(type);
        bool IPlayer.IsActiveHumanPose(HumanPoseType type) => IsActiveHumanPose(type);

        private bool _hasInitialized = false;

        private bool _canUseHands = false;

        private bool CanUseHand => MLHandTracking.IsStarted && _canUseHands;

        #region ### MonoBehaviour ###
        private void Start()
        {
            InitializeMLHandTrackingIfNeeded();
        }

        private void Update()
        {
            if (!_hasInitialized)
            {
                InitializeMLHandTrackingIfNeeded();
            }
        }

        private void OnDestroy()
        {
            MLHandTracking.Stop();
        }
        #endregion ### MonoBehaviour ###

        private void InitializeMLHandTrackingIfNeeded()
        {
            if (_hasInitialized)
            {
                return;
            }

            if (!MLHandTracking.IsStarted)
            {
                return;
            }

            MLResult result = MLHandTracking.Start();

            if (result.IsOk)
            {
                _canUseHands = true;
                _hasInitialized = true;

                Debug.Log("<<<< Sccessed to initialize MLHandTracking feature >>>>");
            }
            else
            {
                Debug.LogError("MLHands won't start.");
            }
        }

        private Pose GetHumanPose(HumanPoseType type)
        {
            switch (type)
            {
                case HumanPoseType.Head:
                    return new Pose(_cameraRig.transform.position, _cameraRig.transform.rotation);

                case HumanPoseType.LeftHand:
                    return new Pose(GetHandPosition(HumanPoseType.LeftHand), GetHandRotation(HumanPoseType.LeftHand));

                case HumanPoseType.RightHand:
                    return new Pose(GetHandPosition(HumanPoseType.RightHand), GetHandRotation(HumanPoseType.RightHand));
            }

            return default;
        }

        private Pose GetHumanLocalPose(HumanPoseType type)
        {
            switch (type)
            {
                case HumanPoseType.Head:
                    return new Pose(_cameraRig.transform.localPosition, _cameraRig.transform.localRotation);

                case HumanPoseType.LeftHand:
                    return new Pose(GetLocalPosition(GetHandPosition(HumanPoseType.LeftHand)), Quaternion.identity);

                case HumanPoseType.RightHand:
                    return new Pose(GetLocalPosition(GetHandPosition(HumanPoseType.RightHand)), Quaternion.identity);
            }

            return default;
        }

        private Vector3 GetLocalPosition(Vector3 worldPos)
        {
            return Root.worldToLocalMatrix.MultiplyPoint3x4(worldPos);
        }

        private Vector3 GetHandPosition(HumanPoseType type)
        {
            if (!CanUseHand)
            {
                return Vector3.zero;
            }

            return GetHandByType(type).Middle.MCP.Position;
        }

        private Quaternion GetHandRotation(HumanPoseType type)
        {
            if (!CanUseHand)
            {
                return Quaternion.identity;
            }

            MLHandTracking.Hand hand = GetHandByType(type);

            if (!hand.IsVisible)
            {
                return Quaternion.identity;
            }

            if (!hand.Wrist.Center.IsSupported)
            {
                return Quaternion.identity;
            }

            Vector3 dir = hand.Center - hand.Wrist.Center.Position;

            return Quaternion.LookRotation(dir);
        }

        private MLHandTracking.Hand GetHandByType(HumanPoseType type)
        {
            switch (type)
            {
                case HumanPoseType.LeftHand:
                    return MLHandTracking.Left;

                case HumanPoseType.RightHand:
                    return MLHandTracking.Right;
            }

            return null;
        }

        private bool IsActiveHumanPose(HumanPoseType type)
        {
            switch (type)
            {
                case HumanPoseType.Head:
                    return true;

                case HumanPoseType.LeftHand:
                    if (!CanUseHand || MLHandTracking.Left == null)
                    {
                        return false;
                    }
                    return MLHandTracking.Left.IsVisible;

                case HumanPoseType.RightHand:
                    if (!CanUseHand || MLHandTracking.Right == null)
                    {
                        return false;
                    }
                    return MLHandTracking.Right.IsVisible;
            }

            return false;
        }
#endif
    }
}

