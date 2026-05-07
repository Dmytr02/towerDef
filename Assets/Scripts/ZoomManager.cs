using System;
using UnityEngine;

public class ZoomManager : MonoBehaviour
{
    public Camera cam;

    private void Start()
    {
        cam.enabled = false;
    }

    public void RenderZoom()
    {
        cam.Render();
    }
}
