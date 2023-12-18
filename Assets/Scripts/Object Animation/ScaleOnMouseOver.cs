using UnityEngine;

public class ScaleOnMouseOver : MonoBehaviour
{
    public float scaleFactor = 1.1f; // Uniform scale factor
    public float transitionSpeed = 0.5f; // Speed of scaling

    private Vector3 originalScale; // Original scale of the object
    private Vector3 targetScale; // Target scale when mouse is over
    private bool isMouseOver = false; // Flag to check if mouse is over the object

    void Start()
    {
        originalScale = transform.localScale; // Store the original scale
        targetScale = originalScale * scaleFactor; // Calculate target scale
    }

    void Update()
    {
        if (isMouseOver)
        {
            // Scale up the object
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, transitionSpeed * Time.deltaTime);
        }
        else
        {
            // Scale down the object
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, transitionSpeed * Time.deltaTime);
        }
    }

    void OnMouseEnter()
    {
        isMouseOver = true; // Set flag to true when mouse enters
    }

    void OnMouseExit()
    {
        isMouseOver = false; // Set flag to false when mouse exits
    }
}
