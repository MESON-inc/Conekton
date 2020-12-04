using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Conekton.ARUtility.Application;
using Conekton.ARUtility.GrabSystemUseCase.Domain;

namespace Conekton.ARUtility.GrabSystemUseCase.Application
{
    public class GrabbableMover : MonoBehaviour
    {
        public event OnThrownEvent OnThrown;

        [SerializeField] private float _powerScale = 1f;

        private IGrabbable _grabbable = null;
        private IGrabber _targetGrabber = null;
        private Rigidbody _rigidbody = null;

        private Vector3 _offsetPos = Vector3.zero;
        private Quaternion _offsetRot = Quaternion.identity;
        private Quaternion _grabberRotInv = Quaternion.identity;

        private int _velocityDataIndex = 0;
        private Vector3[] _velocityData = new Vector3[10];

        private float _throwingTime = 0f;
        private bool _isThrowing = false;
        private bool _hasRigidbody = false;
        private Vector3 _lastVelocity = Vector3.zero;

        private bool _initKinematic = false;

        public bool IsThrowing => _isThrowing;
        public Vector3 LastVelocity => _lastVelocity;

        private Vector3 _prevPos = Vector3.zero;
        private float _throwVelocityThreshold = 0.2f;

        #region ### MonoBehaviour ###

        private void Awake()
        {
            _grabbable = GetComponent<IGrabbable>();

            if (TryGetComponent(out _rigidbody))
            {
                _hasRigidbody = true;
                _initKinematic = _rigidbody.isKinematic;
            }
        }

        #endregion ### MonoBehaviour ###

        public void Begin(IGrabber grabber)
        {
            _targetGrabber = grabber;

            Pose pose = _targetGrabber.GetPose();
            _prevPos = pose.position;
            _isThrowing = false;
            _lastVelocity = Vector3.zero;

            _offsetPos = transform.position - pose.position;
            _offsetRot = Quaternion.Inverse(pose.rotation) * transform.rotation;
            _grabberRotInv = Quaternion.Inverse(pose.rotation);

            if (_hasRigidbody)
            {
                _rigidbody.isKinematic = true;
            }
        }

        public void Move()
        {
            if (_targetGrabber == null)
            {
                return;
            }

            UpdateTransform();
            UpdateVelocity();
        }

        public void End()
        {
            Throwing();

            _targetGrabber = null;
        }

        /// <summary>
        /// Calculate ideal velocity from sampled data.
        /// </summary>
        private Vector3 GetVelocity()
        {
            // Sort the vector array because it's used a pool.
            // FrameCount indicate the index where the array is current to use.
            Vector3[] sortedData = new Vector3[_velocityData.Length];
            for (int i = 0; i < _velocityData.Length; i++)
            {
                sortedData[i] = _velocityData[(_velocityDataIndex + i + 1) % _velocityData.Length];
            }

            // Ignore zero vectors.
            sortedData = sortedData.Where(v => v != Vector3.zero).ToArray();

            if (sortedData.Length == 0)
            {
                return Vector3.zero;
            }

            // Ignore some vectors that indicate opposite by the last vector.
            Vector3 lastVector = sortedData.Last().normalized;
            sortedData = sortedData.Where(v => Vector3.Dot(v.normalized, lastVector) > 0).ToArray();

            if (sortedData.Length == 1)
            {
                return sortedData[0];
            }

            Vector3[] filterdData = Math.LowPassFilter(Math.FilterVelocity(sortedData));

            Vector3 velocity = Vector3.zero;
            if (filterdData.Length == 1)
            {
                velocity = filterdData[0];
            }
            else
            {
                float[] result = Math.LeastSquaresPlane(filterdData);
                float a = result[0];
                float b = result[1];
                float c = result[2];

                Vector3 v = filterdData.LastOrDefault();
                if (filterdData.Length >= 2)
                {
                    Vector3 delta = (filterdData.Last() - filterdData[filterdData.Length - 2]);
                    v += delta;
                }

                // a + bx + cy - z = 0
                float d = a + (b * v.x) + (c * v.z);
                velocity = new Vector3(v.x, d, v.z);
            }

            return velocity;
        }

        /// <summary>
        /// Throw the grabbable if needed.
        /// </summary>
        private void Throwing()
        {
            if (Time.time - _throwingTime < 0.1f)
            {
                return;
            }

            _throwingTime = Time.time;
            _isThrowing = true;

            if (_hasRigidbody)
            {
                Vector3 velocity = GetVelocity();

                _rigidbody.isKinematic = _initKinematic;
                _rigidbody.velocity = velocity * _powerScale;
                _lastVelocity = _rigidbody.velocity;

                if (velocity.sqrMagnitude >= _throwVelocityThreshold)
                {
                    OnThrown?.Invoke(_grabbable);
                }
            }

            _prevPos = Vector3.zero;
            _velocityData = new Vector3[_velocityData.Length];
        }

        private void UpdateVelocity()
        {
            Pose pose = _targetGrabber.GetPose();

            Vector3 velocity = (pose.position - _prevPos) / Time.deltaTime;
            if (velocity == Vector3.zero)
            {
                return;
            }

            _velocityDataIndex = Time.frameCount % _velocityData.Length;
            _velocityData[_velocityDataIndex] = velocity;

            _prevPos = pose.position;
        }

        private void UpdateTransform()
        {
            Pose pose = _targetGrabber.GetPose();

            Quaternion rot = pose.rotation * _offsetRot;
            Vector3 pos = pose.position + (pose.rotation * _grabberRotInv * _offsetPos);

            if (_hasRigidbody)
            {
                _rigidbody.MovePosition(pos);
                _rigidbody.MoveRotation(rot);
            }
            else
            {
                transform.SetPositionAndRotation(pos, rot);
            }
        }
    }
}