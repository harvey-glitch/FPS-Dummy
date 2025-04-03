using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("MOVEMENT")]
    [HideInInspector] public float moveSpeed = 4.0f;

    [Header("ROTATION")]
    [SerializeField] float lookLimit = 85.0f;
    [SerializeField] float lookSpeed = 3.0f;

    CharacterController m_controller;
    Camera m_camera;

    float xRotation = 0.0f;

    private void Start()
    {
        m_controller = GetComponent<CharacterController>();
        m_camera = Camera.main;

        // Hide the cursor at the start
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector3 moveInput = InputManager.GetMoveInput().normalized;
        
        if (moveInput.magnitude > 0)
        {
            Vector3 right = transform.right * moveInput.x;
            Vector3 forward = transform.forward * moveInput.z;

            // Create a direction based on input and player orientation
            Vector3 direction = right + forward;

            m_controller.Move(direction * moveSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        Vector2 mouseInput = InputManager.GetMouseInput() * lookSpeed;

        if (mouseInput.magnitude > 0)
        {
            // Adjust vertical rotation and clamp it to prevent over-rotation
            xRotation -= mouseInput.y;
            xRotation = Mathf.Clamp(xRotation, -lookLimit, lookLimit);

            // Rotate the camera vertically (up and down)
            m_camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Rotate the character horizontally (left and right)
            transform.Rotate(Vector3.up * mouseInput.x);
        }
    }
}
