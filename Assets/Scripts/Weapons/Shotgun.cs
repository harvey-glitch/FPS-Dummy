using UnityEngine;

public class Shotgun : Weapon
{
    // Transform where the projectile are shot from
    [SerializeField] Transform firePoint;

    public override void Fire()
    {
        // Fire multiple pellets with some spread
        for (int i = 0; i < weaponData.pellets; i++)
        {
            Vector3 spreadOffset = new Vector3(
            Random.Range(-weaponData.spreadAngle, weaponData.spreadAngle),
            Random.Range(-weaponData.spreadAngle, weaponData.spreadAngle),
            0);

            Quaternion spreadRotation = Quaternion.Euler(spreadOffset.x, spreadOffset.y, 0);

            // Retrieve the projectile from the pool
            GameObject projectile = PoolManager.instance.GetObject("Projectile");

            // Set each projectile position and rotation
            projectile.transform.position = firePoint.position;
            projectile.transform.rotation = firePoint.rotation * spreadRotation;
            Debug.Log("Spawned Projectile: " + weaponData.pellets);
        }

        // Reduce ammo and update UI
        currentAmmo--;

        // Set the next fire time
        nextFireTime = Time.time + 1f / weaponData.firerate;

        // Handle reloading
        if (currentAmmo <= 0)
        {
            StartCoroutine(ReloadWeapon());
        }
    }
}