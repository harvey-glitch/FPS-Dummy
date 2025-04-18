using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    public static WeaponAnimator instance;

    [Header("Animation Speed")]
    [Tooltip("how fast the recoil animation is")]
    public float recoilSpeed;

    [Tooltip("how fast the bobbing animation is")]
    public float bobbingSpeed;

    [Tooltip("how fast the sway animation is")]
    public float swaySpeed;

    [Tooltip("how fast the bounce animation is")]
    public float bounceSpeed;

    [Tooltip("Transform where the weapons are stored")]
    public Transform weaponSlot;

    // original transform
    private Vector3 _originalPosition;
    private Vector3 _originalRotation;

    // weapon recoil
    private Vector3 _currentRecoilPosition;
    private Vector3 _targetRecoilPosition;
    private Vector3 _currentRecoilRotation;
    private Vector3 _targetRecoilRotation;

    // weapon bobbing
    private Vector3 _currentBobPosition;
    private Vector3 _targetBobPosition;
    private Vector3 _currentBobRotation;
    private Vector3 _targetBobRotation;

    // weapon bounce
    private Vector3 _currentBouncePosition;
    private Vector3 _targetBouncePosition;

    // weapon sway
    private Vector3 _currentSwayPosition;
    private Vector3 _targetSwayPosition;

    // weapon aiming
    private Vector3 _currentAimPosition;
    private Vector3 _targetAimPosition;
    private float _currentCameraFov;
    private float _targetCameraFov;
    private Camera _camera;

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
        // save the original transform
        _originalPosition = weaponSlot.localPosition;
        _originalRotation = weaponSlot.localRotation.eulerAngles;

        _camera = Camera.main;
        _currentCameraFov = _camera.fieldOfView;
    }

    private void Update()
    {
        ApplyBobbing();
        ApplySwaying();
        ApplyRecoil();
        ApplyBounce();
        ApplyAiming();

        BlendOffsets(); // sum all position and rotation offset then apply to the transform
    }

    private void BlendOffsets()
    {
        Vector3 totalPositionOffset = _originalPosition + _currentBobPosition + _currentSwayPosition +
            _currentRecoilPosition + _currentBouncePosition + _currentAimPosition;

        Vector3 totalRotationOffset = _originalRotation + _currentBobRotation + _currentRecoilRotation;

        weaponSlot.localPosition = totalPositionOffset;
        weaponSlot.localRotation = Quaternion.Euler(totalRotationOffset);
    }

    private void ApplyBobbing()
    {
        SmoothoutTransform(ref _currentBobPosition, ref _targetBobPosition, Vector3.zero, bobbingSpeed);
        SmoothoutTransform(ref _currentBobRotation, ref _targetBobRotation, Vector3.zero, bobbingSpeed);
    }

    private void ApplySwaying()
    {
        _currentSwayPosition = Vector3.Lerp(_currentSwayPosition, _targetSwayPosition, Time.deltaTime * swaySpeed);
    }

    private void ApplyAiming()
    {
        _currentAimPosition = Vector3.Lerp(_currentAimPosition, _targetAimPosition, Time.deltaTime * 10.0f);

        // adjust camera fov accordingly
        if (!Mathf.Approximately(_currentCameraFov, _targetCameraFov))
        {
            _currentCameraFov = Mathf.Lerp(_currentCameraFov, _targetCameraFov, Time.deltaTime * 10.0f);
            _camera.fieldOfView = _currentCameraFov;
        }
    }

    private void ApplyRecoil()
    {
        SmoothoutTransform(ref _currentRecoilPosition, ref _targetRecoilPosition, Vector3.zero, recoilSpeed);
        SmoothoutTransform(ref _currentRecoilRotation, ref _targetRecoilRotation, Vector3.zero, recoilSpeed);
    }

    private void ApplyBounce()
    {
        SmoothoutTransform(ref _currentBouncePosition, ref _targetBouncePosition, Vector3.zero, bounceSpeed);
    }

    private void SmoothoutTransform(ref Vector3 current, ref Vector3 target, Vector3 original, float speed)
    {
        float t = Time.deltaTime * speed;

        target = Vector3.Lerp(target, original, t);
        current = Vector3.Lerp(current, target, t);
    }

    public void AddBobbingOffset(Vector3 position, Vector3 rotation)
    {
        _targetBobPosition = position;
        _targetBobRotation = rotation;
    }

    public void AddSwayOffset(Vector3 position)
    {
        _targetSwayPosition = position;
    }

    public void AddRecoilOffset(Vector3 position, Vector3 rotation)
    {
        _targetRecoilPosition += position;
        _targetRecoilRotation += rotation;
    }

    public void AddBounceOffset(Vector3 position)
    {
        _targetBouncePosition += position;
    }

    public void AddAimingOffset(Vector3 position, float newFov)
    {
        _targetAimPosition = position;
        _targetCameraFov = newFov;
    }
}