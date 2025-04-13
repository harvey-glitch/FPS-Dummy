using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum Category {primary, secondary, melee}

    [Tooltip("List of weapon categories")]
    public Category category;

    [Tooltip("Holds the weapon statistic")]
    public WeaponData weaponData;

    private int _currentAmmo;
    private float _lastFireTime;

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
        // track the time elapsed during the reload process
        float timeElapsed = 0f;

        while (timeElapsed <= weaponData.reloadSpeed)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _currentAmmo = weaponData.maxAmmo;
    }
}