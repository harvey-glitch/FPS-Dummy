using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Speed at which the player moves")]
    public float moveSpeed = 4.0f;

    [Tooltip("Speed when sprinting")]
    public float sprintSpeed = 7.0f;

    [Header("Rotation")]
    [Tooltip("Max degree the camera can rotate vertically")]
    public float lookLimit = 85.0f;

    [Tooltip("Speed at which the camera can rotate")]
    public float lookSpeed = 3.0f;

    [Tooltip("Smothness of camera rotation")]
    public float lookSmoothness;

    [Header("Jumping")]
    [Tooltip("Max height the player can jump")]
    public float jumpHeight;

    [Tooltip("Determine how strong the gravity would be")]
    public float gravityMultiplier;

    [Header("Ground Check")]
    [Tooltip("Position where the ground check occurs")]
    public Transform spherePosition;

    [Tooltip("Determine the size of the sphere")]
    public float sphereRadius;

    [Tooltip("Position where the ground check occurs")]
    public float sphereOffset;

    [Tooltip("Determine the layer where the sphere can detect")]
    public LayerMask groundLayer;

    [Tooltip("Tracks whether the player is grounded or not")]
    public bool isGrounded;

    private bool _wasGrounded = false;
    [HideInInspector] public bool justLanded;

    private CharacterController _characterController;
    private Camera _camera;

    private Vector2 _smoothedLookInput;
    private Vector3 _smoothedMoveInput;

    private float _verticalRotation = 0.0f;
    private Vector3 _verticalVelocity;
    private Vector3 knockedVelocity;

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
        GroundCheck();
        HandleMove();
        HandleRotate();
        HandleJump();

        _wasGrounded = isGrounded; // update the was grounded state
    }

    private void HandleMove()
    {
        // determine the current speed based on input
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        Vector3 rawMoveInput = InputManager.GetMoveInput().normalized * currentSpeed;

        // smoothout the inputs
        _smoothedMoveInput = Vector3.Lerp(_smoothedMoveInput, rawMoveInput, Time.deltaTime * 10f);

        // only move when theres a valid input
        if (_smoothedMoveInput.sqrMagnitude >= 0.001f)
        {
            Vector3 direction = transform.right * _smoothedMoveInput.x + transform.forward * _smoothedMoveInput.z;

            _characterController.Move(direction * Time.deltaTime);
        }
    }

    private void HandleRotate()
    {
        Vector2 rawLookInput = InputManager.GetLookInput() * lookSpeed;

        // smoothout the inputs
        _smoothedLookInput = Vector2.Lerp(_smoothedLookInput, rawLookInput, Time.deltaTime * lookSmoothness);

        _verticalRotation -= _smoothedLookInput.y;

        // clamp the vertical rotation to avoid over rotation
        _verticalRotation = Mathf.Clamp(_verticalRotation, -lookLimit, lookLimit);

        _camera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * _smoothedLookInput.x);
    }

    private void GroundCheck()
    {
        Vector3 newSpherePosition = spherePosition.position + new Vector3(0, sphereOffset, 0);

        // check if the character is grounded
        isGrounded = Physics.CheckSphere(newSpherePosition, sphereRadius, groundLayer);

        // check if the character has landed
        justLanded = !_wasGrounded && isGrounded && Mathf.Abs(_verticalVelocity.y) > 0.1f;
    }

    private void HandleJump()
    {   
        // push the character to the ground
        if (isGrounded && _verticalVelocity.y < 0)
        {
            _verticalVelocity.y = -2f;
        }

        // calculate the required velocity to jump at specified height
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f);
        }

        _verticalVelocity.y += -9.81f * gravityMultiplier * Time.deltaTime;

        Vector3 totalVelocity = _verticalVelocity + knockedVelocity;
        _characterController.Move(totalVelocity * Time.deltaTime);
    }

    private void ApplyKnockBack(float knockStrength, float offset)
    {
        knockedVelocity += -transform.forward * knockStrength + Vector3.up * offset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 newSpherePosition = spherePosition.position + new Vector3(0, sphereOffset, 0);

        Gizmos.DrawWireSphere(newSpherePosition, sphereRadius);
    }
}