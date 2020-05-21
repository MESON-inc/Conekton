using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Conekton.ARUtility.Input.Application
{
    public class UIRaycastHelper
    {
        static public bool TryGetNearUI(Canvas canvas, Camera eventCamera, Ray ray, out Graphic targetGraphic, out float outDistance)
        {
            bool isHit = false;

            targetGraphic = null;
            outDistance = -1f;

            if (!canvas.enabled)
            {
                return false;
            }

            if (!canvas.gameObject.activeInHierarchy)
            {
                return false;
            }

            float minDistance = float.MaxValue;

            IList<Graphic> graphics = GraphicRegistry.GetGraphicsForCanvas(canvas);

            for (int i = 0; i < graphics.Count; i++)
            {
                Graphic graphic = graphics[i];

                if (graphic.depth == -1 || !graphic.raycastTarget)
                {
                    continue;
                }

                Transform graphicTransform = graphic.transform;
                Vector3 graphicForward = graphicTransform.forward;

                float dir = Vector3.Dot(graphicForward, ray.direction);

                // Return immediately if direction is negative.
                if (dir < 0)
                {
                    continue;
                }

                float distance = Vector3.Dot(graphicForward, graphicTransform.position - ray.origin) / dir;

                // If distance is far from min-distance will return.
                if (distance >= minDistance)
                {
                    continue;
                }

                Vector3 position = ray.GetPoint(distance);
                Vector2 pointerPosition = eventCamera.WorldToScreenPoint(position);

                // To continue if the graphic doesn't include the point.
                if (!RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, pointerPosition, eventCamera))
                {
                    continue;
                }

                // To continue if graphic raycast has failed.
                if (!graphic.Raycast(pointerPosition, eventCamera))
                {
                    continue;
                }

                isHit = true;
                minDistance = distance;
                targetGraphic = graphic;
                outDistance = distance;
            }

            return isHit;
        }
    }
}

