using UnityEngine;

public class CenteredFigureEightMovement : MonoBehaviour, IAnimatable
{
    public float speed = 1f;
    public float diameter = 10;

    private float timeCounter = 0;
    private Vector3 initialPosition;
    private bool _isAnimating = false; // Flag to control animation

    public void ToggleAnimation(bool isAnimated)
    {
        _isAnimating = isAnimated;
        
        // Reset position if animation is stopped
        if (!_isAnimating)
        {
            timeCounter = Mathf.PI / 2; // Reset to starting point in figure-eight
            transform.position = initialPosition; // Reset to initial position
        }
    }

    void Start()
    {
        initialPosition = transform.position;
        timeCounter = Mathf.PI / 2; // Start movement from center
    }

    void Update()
    {
        // Perform animation only if _isAnimating is true
        if (_isAnimating)
        {
            timeCounter += Time.deltaTime * speed;

            float x = initialPosition.x + Mathf.Cos(timeCounter) * diameter; // Horizontal movement
            float y = initialPosition.y; // Maintain y value
            float z = initialPosition.z + Mathf.Sin(2 * timeCounter) * diameter / 2; // Vertical movement

            transform.position = new Vector3(x, y, z);
        }
    }
}
