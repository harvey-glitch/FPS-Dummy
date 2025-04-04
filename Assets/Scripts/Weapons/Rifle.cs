using UnityEngine;

public class Rifle : Weapon
{
    [SerializeField] Transform firePoint;

    public override void Fire()
    {
        // Get the projectile from the pool
        GameObject projectile = PoolManager.instance.GetObject("Projectile");

        // Set the position and rotation
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;

        currentAmmo--;

        // Set the next fire time
        nextFireTime = Time.time + 1f / weaponData.firerate;

        PlayAudio();
        
        if (currentAmmo <= 0)
        {
            StartCoroutine(ReloadWeapon());
        }
    }
}