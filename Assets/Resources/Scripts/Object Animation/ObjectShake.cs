using UnityEngine;
using System.Collections;

public class ObjectShake : MonoBehaviour
{
    // Duration and intensity of shake
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.1f;

    private Vector3 originalPosition;
    private bool isShaking = false;
    public void StartShake()
    {
        if (!isShaking)
        {
            originalPosition = transform.position;
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        isShaking = true;
        float endTime = Time.time + shakeDuration;

        while (Time.time < endTime)
        {
            float x = Random.Range(-shakeIntensity, shakeIntensity) + originalPosition.x;
            float y = Random.Range(-shakeIntensity, shakeIntensity) + originalPosition.y;
            float z = Random.Range(-shakeIntensity, shakeIntensity) + originalPosition.z;

            transform.position = new Vector3(x, y, z);
            yield return null;
        }

        transform.position = originalPosition;
        isShaking = false;
    }
}