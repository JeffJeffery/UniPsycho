using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    public float rotationAngle = 20f;
    public float rotationSpeed = 1f;
    public bool useEasing = true; 
    public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f); 

    void Update()
    {
        float time = Time.time * rotationSpeed;

        if (useEasing)
        {
            float easedTime = easeCurve.Evaluate(Mathf.PingPong(time, 2f) / 2f); 
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(-rotationAngle, rotationAngle, easedTime));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.PingPong(time, rotationAngle * 2) - rotationAngle);
        }
    }
}