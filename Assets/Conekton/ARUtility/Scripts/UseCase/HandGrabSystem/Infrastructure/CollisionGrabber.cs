using System;
using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.GrabSystemUseCase.Domain;
using UnityEngine;
using ModestTree;

namespace Conekton.ARUtility.GrabSystemUseCase.Infrastructure
{
    public class CollisionGrabber : Grabber
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IGrabbable grabbable))
            {
                TryTouch(grabbable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IGrabbable grabbable))
            {
                TryUntouch(grabbable);
            }
        }
    }
}