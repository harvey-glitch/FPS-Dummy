using UnityEngine;

public class Rifle : Weapon
{
    [Tooltip("Position where the projectile spawns")]
    public Transform firePoint;

    public override void Fire()
    {
        if (!CanFire()) return;

        // get the projectile from the pool and set its transform
        GameObject projectile = PoolManager.instance.GetObject("Projectile");
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;

        base.Fire(); // call the base method for firing
    }
}