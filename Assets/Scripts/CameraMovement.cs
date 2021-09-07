using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraHolder;
    public Transform cameraPlace;
    public float smooth = 0.3f;

    public float height = 10f;
    public float zett = 0f;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        
    }
    
    void FixedUpdate()
    {
        Vector3 pos = new Vector3();

        pos.x = 0f;
        pos.y = height;
        pos.z = zett;

        cameraPlace.localPosition = Vector3.SmoothDamp(cameraPlace.localPosition, pos, ref velocity, smooth);
    }
}