using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    [Header("Weapon")]
    [Tooltip("Holds the current equipped weapon")]
    public Weapon currentWeapon;

    [Tooltip("Transform where the weapons are place")]
    public Transform weaponSlot;

    [Header("Aiming")]
    [Tooltip("How much the camera zoom when aiming")]
    public float maxAimZoom;

    [Tooltip("Position of the weapon when aiming")]
    public Vector3 aimOffset;

    private Dictionary<Weapon.Category, Weapon> _equippedWeapons = new();

    private Camera _camera;
    private float _currentCameraFOV;
    private float _originalCameraFOV;
    private float _targetCameraFOV;

    private Vector3 _currentWeaponPosition;
    private Vector3 _originalWeaponPosition;

    #region Singleton
    private void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        _originalWeaponPosition = weaponSlot.transform.localPosition;

        _camera = Camera.main;
        _originalCameraFOV = _camera.fieldOfView;
        _currentCameraFOV = _originalCameraFOV;
    }

    private void Update()
    {
        WeaponShoot();
        WeaponAim();
    }
    
    private void WeaponShoot()
    {
        // only fire when theres valid equipped weapon
        if (Input.GetMouseButton(0) && currentWeapon != null)
        {
            currentWeapon.Fire();
        }
    }

    private void WeaponAim()
    {
        // Right mouse button held = aiming
        bool aiming = Input.GetMouseButton(1);

        // Determine target FOV and weapon position based on aiming state
        _targetCameraFOV = aiming ? maxAimZoom : _originalCameraFOV;
        Vector3 targetPosition = aiming ? _originalWeaponPosition + aimOffset : _originalWeaponPosition;

        // Smoothly move weapon to aim position
        if (Vector3.Distance(_currentWeaponPosition, targetPosition) > 0.01f)
        {
            _currentWeaponPosition = Vector3.Lerp(_currentWeaponPosition, targetPosition, Time.deltaTime * 10f);
            weaponSlot.transform.localPosition = _currentWeaponPosition;
        }

        // Smoothly zoom the camera (FOV) only if different from target
        if (!Mathf.Approximately(_currentCameraFOV, _targetCameraFOV))
        {
            _currentCameraFOV = Mathf.Lerp(_currentCameraFOV, _targetCameraFOV, Time.deltaTime * 10f);
            _camera.fieldOfView = _currentCameraFOV;
        }
    }
    public void SetWeapon(Weapon newWeapon)
    {
        // Destroy old weapon if one already exists in the same slot
        if (_equippedWeapons.ContainsKey(newWeapon.category) && _equippedWeapons[newWeapon.category] != null)
        {
            Destroy(_equippedWeapons[newWeapon.category].gameObject);
        }

        _equippedWeapons[newWeapon.category] = newWeapon;
        EquipWeapon(newWeapon.category);
    }

    private void EquipWeapon(Weapon.Category category)
    {
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }

        currentWeapon = _equippedWeapons[category];
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.InitializeWeapon();
    }

    public bool isWeaponFired()
    {
        // return true if theres a valid weapon and it can be fire
        return currentWeapon != null && currentWeapon.CanFire();
    }
}