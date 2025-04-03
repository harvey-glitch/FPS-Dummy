using UnityEngine;
using UnityEngine.Rendering;

public class WeaponRecoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    [SerializeField] float recoilRotation = 0.3f;
    [SerializeField] float recoilKickback = 0.1f;
    [SerializeField] float recoilSpeed = 5f;

    Quaternion m_originalRot;
    Vector3 m_originalPos;

    void Start()
    {
        m_originalRot = transform.localRotation;
        m_originalPos = transform.localPosition;
    }

    void Update()
    {
        ResetRecoil();
        ApplyRecoil();
    }
        
    public void ApplyRecoil()
    {
        if (!WeaponManager.instance.Fired()) return;

        float randomRecoil = Random.Range(-recoilRotation, recoilRotation);

        Quaternion newRotation = Quaternion.Euler(0, randomRecoil, 0);
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
