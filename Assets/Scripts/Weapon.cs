using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] public WeaponData weaponData;
    [HideInInspector] protected int currentAmmo;

    protected float nextFireTime;

    public void Initialize(WeaponData data)
    {
        weaponData = data;

        currentAmmo = weaponData.maxAmmo;
    }

    // Abstract method for firing projectiles; to be implemented by derived classes
    public abstract void Fire();

    public bool CanFire()
    {
        return Time.time >= nextFireTime && currentAmmo > 0;
    }

    protected IEnumerator ReloadWeapon()
    {
        // Track the time elapsed during the reload process
        float timeElapsed = 0f;

        while (timeElapsed <= weaponData.reloadSpeed)
        {
            timeElapsed += Time.deltaTime;

            // Wait for the next frame before continuing the loop
            yield return null;
        }

        // Reset the current ammo
        currentAmmo = weaponData.maxAmmo;
    }
}