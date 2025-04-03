using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // Reference to the weapon data
    [SerializeField] public WeaponData weaponData;

    // Tracks the current ammo of the weapon
    [HideInInspector] protected int currentAmmo;

    protected float nextFireTime; // Tracks when the weapon can be fire again

    public void Initialize(WeaponData data)
    {
        weaponData = data;
        currentAmmo = weaponData.maxAmmo;
    }

    // Abstract virtual for firing projectiles
    public abstract void Fire();

    // Method that check if the weapon can be fire
    public bool CanFire()
    {
        return Time.time >= nextFireTime && currentAmmo > 0;
    }

    // Method to reset the ammo
    protected IEnumerator ReloadWeapon()
    {
        // Track the time elapsed during the reload process
        float timeElapsed = 0f;

        while (timeElapsed <= weaponData.reloadSpeed)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Reset the current ammo
        currentAmmo = weaponData.maxAmmo;
    }
}