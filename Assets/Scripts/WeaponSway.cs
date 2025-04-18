using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Weapon Sway")]
    [Tooltip("How strong the sway is")]
    public float swayStrength = 0.1f;

    [Tooltip("Limits how far the sway can be")]
    public float maxSway = 0.2f;

    private void Update()
    {
        CalculateSwayOffset();
    }

    private void CalculateSwayOffset()
    {
        Vector2 rawLookInput = InputManager.GetLookInput() * swayStrength;

        // clamp the sway amount to avoid over swaying
        float swayX = Mathf.Clamp(-rawLookInput.x, -maxSway, maxSway);
        float swayY = Mathf.Clamp(-rawLookInput.y, -maxSway, maxSway);

        WeaponAnimator.instance.AddSwayOffset(new Vector3(swayX, swayY, 0.0f));
    }
}
