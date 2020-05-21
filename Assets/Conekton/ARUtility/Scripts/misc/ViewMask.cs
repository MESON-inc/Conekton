using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewMask : MonoBehaviour
{
    [SerializeField] private Material _material = null;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _material);
    }
}
