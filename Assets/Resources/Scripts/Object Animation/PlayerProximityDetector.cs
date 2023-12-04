using System;
using UnityEngine;

public class ProximityDetector : MonoBehaviour
{
    public float detectionRadius = 7f;
    public LayerMask playerLayer;
    private bool isPlayerNearby = false;

    // References to the animation scripts
    private ObjectBounceAndSomersault _bounceAndSomersault;
    private ObjectRotation _rotation;
    
    // Animation state tracking
    private bool stoppedBounceAndSomersault = false;
    private bool stoppedRotation = false;

    private void Start()
    {
        // Initialize the references to the animation scripts
        _bounceAndSomersault = GetComponent<ObjectBounceAndSomersault>();
        _rotation = GetComponent<ObjectRotation>();
    }

    private void FixedUpdate()
    {
        bool playerInRange = Physics.CheckSphere(transform.position, detectionRadius, playerLayer);
        if (playerInRange && !isPlayerNearby)
        {
            isPlayerNearby = true;
            StopAnimations();
        }
        else if (!playerInRange && isPlayerNearby)
        {
            isPlayerNearby = false;
            StartAnimations();
        }
    }

    private void StopAnimations()
    {
        if (_bounceAndSomersault.IsAnimationRunning())
        {
            _bounceAndSomersault.StopAnimation();
            stoppedBounceAndSomersault = true;
        }
        if (_rotation.IsAnimationRunning())
        {
            _rotation.StopAnimation();
            stoppedRotation = true;
        }
    }

    private void StartAnimations()
    {
        if (stoppedBounceAndSomersault && !_bounceAndSomersault.IsAnimationRunning())
        {
            _bounceAndSomersault.StartAnimation();
            stoppedBounceAndSomersault = false;
        }
        if (stoppedRotation && !_rotation.IsAnimationRunning())
        {
            _rotation.StartAnimation();
            stoppedRotation = false;
        }
    }

    // Visualize the detection radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
