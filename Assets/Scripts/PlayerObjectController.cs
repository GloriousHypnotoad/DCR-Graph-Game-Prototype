using UnityEngine;

public class PlayerObjectController : MonoBehaviour
{
    public float moveSpeed = 5f;
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
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

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

        if (Mathf.Abs(transform.position.y - fixedVerticalPosition) > 0.01f)
        {
            transform.position = new Vector3(transform.position.x, fixedVerticalPosition, transform.position.z);
        }
    }
}