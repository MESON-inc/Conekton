using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

using Conekton.ARUtility.EventSystem.Application;
using Conekton.ARUtility.Player.Domain;
using Conekton.ARUtility.Input.Domain;
using Conekton.ARUtility.Input.Application;

namespace Conekton.ARUtility.Input.Presenter
{
    public class InputControllerView : MonoBehaviour
    {
        [Inject] private IInputController _inputController = null;
        [Inject] private IPlayer _player = null;

        [SerializeField] private float _laserDistance = 1f;
        [SerializeField] private Transform _reticle = null;

        private LineRenderer _lineRenderer = null;
        private Vector3[] _positions = new Vector3[2];
        private RaycastHit[] _resultsCache = new RaycastHit[100];

        private readonly Vector3 VECTOR3_FORWARD = Vector3.forward;

        private Vector3 ControllerPosition => _inputController.Position;
        private Vector3 ControllerForward => _inputController.Rotation * VECTOR3_FORWARD;

        #region ### MonoBehaviour ###
        private void Awake()
        {
            _lineRenderer = GetComponentInChildren<LineRenderer>();
        }

        private void Update()
        {
            UpdatePose();
            //CheckInput();
        }
        #endregion ### MonoBehaviour ###

        private void CheckInput()
        {
            if (_inputController.IsTriggerDown)
            {
                Debug.Log("Trigger Down");
            }

            if (_inputController.IsTriggerUp)
            {
                Debug.Log("Trigger Up");
            }

            if (_inputController.IsTouchDown)
            {
                Debug.Log("Touch Down");
            }

            if (_inputController.IsTouchUp)
            {
                Debug.Log("Touch Up");
            }

            if (_inputController.IsTouch)
            {
                Debug.Log("Touch");
            }
        }

        private void UpdatePose()
        {
            transform.SetPositionAndRotation(ControllerPosition, _inputController.Rotation);

            float dist = _laserDistance;

            if (TryLaserCollision(out float hitDistance))
            {
                dist = hitDistance;
            }

            _positions[0] = ControllerPosition;
            _positions[1] = _positions[0] + ControllerForward * dist;

            _lineRenderer.SetPositions(_positions);

            _reticle.position = _positions[1];
        }

        private bool TryLaserCollision(out float hitDistance)
        {
            hitDistance = float.MaxValue;

            float objDist = ObjectCollisionDistance();
            float uiDist = UICollisionDistance();
            float dist = Mathf.Min(objDist, uiDist);

            if (dist == float.MaxValue)
            {
                return false;
            }

            hitDistance = dist;

            return true;
        }

        private Ray GetRayByController()
        {
            return new Ray(ControllerPosition, ControllerForward);
        }

        private float ObjectCollisionDistance()
        {
            Ray ray = GetRayByController();
            int count = Physics.RaycastNonAlloc(ray, _resultsCache);

            if (count == 0)
            {
                return float.MaxValue;
            }

            float minDistance = float.MaxValue;

            for (int i = 0; i < count; i ++)
            {
                if (minDistance > _resultsCache[i].distance)
                {
                    minDistance = _resultsCache[i].distance;
                }
            }

            return minDistance;
        }

        private float UICollisionDistance()
        {
            Ray ray = GetRayByController();

            float minDistance = float.MaxValue;

            foreach (var canvas in ARRaycaster.AllCanvases)
            {
                if (canvas == null)
                {
                    continue;
                }

                if (UIRaycastHelper.TryGetNearUI(canvas, _player.MainCamera, ray, out Graphic graphic, out float outDistance))
                {
                    if (outDistance < minDistance)
                    {
                        minDistance = outDistance;
                    }
                }
            }

            return minDistance;
        }
    }
}

