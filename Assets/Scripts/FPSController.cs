using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Tooltip("Speed at which the player moves")]
    public float moveSpeed = 4.0f;

    [Tooltip("Speed when sprinting")]
    public float sprintSpeed = 7.0f;

    [Tooltip("Max degree the camera can rotate vertically")]
    public float lookLimit = 85.0f;

    [Tooltip("Speed at which the camera can rotate")]
    public float lookSpeed = 3.0f;

    [Tooltip("Smothness of camera rotation")]
    public float lookSmoothness;
    
    private CharacterController _characterController;
    private Camera _camera;

    private Vector2 smoothedLookInput;
    private Vector3 smoothedMoveInput;


    [HideInInspector] public float currentSpeed;

    private float _verticalRotation = 0.0f;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _camera = Camera.main;

        // hide the cursor at the start
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        Vector3 rawMoveInput = InputManager.GetMoveInput().normalized * currentSpeed;

        // Smoothout the inputs
        smoothedMoveInput = Vector3.Lerp(smoothedMoveInput, rawMoveInput, Time.deltaTime * 10f);

        // only move when theres a valid input
        if (smoothedMoveInput.sqrMagnitude >= 0.001f)
        {
            Vector3 direction = transform.right * smoothedMoveInput.x + transform.forward * smoothedMoveInput.z;

            _characterController.Move(direction * Time.deltaTime);
        }
    }

    private void Rotate()
    {
        Vector2 rawLookInput = InputManager.GetLookInput() * lookSpeed;

        // Smoothout the inputs
        smoothedLookInput = Vector2.Lerp(smoothedLookInput, rawLookInput, Time.deltaTime * lookSmoothness);

        _verticalRotation -= smoothedLookInput.y;

        // clamp the vertical rotation to avoid over rotation
        _verticalRotation = Mathf.Clamp(_verticalRotation, -lookLimit, lookLimit);

        _camera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * smoothedLookInput.x);
    }
}
