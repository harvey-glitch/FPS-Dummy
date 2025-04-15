using System.Collections;
using System.Data;
using UnityEngine;

public class WeaponFall : MonoBehaviour
{
    [Tooltip("Determine how fast the falling speed is")]
    public float speedMultiplier;

    [Tooltip("Define how fast the bounce when the character land")]
    public float bounceSpeed;

    [Tooltip("The max bounce of the character")]
    public float maxBounceOffset;
    
    private FPSController _fpsController;

    private Vector3 _targetPosition;
    private Vector3 _currentPosition;
    private Vector3 _originalPosition;

    private float _fallVelocity= 0.0f;

    public void Start()
    {
        // save the original position of this transform
        _originalPosition = transform.localPosition;

        // try to get the fps controller script
        _fpsController = FindAnyObjectByType<FPSController>();
    }

    public void Update()
    {
        if (!_fpsController.isGrounded)
        {
            _fallVelocity += Time.deltaTime * speedMultiplier;
        }

        ApplyBounce();
        updateTransform();
    }


    public void ApplyBounce()
    {
        if (_fpsController.justLanded)
        {
             // calculate how much to bounce based on fall velocity
            float bounceAmount = Mathf.Clamp(_fallVelocity, 0, maxBounceOffset);

            // Add downward offset to the target position
            _targetPosition += Vector3.down * bounceAmount;

            _fallVelocity = 0f; // reset fall velocity when lands
        }
    }

    public void updateTransform()
    {
        // smoothly return back to original position
        _targetPosition = Vector3.Lerp(_targetPosition, _originalPosition, Time.deltaTime * bounceSpeed);

        // smoothly moves toward target position
        _currentPosition = Vector3.Lerp(_currentPosition, _targetPosition, Time.deltaTime * bounceSpeed);

        transform.localPosition = _currentPosition;
    }
}
