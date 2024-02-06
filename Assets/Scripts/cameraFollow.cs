using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;

    public float yOffset = 3f;

    void Update()
    {
        if (target != null)
        {

            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y + yOffset, transform.position.z);

            transform.position = Vector3.Slerp(transform.position, desiredPosition, smoothSpeed);
        }
    }
}
