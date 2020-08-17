using System;
using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.HandGrabSystemUseCase.Domain;
using UnityEngine;

namespace Conekton.ARUtility.Demo
{
    public class GrabbableTest : MonoBehaviour
    {
        private IGrabbable _grabbable = null;

        private void Awake()
        {
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
    }
}
