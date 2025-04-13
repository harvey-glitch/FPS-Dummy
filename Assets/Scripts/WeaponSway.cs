using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Tooltip("How strong the sway is")]
    public float swayStrength = 3.0f;

    [Tooltip("Limits how far the sway can be")]
    [SerializeField] private float maxSway = 0.1f;

    [Tooltip("How smooth the sway is")]
    [SerializeField] private float swaySmoothness = 6f;

    private Vector3 _targetPosition;
    private Vector3 _initialPosition;

    void Start()
    {
        _initialPosition = transform.localPosition;
    }

    void Update()
    {
        Sway();
    }

    private void Sway()
    {
        Vector2 rawLookInput = InputManager.GetLookInput() * swayStrength;

        // clamp the sway to avoid over shooting
        float swayX = Mathf.Clamp(-rawLookInput.x, -maxSway, maxSway);
        float swayY = Mathf.Clamp(-rawLookInput.y, -maxSway, maxSway);

        _targetPosition = _initialPosition + new Vector3(swayX, swayY, 0f);

        // Smoothly apply sway
        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, Time.deltaTime * swaySmoothness);
    }
}
