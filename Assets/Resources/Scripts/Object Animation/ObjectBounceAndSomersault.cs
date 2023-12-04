using UnityEngine;
using System.Collections;

public class ObjectBounceAndSomersault : MonoBehaviour
{
    public float jumpHeight = 30f;
    public float jumpDuration = 3f;
    public float waitTime = 3f;
    private Vector3 startPosition;
    private bool isJumping = false; // Boolean flag to control jumping
    private Coroutine jumpRoutine; // Reference to the coroutine
    private bool stopAfterCurrentCycle = false; // Flag to indicate stopping after the current cycle


    void Start()
    {
        startPosition = transform.position;
    }

    public void StartAnimation()
    {
        if (!isJumping)
        {
            isJumping = true;
            stopAfterCurrentCycle = false; // Reset the flag when animation starts
            jumpRoutine = StartCoroutine(JumpAndDualRotateRoutine());
        }
    }

    public void StopAnimation()
    {
        // Indicate that the animation should stop after the current cycle
        stopAfterCurrentCycle = true;
    }

    IEnumerator JumpAndDualRotateRoutine()
    {
        while (isJumping)
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

            // Reset rotation and position at the end of jump
            transform.rotation = Quaternion.identity;
            transform.position = startPosition;

            // Check if the animation should stop after the current cycle
            if (stopAfterCurrentCycle)
            {
                isJumping = false;
                yield break; // Exit the coroutine
            }

            // Wait for the specified time before next jump
            yield return new WaitForSeconds(waitTime);
        }
    }


    // Method to check if the animation is running
    public bool IsAnimationRunning()
    {
        return isJumping;
    }
}