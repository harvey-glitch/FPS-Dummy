using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    [Header("Weapon Bob")]
    [Tooltip("How fast the bob is")]
    public float bobSpeed = 8.0f;

    [Tooltip("How strong the bob is")]
    public float bobStrength = 0.1f;

    [Tooltip("Horizontal offset of bob")]
    public float bobOffset = 0.1f;

    private float _bobDeltaTime = 0.0f;

    public void Update()
    {
        CalculateBobOffset();
    }

    private void CalculateBobOffset()
    {
        Vector3 rawMoveInput = InputManager.GetMoveInput().normalized;

        if (rawMoveInput.sqrMagnitude >= 0.001f)
        {
            _bobDeltaTime += Time.deltaTime * bobSpeed;

            float yBob = -Mathf.Abs(Mathf.Sin(_bobDeltaTime)) * bobStrength;
            float xBob = Mathf.Sin(_bobDeltaTime + Mathf.PI / 2) * bobOffset;

            Vector3 newPosition = new Vector3(xBob, yBob, 0.0f);
            Vector3 newRotation = new Vector3(0.0f, 0.0f, -xBob * 40.0f);
            WeaponAnimator.instance.AddBobbingOffset(newPosition, newRotation);
        }
    }
}
