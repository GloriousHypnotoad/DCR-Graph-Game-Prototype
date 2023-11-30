using UnityEngine;
using UnityEngine.UI;

public class ReticuleRaycaster : MonoBehaviour
{
    public Image reticule;  // Assign the reticule UI Image in the Inspector.
    public Color defaultColor = Color.green;
    public Color targetColor = Color.red;
    public float rayLength = 100f;  // Adjust as needed based on your scene scale.
    public LayerMask targetLayer;   // Set this to the layer of your target objects in the Inspector.

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength, targetLayer))
        {
            if (hit.collider.gameObject.CompareTag("Target Cube"))  // Assuming the Cube has a Tag "Target"
            {
                reticule.color = targetColor;
            }
        }
        else
        {
            reticule.color = defaultColor;
        }
    }
}