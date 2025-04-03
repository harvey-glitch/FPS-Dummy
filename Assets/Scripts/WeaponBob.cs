using Unity.Mathematics;
using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    [Header("BOBBING SETTINGS")]
    [SerializeField] float bobSpeed = 8.0f;
    [SerializeField] float bobAmount = 0.05f;
    [SerializeField] float bobOffset = 0.04f;

    Vector3 originalPos;
    quaternion originalRot;
    Vector3 currentPos;
    float timer = 0.0f;

    private void Start()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
    }

    private void Update()
    {
        HandleBobbing();
    }

    private void HandleBobbing()
    {
        var moveInput = InputManager.GetMoveInput().normalized;

        if (moveInput.magnitude > 0)
        {
            timer += Time.deltaTime * bobSpeed;

            float yBob = Mathf.Abs(Mathf.Sin(timer)) * bobAmount;

            float xBob = Mathf.Sin(timer + Mathf.PI / 2) * bobOffset;

            transform.localPosition = new Vector3(
                originalPos.x + xBob, 
                originalPos.y + yBob, 
                originalPos.z);

            transform.localRotation = Quaternion.Euler(0f, 0f, -xBob * 50.0f);
        }
    }
}
