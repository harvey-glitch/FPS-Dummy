using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    [Header("Aiming Settings")]
    [Tooltip("Position offset of the weapon when aiming")]
    public Vector3 aimOffset;

    [Tooltip("Max camera FOV when aiming")]
    public float aimFov;

    private const float hipFov = 60.0f;

    public void Update()
    {
        WeaponAnimator.instance.AddAimingOffset(WeaponOffset(), CameraFov());
    }

    public Vector3 WeaponOffset()
    {
        return InputManager.GetAimInput() ? aimOffset : Vector3.zero;
    }

    public float CameraFov()
    {
        return InputManager.GetAimInput() ? aimFov : hipFov;
    }
}
