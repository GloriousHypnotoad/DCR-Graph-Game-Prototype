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
        
        // If the animation is being stopped, reset the position.
        if (!_isAnimating)
        {
            timeCounter = Mathf.PI / 2; // Reset to starting point in the figure-eight
            transform.position = initialPosition; // Reset to the initial position
        }
        
    }

    void Start()
    {
        initialPosition = transform.position;
        timeCounter = Mathf.PI / 2;  // This will start the movement from the center
    }

    void Update()
    {
        if (_isAnimating)
        {
            timeCounter += Time.deltaTime * speed;

            float x = initialPosition.x + Mathf.Cos(timeCounter) * diameter; // Horizontal movement (left/right)
            float y = initialPosition.y; // Maintain the current y value
            float z = initialPosition.z + Mathf.Sin(2 * timeCounter) * diameter / 2; // Vertical movement (forward/backward)

            transform.position = new Vector3(x, y, z);
        }
    }
}
