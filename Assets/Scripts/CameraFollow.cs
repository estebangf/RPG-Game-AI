using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 offset = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private float zPosition = -10f;
    void Start()
    {
        zPosition = transform.position.z;
    }

    void Update()
    {
        Vector3 targetPosition = target.position + offset;
        targetPosition.z = zPosition;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        
    }
}
