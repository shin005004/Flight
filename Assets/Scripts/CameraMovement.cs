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

        pos.x = cameraHolder.position.x;
        pos.y = cameraHolder.position.y + height;
        pos.z = cameraHolder.position.z + zett;

        cameraPlace.position = Vector3.SmoothDamp(cameraPlace.position, pos, ref velocity, smooth);
    }
}