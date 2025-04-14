using UnityEngine;

public class Pistol : Weapon
{
    [Tooltip("Transform where the projectile spawns")]
    public Transform firePoint;

    public override void Fire()
    {
        if (!CanFire()) return;

        // Get the projectile from the pool
        GameObject projectile = PoolManager.instance.GetObject("Projectile");

        // Set the position and rotation
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;

        base.Fire();
    }
}