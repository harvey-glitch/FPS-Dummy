using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    [Header("BOBBING SETTINGS")]
    // How fast the bob is
    [SerializeField] float bobSpeed = 8.0f;
    // How strong the bob is
    [SerializeField] float bobAmount = 0.05f;
    // Horizontal offset of the bob
    [SerializeField] float bobOffset = 0.04f;
    // Smoothing effect of the bob
    [SerializeField] float smoothness = 3.0f;

    Vector3 m_originalPos; // Save the original position of this object
    Quaternion m_originalRot; // Save the original rotation of this object

    PlayerControl m_player; // Reference to the player controller
    float m_timer = 0.0f;

    private void Start()
    {
        m_player = FindAnyObjectByType<PlayerControl>();

        m_originalPos = transform.localPosition;
        m_originalRot = transform.localRotation;
    }

    private void Update()
    {
        ApplyBob();
    }

    private void ApplyBob()
    {
        Vector3 moveInput = InputManager.GetMoveInput().normalized;
        float speedFactor = m_player.currentSpeed / m_player.moveSpeed;

        // Reduce the bob intensity when firing
        float bobMultiplier = Input.GetMouseButton(0) ? 0.2f : 1.0f;

        if (moveInput.magnitude > 0)
        {
            // Make the speed of the bob relative to player current speed
            m_timer += Time.deltaTime * bobSpeed * speedFactor;

            // Create vertical and horizontal motion using sin wave
            float yBob = Mathf.Abs(Mathf.Sin(m_timer)) * bobAmount * bobMultiplier;
            float xBob = Mathf.Sin(m_timer + Mathf.PI / 2) * bobOffset * bobMultiplier;

            // Apply the calculated bob offset
            transform.localPosition = new Vector3(
                m_originalPos.x + xBob, m_originalPos.y + yBob, m_originalPos.z);

            transform.localRotation = Quaternion.Euler(0f, 0f, -xBob * 50.0f);
        }
        
        ResetBob();
    }

    private void ResetBob()
    {
        // Smoothly return back to its original position and rotation
        transform.localPosition = Vector3.Lerp(
            transform.localPosition, m_originalPos, Time.deltaTime * smoothness
        );

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation, m_originalRot, Time.deltaTime * smoothness
        );
    }
}
