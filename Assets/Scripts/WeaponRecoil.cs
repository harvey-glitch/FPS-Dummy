using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [Tooltip("Determine how fast the recoil is")]
    public float recoilSpeed;

    private Vector3 _currentPosition;
    private Vector3 _targetPosition;
    private Vector3 _originalPosition;

    private Quaternion _originalRotation;
    private Quaternion _currentRotation;
    private Quaternion _targetRotation;

    private void Start()
    {
        // save the original local transform
        _originalPosition = transform.localPosition;
        _originalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && isWeaponFired())
        {
            ApplyRecoil();
        }
    
        UpdateTransform();
    }

    public void ApplyRecoil()
    {
        WeaponData weaponData = WeaponManager.instance.currentWeapon.weaponData;

        // create random horizontal rotation (y)
        float rotationY = Random.Range(-weaponData.horizontalRecoil, weaponData.horizontalRecoil);
        float rotationX = -Mathf.Abs(weaponData.verticalRecoil); // upward rotation

        _targetPosition += Vector3.back * weaponData.recoilKickback;

        _targetRotation *= Quaternion.Euler(rotationX, rotationY, 0f);
    }

    private void UpdateTransform()
    {
        // smoothly return back to original transform
        _targetPosition = Vector3.Lerp(_targetPosition, _originalPosition, recoilSpeed * Time.deltaTime);
        _targetRotation = Quaternion.Slerp(_targetRotation, _originalRotation, recoilSpeed * Time.deltaTime);

        // smoothly move towards the target transform
        _currentPosition = Vector3.Lerp(_currentPosition, _targetPosition, recoilSpeed * Time.deltaTime);
        _currentRotation = Quaternion.Slerp(_currentRotation, _targetRotation, recoilSpeed * Time.deltaTime);

        // Apply the smoothout transform
        transform.localPosition = _currentPosition;
        transform.localRotation = _currentRotation;
    }
    
    private bool isWeaponFired()
    {
        WeaponManager manager = WeaponManager.instance;

        // return true if theres a valid weapon and it can be fire
        return manager.currentWeapon != null && manager.currentWeapon.CanFire();
    }
}