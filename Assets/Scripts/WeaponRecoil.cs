using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [Tooltip("Rotation offset of the recoil")]
    public float rotationOffset = 0.3f;

    [Tooltip("How fast the recoild is")]
    [SerializeField] float recoilSpeed = 5f;

    private Vector3 _originalPosition;
    private Vector3 _targetPosition;
    
    private Quaternion _targetRotation;
    private Quaternion _originalRotation;

    private void Start()
    {
        // save the initial local transform
        _originalPosition = transform.localPosition;
        _originalRotation = transform.localRotation;
    }

    private void Update()
    {     
        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        if (Input.GetMouseButton(0) && isWeaponFired())
        {
            WeaponData weaponData = WeaponManager.instance.currentWeapon.weaponData;

            // create random horizontal rotation
            float rotationY = Random.Range(-weaponData.horizontalRecoil, weaponData.horizontalRecoil);

            // make rotation always upward
            float rotationX = -Mathf.Abs(weaponData.verticalRecoil);

            _targetPosition = Vector3.back * weaponData.recoilKickback;
            _targetRotation = Quaternion.Euler(rotationX, rotationY, 0f);

            transform.localPosition = _targetPosition;
            transform.localRotation = _targetRotation;
        }

        else{
            transform.localPosition = Vector3.Lerp(
                transform.localPosition, _originalPosition, Time.deltaTime * recoilSpeed
            );

            transform.localRotation = Quaternion.Slerp(
                transform.localRotation, _originalRotation, Time.deltaTime * recoilSpeed
            );
        }
    }

    private bool isWeaponFired()
    {
        WeaponManager manager = WeaponManager.instance;

        // return true if theres a valid weapon and it can be fire
        return manager.currentWeapon != null && manager.currentWeapon.CanFire();
    }
}
