using UnityEngine;

public class CenteredFigureEightMovement : MonoBehaviour, IAnimatable
{
    public float speed = 1f;
    public float diameter = 10;

    private float timeCounter = 0;
    private Vector3 initialPosition;
    private bool _isAnimating = false;

    public void ToggleAnimation(bool isAnimated)
    {
        _isAnimating = isAnimated;
        
        if (!_isAnimating)
        {
            timeCounter = Mathf.PI / 2;
            transform.position = initialPosition;
        }
    }

    void Start()
    {
        initialPosition = transform.position;
        timeCounter = Mathf.PI / 2;
    }

    void Update()
    {
        if (_isAnimating)
        {
            timeCounter += Time.deltaTime * speed;

            float x = initialPosition.x + Mathf.Cos(timeCounter) * diameter;
            float y = initialPosition.y;
            float z = initialPosition.z + Mathf.Sin(2 * timeCounter) * diameter / 2;

            transform.position = new Vector3(x, y, z);
        }
    }
}
