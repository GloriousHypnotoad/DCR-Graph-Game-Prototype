using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBobbing : MonoBehaviour
{
    public float amplitude = 0.25f;
    public float frequency = 0.75f;

    private Vector3 startPos;
    private float tempVal;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        tempVal = startPos.y + amplitude * Mathf.Sin(Time.time * frequency);
        transform.position = new Vector3(startPos.x, tempVal, startPos.z);
    }
}
