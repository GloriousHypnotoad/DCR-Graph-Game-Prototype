
using UnityEngine;
using System;
using System.Collections;

public class ObjectJumpAndSpin : MonoBehaviour
{
    public float jumpHeight = 50f;
    public float jumpDuration = 2f;
    public float waitTime = 2f;
    private Vector3 startPosition;
    private Coroutine jumpCoroutine;
    private bool isAnimationRunning = false;
    private bool stopRequested = false;
    private event Action _onJumpStart;
    private event Action _onJumpApex;

    void Start() 
    {
        startPosition = transform.position;
    }

    // Start the animation
    public void StartAnimation()
    {
        if (!isAnimationRunning)
        {
            jumpCoroutine = StartCoroutine(JumpAndRotateRoutine());
            isAnimationRunning = true;
        }
    }

    // Stop the animation
    public void StopAnimation()
    {
        if (isAnimationRunning)
        {
            stopRequested = true;
        }
    }

    // Check if the animation is running
    public bool IsAnimationRunning()
    {
        return isAnimationRunning;
    }

    IEnumerator JumpAndRotateRoutine()
    {
        while (true)
        {
            _onJumpStart?.Invoke();
            // Jump and Rotate with Realistic Movement
            float timer = 0;
            bool apexReached = false;
            while (timer <= jumpDuration)
            {
                // Vertical movement with parabolic trajectory
                float height = jumpHeight * (1 - Mathf.Pow(2 * timer / jumpDuration - 1, 2));
                transform.position = startPosition + new Vector3(0, height, 0);

                // Faster Rotation around Y-axis
                transform.Rotate(0, 900.0f * (Time.deltaTime / jumpDuration), 0, Space.Self);
                
                if (!apexReached && timer >= jumpDuration / 2)
                {
                    // Invoke the _onJumpApex event at the apex of the jump
                    _onJumpApex?.Invoke();
                    apexReached = true;
                }
                timer += Time.deltaTime;
                yield return null;
            }

            // Reset to start position and rotation
            ResetPositionAndRotation();

            if (stopRequested)
            {
                stopRequested = false;
                isAnimationRunning = false;
                ResetPositionAndRotation();
                yield break; // Exit the coroutine
            }

            // Wait before the next jump
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void ResetPositionAndRotation()
    {
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
    }
}