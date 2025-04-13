using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [Tooltip("Rotation offset of the recoil")]
    public float rotationOffset = 0.3f;

    [Tooltip("How strong the recoil is")]
    public float recoilStrength = 0.1f;

    [Tooltip("How fast the recoild is")]
    [SerializeField] float recoilSpeed = 5f;

    private Vector3 _originalPosition;
    private Vector3 _currentRecoilPosition;
    private Quaternion _currentRecoilRotation;
    private Quaternion _originalRotation;

    private void Start()
    {
        // save the initial local transform
        _originalPosition = transform.localPosition;
        _originalRotation = transform.localRotation;
    }

    private void Update()
    {
        Recoil();
    }

    private void Recoil()
    {
        // determine target offset and rotation
        Vector3 targetPosition = Vector3.zero;
        Quaternion targetRotation = Quaternion.identity;

        if (Input.GetMouseButton(0) && isWeaponFired())
        {
            float randomRecoil = Random.Range(-rotationOffset, rotationOffset);

            targetPosition = Vector3.back * recoilStrength;
            targetRotation = Quaternion.Euler(-randomRecoil, randomRecoil, 0f);
        }

        // smoothly interpolate toward target
        _currentRecoilPosition = Vector3.Lerp(_currentRecoilPosition, targetPosition, Time.deltaTime * recoilSpeed);
        _currentRecoilRotation = Quaternion.Slerp(_currentRecoilRotation, targetRotation, Time.deltaTime * recoilSpeed);

        // apply final transform
        transform.localPosition = _originalPosition + _currentRecoilPosition;
        transform.localRotation = _originalRotation * _currentRecoilRotation;
    }

    private bool isWeaponFired()
    {
        WeaponManager manager = WeaponManager.instance;

        // return true if theres a valid weapon and i can be fire
        return manager.currentWeapon != null && manager.currentWeapon.CanFire();
    }
}
