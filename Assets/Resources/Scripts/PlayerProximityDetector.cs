using System;
using UnityEngine;

public class ProximityDetector : MonoBehaviour
{
    public float detectionRadius = 5f;
    public LayerMask playerLayer;
    public event Action OnPlayerProximity;

    private void FixedUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player within radius!");
                 OnPlayerProximity?.Invoke();
            }
        }
    }

    // Visualize the detection radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}