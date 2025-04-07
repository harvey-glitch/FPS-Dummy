using UnityEngine;

public class Pistol : Weapon
{
    [Header("WEAPON")]
    [SerializeField] Transform firePoint;

    public override void Fire()
    {
        // Get the projectile from the pool
        GameObject projectile = PoolManager.instance.GetObject("Projectile");

        // Set the position and rotation
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;

        base.Fire();
    }
}