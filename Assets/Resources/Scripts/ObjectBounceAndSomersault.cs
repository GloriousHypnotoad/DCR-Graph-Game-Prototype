using UnityEngine;
using System.Collections;

public class ObjectBounceAndSomersault : MonoBehaviour
{
    public float jumpHeight = 5f;
    public float jumpDuration = 1f;
    public float waitTime = 1f;
    private Vector3 startPosition;
    private Vector3 randomRotationAxis;

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(JumpAndDualRotateRoutine());
    }

    IEnumerator JumpAndDualRotateRoutine()
    {
        while (true)
        {
            // Determine random rotation axis for both somersaults
            Vector3 randomRotationAxis1 = new Vector3(Random.value, Random.value, Random.value).normalized;
            Vector3 randomRotationAxis2 = new Vector3(Random.value, Random.value, Random.value).normalized;

            // Jump and Rotate with Realistic Movement
            float timer = 0;
            while (timer <= jumpDuration)
            {
                // Vertical movement with parabolic trajectory
                float height = jumpHeight * (1 - Mathf.Pow(2 * timer / jumpDuration - 1, 2));
                transform.position = startPosition + new Vector3(0, height, 0);

                // Rotation (divided into two parts: up and down)
                if (timer <= jumpDuration / 2)
                    transform.Rotate(randomRotationAxis1, 360 * (Time.deltaTime / (jumpDuration / 2)), Space.Self);
                else
                    transform.Rotate(randomRotationAxis2, 360 * (Time.deltaTime / (jumpDuration / 2)), Space.Self);

                timer += Time.deltaTime;
                yield return null;
            }

            // Reset rotation, position and wait
            transform.rotation = Quaternion.identity;
            transform.position = startPosition;
            yield return new WaitForSeconds(waitTime);
        }
    }
}