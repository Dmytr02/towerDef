using System;
using UnityEngine;
using UnityEngine.UI;

public class ZoomManager : MonoBehaviour
{
    public Camera cam;
    public RawImage img;
    public static ZoomManager instance;
    public float distance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        cam.enabled = false;
    }

    public void RenderZoom(Vector3 pos)
    {
        cam.transform.position = pos;
        cam.transform.rotation = Camera.main.transform.rotation;
        cam.transform.position -= cam.transform.forward*distance*SceneGenerator.m_transform.lossyScale.x;
        cam.Render();
    }
}
