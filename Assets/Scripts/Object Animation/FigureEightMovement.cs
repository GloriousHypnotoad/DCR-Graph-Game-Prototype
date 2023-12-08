using UnityEngine;

public class CenteredFigureEightMovement : MonoBehaviour
{
    public float speed = 1f;
    public float diameter = 10;

    private float timeCounter = 0;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
        timeCounter = Mathf.PI / 2;  // This will start the movement from the center
    }

    void Update()
    {
        timeCounter += Time.deltaTime * speed;

        float x = initialPosition.x + Mathf.Cos(timeCounter) * diameter; // Horizontal movement (left/right)
        float y = initialPosition.y; // Maintain the current y value
        float z = initialPosition.z + Mathf.Sin(2 * timeCounter) * diameter / 2; // Vertical movement (forward/backward)

        transform.position = new Vector3(x, y, z);
    }
}
