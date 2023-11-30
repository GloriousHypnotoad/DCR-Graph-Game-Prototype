using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public float keySpeed = 10.0f;

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    void Update()
    {
        // Mouse Input for Rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Applying Rotation
        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation

        // Applying the rotations to the camera
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Keyboard Input for Movement
        float horizontalInput = Input.GetAxis("Horizontal") * keySpeed * Time.deltaTime;
        float verticalInput = Input.GetAxis("Vertical") * keySpeed * Time.deltaTime;

        // Applying Movement
        transform.Translate(new Vector3(horizontalInput, 0, verticalInput));
    }
}
