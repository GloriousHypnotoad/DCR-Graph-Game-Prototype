using UnityEngine;

public class ContinuousScaleOnMouseOver : MonoBehaviour
{
    private float scaleFactor = 2f; // Uniform scale factor
    private float transitionSpeed = 2f; // Speed of scaling

    private Vector3 originalScale; // Original scale of the object
    private Vector3 targetScale; // Target scale when mouse is over
    private bool isMouseOver = false; // Flag to check if mouse is over the object

    // Additional variables for continuous scaling
    private bool scalingUp = true; // Flag to check the direction of scaling

    void Start()
    {
        originalScale = transform.localScale; // Store the original scale
        targetScale = originalScale * scaleFactor; // Calculate target scale
    }

    void Update()
    {
        if (isMouseOver)
        {
            // Continuously scale up and down
            if (scalingUp)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, transitionSpeed * Time.deltaTime);
                if (transform.localScale == targetScale)
                {
                    scalingUp = false; // Change direction of scaling
                }
            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, originalScale, transitionSpeed * Time.deltaTime);
                if (transform.localScale == originalScale)
                {
                    scalingUp = true; // Change direction of scaling
                }
            }
        }
        else
        {
            // Return to original scale when mouse exits
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, transitionSpeed * Time.deltaTime);
        }
    }

    void OnMouseEnter()
    {
        isMouseOver = true; // Set flag to true when mouse enters
    }

    void OnMouseExit()
    {
        isMouseOver = false; // Reset scaling direction and set flag to false when mouse exits
        scalingUp = true;
    }
}
