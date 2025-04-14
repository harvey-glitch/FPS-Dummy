using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    [Tooltip("How fast the bob is")]
    public float bobSpeed = 8.0f;

    [Tooltip("How strong the bob is")]
    public float bobStrength = 0.05f;

    [Tooltip("Horizontal offset of bob")]
    public float bobOffset = 0.2f;

    [Tooltip("Rotation offset of bob")]
    public float bobRotation = 70.0f;

    [Tooltip("Smoothness of bob motion")]
    public float bobSmoothness = 10.0f;

    private Vector3 _originalPosition;
    private Vector3 _currentBobPosition;
    private Quaternion _currentBobRotation;

    private float _bobDeltaTime = 0.0f;

    private void Start()
    {
        _originalPosition = transform.localPosition;
    }

    private void Update()
    {
        ApplyBob();
    }

    private void ApplyBob()
    {
        Vector3 rawMoveInput = InputManager.GetMoveInput().normalized;

        // determine target offset and rotation
        Vector3 targetPosition = Vector3.zero;
        Quaternion targetRotation = Quaternion.identity;

        if (rawMoveInput.sqrMagnitude >= 0.001f)
        {
            _bobDeltaTime += Time.deltaTime * bobSpeed;

            float yBob = -Mathf.Abs(Mathf.Sin(_bobDeltaTime)) * bobStrength;
            float xBob = Mathf.Sin(_bobDeltaTime + Mathf.PI / 2) * bobOffset;

            targetPosition = new Vector3(xBob, yBob, 0f);
            targetRotation = Quaternion.Euler(0f, 0f, -xBob * bobRotation);
        }

        // smoothly interpolate toward target
        _currentBobPosition = Vector3.Lerp(_currentBobPosition, targetPosition, Time.deltaTime * bobSmoothness);
        _currentBobRotation = Quaternion.Lerp(_currentBobRotation, targetRotation, Time.deltaTime * bobSmoothness);

        // apply final transform
        transform.localPosition = _originalPosition + _currentBobPosition;
        transform.localRotation =  _currentBobRotation;
    }
}
