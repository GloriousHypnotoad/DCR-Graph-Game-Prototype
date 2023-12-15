using System;
using UnityEngine;

public class PlayerObjectController : MonoBehaviour
{

    public float moveSpeed = 10f;
    public float sprintMultiplier = 2f;
    public float fixedVerticalPosition = 1f;

    private Vector3 moveDirection;
    private CharacterController controller;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();

        if (controller == null)
        {
            Debug.LogError("CharacterController not found on the player object.");
        }
    }

    void Update()
    {
        switch (GameSettings.ActiveCamera)
        {
            case CameraMode.FirstPerson:
                HandleFirstPersonInput();
                break;
            case CameraMode.ThirdPerson:
                HandleThirdPersonInput();
                break;
            case CameraMode.BirdsEye:
                break;
        }
    }

    void HandleFirstPersonInput()
    {
        // Original Input Handling
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        MoveCharacter(moveX, moveZ);

        // New Rotation Handling
        float rotateY = Input.GetAxis("Mouse X");
        RotateCharacter(rotateY);
    }

    void HandleThirdPersonInput()
    {
        // Tank Mode: Horizontal input for rotation, Vertical for forward/backward
        float rotateY = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        transform.Rotate(0, rotateY, 0);
        MoveCharacter(0, moveZ);
    }

    void MoveCharacter(float moveX, float moveZ)
    {
        moveDirection = transform.right * moveX + transform.forward * moveZ;
        
        // Check for sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveDirection *= moveSpeed * sprintMultiplier;
        }
        else
        {
            moveDirection *= moveSpeed;
        }

        controller.Move(moveDirection * Time.deltaTime);

        // Fixed vertical position
        if (Mathf.Abs(transform.position.y - fixedVerticalPosition) > 0.01f)
        {
            transform.position = new Vector3(transform.position.x, fixedVerticalPosition, transform.position.z);
        }
    }

    void RotateCharacter(float rotateY)
    {
        // Assuming a sensitivity factor for rotation speed
        float sensitivity = 100f;
        float rotationAmount = rotateY * sensitivity * Time.deltaTime;

        // Apply rotation around the y-axis
        transform.Rotate(0, rotationAmount, 0);
    }
}
