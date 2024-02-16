using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public float moveDistance = 2f;
    public float moveSpeed = 1f;
    public float moveAngle = 0f;
    public float timeScale = 1f;
    public float startOffset = 0f;
    public bool useEasing = true;
    public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    private Vector3 startPos;
    private float currentTime = 0f;

    void Start()
    {
        startPos = transform.position;
        currentTime = startOffset;
    }

    void Update()
    {
        currentTime += Time.deltaTime * timeScale;
        float time;
        if (useEasing)
        {
            time = easeCurve.Evaluate(Mathf.PingPong(currentTime, 3f) / 3f);
        }
        else
        {
            time = Mathf.PingPong(currentTime, 3f) / 3f;
        }
        float radians = moveAngle * Mathf.Deg2Rad;
        float xPos = Mathf.Lerp(-moveDistance, moveDistance, time) * Mathf.Cos(radians); 
        float yPos = Mathf.Lerp(-moveDistance, moveDistance, time) * Mathf.Sin(radians);
        transform.position = startPos + new Vector3(xPos, yPos, 0f);
    }
}