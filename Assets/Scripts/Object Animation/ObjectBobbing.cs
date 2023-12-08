using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBobbing : MonoBehaviour
{
    public float amplitude = 0.25f;
    public float frequency = 0.75f;

    private Vector3 startPos;
    private float tempVal;
    private bool isBobbing = false; // Boolean flag to control bobbing

    void Start()
    {
        startPos = transform.position;
        isBobbing = true;
    }

    void Update()
    {
        if (isBobbing) // Only bob if isBobbing is true
        {
            tempVal = startPos.y + amplitude * Mathf.Sin(Time.time * frequency);
            transform.position = new Vector3(startPos.x, tempVal, startPos.z);
        }
    }

    // Method to start bobbing
    public void StartAnimation()
    {
        isBobbing = true;
    }

    // Method to stop bobbing
    public void StopAnimation()
    {
        isBobbing = false;
    }

    // Method to check if the animation is running
    public bool IsAnimationRunning()
    {
        return isBobbing;
    }
}