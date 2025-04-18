using System.Collections;
using System.Data;
using UnityEngine;

public class WeaponFall : MonoBehaviour
{
    [Header("Weapon Fall")]
    [Tooltip("Determine how fast the falling speed is")]
    public float fallSpeed = 0.5f;

    [Tooltip("Define how fast the bounce when the character land")]
    public float bounceSpeed = 15.0f;

    [Tooltip("The max bounce of the character")]
    public float bounceOffset = 1.0f;
    
    private FPSController _fpsController;

    private float _fallVelocity= 0.0f;

    private void Start()
    {
        _fpsController = FindAnyObjectByType<FPSController>();
    }

    private void Update()
    {
        CalculateBounceOffset();
    }


    public void CalculateBounceOffset()
    {
        if (!_fpsController.isGrounded)
        {
            // advance the fall velocity based on fall speed
            _fallVelocity += Time.deltaTime * fallSpeed;
        }

        if (_fpsController.justLanded)
        {
            // calculate how much to bounce based on fall velocity
            float bounceAmount = Mathf.Clamp(_fallVelocity, 0, bounceOffset);

            WeaponAnimator.instance.AddBounceOffset(Vector3.down * bounceAmount);

            _fallVelocity = 0f; // reset fall velocity when lands
        }
    }
}
