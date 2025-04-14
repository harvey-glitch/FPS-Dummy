using UnityEngine;

public class Shotgun : Weapon
{
    [Tooltip("Transform where the projectile spawns")]
    public Transform firePoint;

    public override void Fire()
    {
        if (!CanFire()) return;
        
        for (int i = 0; i < weaponData.pellets; i++)
        {
            // Create random direction based on spread value
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
        }

        base.Fire();
    }
}