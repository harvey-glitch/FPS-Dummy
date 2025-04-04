using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("MOVEMENT")]
    public float moveSpeed = 4.0f; // Speed at which the player moves
    [SerializeField] private float sprintSpeed = 7.0f; // Speed when sprinting

    [Header("ROTATION")]
    [SerializeField] private float lookLimit = 85.0f; // Max camera rotation
    [SerializeField] private float lookSpeed = 3.0f; // How fast the camera rotation is

    CharacterController m_controller; // Reference to the character controller
    Camera m_camera; // Reference to the main camera

    [HideInInspector] public float currentSpeed; // Tracks the current speed of the player

    float m_xRotation = 0.0f; // Tracks the horizontal rotation of the camera

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
        currentSpeed = GetMovementSpeed();

        if (moveInput.magnitude > 0)
        {
            Vector3 right = transform.right * moveInput.x;
            Vector3 forward = transform.forward * moveInput.z;

            // Create a direction based on input and player orientation
            Vector3 direction = right + forward;


            m_controller.Move(direction * currentSpeed * Time.deltaTime);
        }
    }

    private float GetMovementSpeed()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
        return speed;
    }

    private void HandleRotation()
    {
        Vector2 mouseInput = InputManager.GetMouseInput() * lookSpeed;

        if (mouseInput.magnitude > 0)
        {
            // Adjust vertical rotation and clamp it to prevent over-rotation
            m_xRotation -= mouseInput.y;
            m_xRotation = Mathf.Clamp(m_xRotation, -lookLimit, lookLimit);

            // Rotate the camera vertically (up and down)
            m_camera.transform.localRotation = Quaternion.Euler(m_xRotation, 0f, 0f);

            // Rotate the character horizontally (left and right)
            transform.Rotate(Vector3.up * mouseInput.x);
        }
    }
}
