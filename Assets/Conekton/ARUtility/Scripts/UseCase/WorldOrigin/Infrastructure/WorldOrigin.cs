﻿using System;
using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.UseCase.WorldOrigin.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARUtility.UseCase.WorldOrigin.Infrastructure
{
    public class WorldOrigin : MonoBehaviour, IWorldOrigin
    {
        public event MovedEvent OnMoved;
        
        public Transform Transform => transform;
        
        private List<IWorldAnchor> _worldAnchors = new List<IWorldAnchor>();

        private void Update()
        {
            UpdatePose();
        }

        public void AddAnchor(IWorldAnchor anchor)
        {
            if (_worldAnchors.Contains(anchor))
            {
                return;
            }
            
            _worldAnchors.Add(anchor);
            
            UpdatePose();
        }

        public void Clear()
        {
            _worldAnchors.Clear();
        }

        private void UpdatePose()
        {
            if (_worldAnchors.Count == 0)
            {
                return;
            }
            
            Pose pose = GetAveragePose();
            
            transform.SetPositionAndRotation(pose.position, pose.rotation);
            
            OnMoved?.Invoke(this);
        }

        private Pose GetAveragePose()
        {
            if (_worldAnchors.Count == 0)
            {
                return default;
            }
            
            Vector3 resultPos = Vector3.zero;
            Quaternion firstRot = _worldAnchors[0].RelativePose.rotation;
            float x = 0, y = 0, z = 0, w = 0;
            
            foreach (var a in _worldAnchors)
            {
                resultPos += a.RelativePose.position;

                float dot = Quaternion.Dot(firstRot, a.RelativePose.rotation);
                float multi = dot > 0 ? 1f : -1f;

                x += a.RelativePose.rotation.x * multi;
                y += a.RelativePose.rotation.y * multi;
                z += a.RelativePose.rotation.z * multi;
                w += a.RelativePose.rotation.w * multi;
            }

            resultPos /= _worldAnchors.Count;

            float k = 1f / Mathf.Sqrt(x * x + y * y + z * z + w * w);
            Quaternion resultRot = new Quaternion(x * k, y * k, z * k, w * k);
            
            return new Pose(resultPos, resultRot);
        }
    }
}