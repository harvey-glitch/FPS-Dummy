using UnityEngine;
public class WeaponRecoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    // Rotation offset of the recoil
    [SerializeField] float recoilRotation = 0.3f;
    // How strong the recoili is
    [SerializeField] float recoilKickback = 0.1f;
    // How fast the recoil is
    [SerializeField] float recoilSpeed = 5f;

    Quaternion m_originalRot; // Saved the original rotation of this object
    Vector3 m_originalPos; // Saved the original position of this object

    void Start()
    {
        // Store the original position and rotation of this object
        m_originalRot = transform.localRotation;
        m_originalPos = transform.localPosition;
    }

    void Update()
    {
        ApplyRecoil();
        ResetRecoil();
    }
        
    public void ApplyRecoil()
    {
        // Only apply recoil when the user fired the weapon
        if (!WeaponManager.instance.Fired()) return;

        // Add random rotation offset on the recoil
        float randomRecoil = Random.Range(-recoilRotation, recoilRotation);

        Quaternion newRotation = Quaternion.Euler(-recoilRotation, randomRecoil, 0);
        transform.localRotation = transform.localRotation *= newRotation;

        // Move the weapon slightly backward
        transform.localPosition += Vector3.back * recoilKickback;
    }

    public void ResetRecoil()
    {
        // Smoothly return back to the original position and rotation
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation, m_originalRot, Time.deltaTime * recoilSpeed);

        transform.localPosition = Vector3.Lerp( 
            transform.localPosition, m_originalPos, Time.deltaTime * recoilSpeed);
    }
}
