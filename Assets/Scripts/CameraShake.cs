using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Quaternion _originalcameraRotation;
    private Quaternion _currentCameraRotation;
    private Quaternion _targetCameraRotation;

    private Vector3 _originalCameraPosition;
    private Vector3 _currentCameraPosition;
    private Vector3 _targetCameraPosition;

    [Tooltip("How strong the camera shake is")]
    public float shakeMagnitude = 2.0f;

    [Tooltip("Determine the speed of the shake")]
    public float shakeSpeed = 10.0f;

    private FPSController _fpsController;

    private void Start()
    {
        _originalcameraRotation = transform.localRotation;

        _fpsController = FindFirstObjectByType<FPSController>();
    }

    private void ApplyCameraShake()
    {
        _targetCameraRotation = Quaternion.Slerp(_targetCameraRotation, _originalcameraRotation, Time.deltaTime * shakeSpeed);
        _currentCameraRotation = Quaternion.Slerp(_currentCameraRotation, _targetCameraRotation, Time.deltaTime * shakeSpeed);
        transform.localRotation = _currentCameraRotation;
    }

    public void AddCameraShake(float magtinude)
    {
        float xRotation = Random.Range(-magtinude, magtinude) * shakeMagnitude;
        float yRotation = Random.Range(-magtinude, magtinude) * shakeMagnitude;

        _targetCameraRotation *= Quaternion.Euler(-Mathf.Abs(xRotation), yRotation, 0.0f);
    }
}
