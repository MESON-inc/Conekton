using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Conekton.ARUtility.HandSystemUseCase.Application
{
    public class OculusHandPalm : MonoBehaviour
    {
#if UNITY_ANDROID && PLATFORM_OCULUS
        private OVRHand _hand = null;
        private OVRSkeleton _skeleton = null;

        private OVRHand Hand => _hand ?? (_hand = GetComponent<OVRHand>());
        private OVRSkeleton Skeleton => _skeleton ?? (_skeleton = GetComponent<OVRSkeleton>());

        private OVRSkeleton.BoneId[] _forPalmCalcTargetList = new[]
        {
            // First two of them are used for calculating palm normal.
            OVRSkeleton.BoneId.Hand_Index1,
            OVRSkeleton.BoneId.Hand_Pinky0,

            OVRSkeleton.BoneId.Hand_Middle1,
            OVRSkeleton.BoneId.Hand_Ring1,
            OVRSkeleton.BoneId.Hand_Pinky1,
            OVRSkeleton.BoneId.Hand_Thumb0,
        };

        private OVRSkeleton.BoneId BoneIDForNormalCalculation1 => OVRSkeleton.BoneId.Hand_Index1;
        private OVRSkeleton.BoneId BoneIDForNormalCalculation2 => OVRSkeleton.BoneId.Hand_Pinky0;

        public bool TryGetPositionAndNormal(out Vector3 position, out Vector3 normal)
        {
            if (!Hand.IsTracked)
            {
                position = Vector3.zero;
                normal = Vector3.up;
                return false;
            }

            Vector3 center = Vector3.zero;

            foreach (var id in _forPalmCalcTargetList)
            {
                OVRBone bone = GetBoneById(id);
                center += bone.Transform.position;
            }

            center /= _forPalmCalcTargetList.Length;

            position = center;

            OVRBone bone0 = GetBoneById(BoneIDForNormalCalculation1);
            OVRBone bone1 = GetBoneById(BoneIDForNormalCalculation2);

            Vector3 edge0 = bone0.Transform.position - center;
            Vector3 edge1 = bone1.Transform.position - center;

            normal = Vector3.Cross(edge0, edge1).normalized;

            return true;
        }

        private OVRBone GetBoneById(OVRSkeleton.BoneId id)
        {
            return Skeleton.Bones[(int)id];
        }
#endif
    }
}
