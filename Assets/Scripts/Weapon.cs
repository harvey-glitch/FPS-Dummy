using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum Category {primary, secondary, melee}

    [Tooltip("List of weapon categories")]
    public Category category;

    [Tooltip("Holds the weapon statistic")]
    public WeaponData weaponData;

    [Tooltip("Reload animation reference to play")]
    public string reloadClip;

    private Animation _animation;

    private int _currentAmmo = 0;
    private bool _isReloading = false;
    private float _lastFireTime = 0.0f;

    private void Start()
    {
        _animation = GetComponent<Animation>();
    }

    public void InitializeWeapon()
    {
        _currentAmmo = weaponData.maxAmmo;
        _lastFireTime = Time.time;
    }

    public virtual void Fire()
    {
        _currentAmmo--;
        _lastFireTime = Time.time;

        if (_currentAmmo == 0)
        {
            StartCoroutine(StartReload());
        }
    }

    public bool CanFire()
    {
        return _currentAmmo > 0 && Time.time >= _lastFireTime + weaponData.firerate;
    }

    protected IEnumerator StartReload()
    {
        _animation.Play(reloadClip);

        while (_animation.isPlaying)
        {
            yield return null;
        }

        _currentAmmo = weaponData.maxAmmo;
    }
}