using System.Collections;
using UnityEngine;

public class ObjectRotationContinuous : MonoBehaviour
{
    public float rotationSpeed = 25.0f;
    private float originalSpeed;
    public float quickRotationSpeed = 900.0f;
    public int numberOfQuickRotations = 3;

    // Start method removed to enable immediate rotation

    void Update()
    {
        // Object now rotates continuously around the global Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    public void PerformQuickRotation()
    {
        StartCoroutine(QuickRotationCoroutine());
    }

    private IEnumerator QuickRotationCoroutine()
    {
        float previousSpeed = rotationSpeed; // Store the current speed before quick rotation
        rotationSpeed = quickRotationSpeed;

        // Calculate the duration of quick rotation
        float quickRotationDuration = numberOfQuickRotations * 360 / quickRotationSpeed;
        yield return new WaitForSeconds(quickRotationDuration);

        rotationSpeed = previousSpeed; // Revert to the original speed
    }
}