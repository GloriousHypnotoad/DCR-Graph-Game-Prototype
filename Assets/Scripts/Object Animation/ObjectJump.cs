using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;

public class ObjectJump : MonoBehaviour, IAnimatable
{
    private float _jumpHeight = 1f;
    private float _jumpDuration = 1f;
    private float _waitTime = 2f;
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

    public void ToggleAnimation(bool isAnimated)
    {
        if (isAnimated && !isAnimationRunning)
        {
            StartAnimation();
        }
        else if (!isAnimated && isAnimationRunning)
        {
            StopAnimation();
        }
    }

    // Start the animation
    private void StartAnimation()
    {
        jumpCoroutine = StartCoroutine(JumpRoutine());
        isAnimationRunning = true;
    }

    // Stop the animation
    private void StopAnimation()
    {
        stopRequested = true;
    }

    IEnumerator JumpRoutine()
    {
        while (true)
        {
            _onJumpStart?.Invoke();
            float timer = 0;
            bool apexReached = false;
            while (timer <= _jumpDuration)
            {
                // Vertical movement with parabolic trajectory
                float height = _jumpHeight * (1 - Mathf.Pow(2 * timer / _jumpDuration - 1, 2));
                transform.position = startPosition + new Vector3(0, height, 0);
                
                if (!apexReached && timer >= _jumpDuration / 2)
                {
                    _onJumpApex?.Invoke();
                    apexReached = true;
                }
                timer += Time.deltaTime;
                yield return null;
            }

            // Reset to start position
            ResetPosition();

            if (stopRequested)
            {
                stopRequested = false;
                isAnimationRunning = false;
                yield break; // Exit the coroutine
            }

            // Wait before the next jump
            yield return new WaitForSeconds(_waitTime);
        }
    }

    private void ResetPosition()
    {
        transform.position = startPosition;
    }
}
