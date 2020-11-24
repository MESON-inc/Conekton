using System;
using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.GrabSystemUseCase.Domain;
using UnityEngine;

namespace Conekton.ARUtility.Demo
{
    public class GrabbableTest : MonoBehaviour
    {
        private IGrabbable _grabbable = null;
        
        private Vector3 _initPos = Vector3.zero;
        private Quaternion _initRot = Quaternion.identity;

        private void Awake()
        {
            _initPos = transform.position;
            _initRot = transform.rotation;
            
            var ren = GetComponent<Renderer>();
            var material = ren.material;
            
            _grabbable = GetComponent<IGrabbable>();
            _grabbable.OnTouched += (grabber, grabbable) =>
            {
                material.color = Color.red;
            };

            _grabbable.OnUntouched += (grabber, grabbable) =>
            {
                material.color = Color.white;
            };

            _grabbable.OnBeganGrab += (grabbere, grabbable) =>
            {
                material.color = Color.blue;
            };

            _grabbable.OnEndedGrab += (grabbere, grabbable) =>
            {
                material.color = Color.green;
            };
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                transform.SetPositionAndRotation(_initPos, _initRot);
            }
        }
    }
}
