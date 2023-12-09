using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 200f;
    private CameraMode _currentMode;
    private float _xRotation = 0f;
    private float _thirdPersonZoomSpeend = 4f;
    private float _birdsEyeZoomSpeend = 40f;
    private float _birdsEyeMovementSpeend = 10f;
    private float sprintMultiplier = 2f;

    public void SetCameraMode(CameraMode mode)
    {
        _currentMode = mode;
        switch (_currentMode)
        {
            case CameraMode.FirstPerson:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case CameraMode.ThirdPerson:
            case CameraMode.BirdsEye: // Currently, BirdsEye mode shares the same settings as ThirdPerson
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }

    void Update()
    {
        switch (_currentMode)
        {
            case CameraMode.FirstPerson:
                HandleFirstPersonModeInput();
                break;

            case CameraMode.ThirdPerson:
                HandleThirdPersonModeInput();
                break;

            case CameraMode.BirdsEye:
                HandleBirdsEyeModeInput();
                break;
        }
    }

    private void HandleFirstPersonModeInput()
    {  
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        // Note: Add any additional first-person camera logic here
    }

    private void HandleThirdPersonModeInput()
    {
        HandleCameraZoom(_thirdPersonZoomSpeend);
    }

    private void HandleBirdsEyeModeInput()
    {
        HandleCameraZoom(_birdsEyeZoomSpeend);

        // Example camera adjustment for zoom
        // transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -cameraZoom);

        // Movement along the x and z axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement;

        // Check for sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement = new Vector3(horizontal, 0, vertical) * _birdsEyeMovementSpeend * sprintMultiplier;
        }
        else
        {
            movement = new Vector3(horizontal, 0, vertical) * _birdsEyeMovementSpeend;
        }

        
        transform.Translate(movement * Time.deltaTime, Space.World);
    }

    void HandleCameraZoom(float movementSpeed)
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(Vector3.forward * scrollInput * movementSpeed, Space.Self);

        /*
        switch (axis.ToLower())
        {

            
            case "x":
                // Move along the local X-axis
                transform.Translate(Vector3.right * scrollInput * movementSpeed, Space.Self);
                break;

            case "y":
                // Move along the local Y-axis (inverted for natural feel)
                transform.Translate(Vector3.down * scrollInput * movementSpeed, Space.Self);
                break;

            case "z":
                // Move along the local Z-axis
                transform.Translate(Vector3.forward * scrollInput * movementSpeed, Space.Self);
                break;

            default:
                Debug.LogError("Invalid axis input. Please use 'x', 'y', or 'z'.");
                break;
            
        }
        */
    }
}
