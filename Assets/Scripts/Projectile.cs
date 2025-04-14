using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("Speed at which the projectile moves")]
    public float speed;

    private TrailRenderer _projectileTrail;

    private float _distanceTraveled;

    private void Start()
    {
        _projectileTrail = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        // continuesy move the projectile forward
        transform.position += transform.forward * speed * Time.deltaTime;

        _distanceTraveled += speed * Time.deltaTime;

        // disable upon reaching specified distance
        if (_distanceTraveled >= 10)
        {
            ReturnToPool();
        }
    }

    private void ResetProjectleData()
    {
        _distanceTraveled = 0;

        _projectileTrail.Clear();
    }

    void ReturnToPool()
    {
        PoolManager.instance.ReturnObject("Projectile", gameObject);
        ResetProjectleData();
    }

    private void OnTriggerEnter(Collider other)
    {
        // avoid colliding with other projectiles
        if (other.transform.CompareTag("Projectiles")) return;

        if (other.TryGetComponent<HitBox>(out HitBox hitBox))
        {
            float damage = WeaponManager.instance.currentWeapon.weaponData.damage;
            hitBox.ApplyDamage(damage);
        }

        SpawnParticle();
    }
    
    private void SpawnParticle()
    {   
        // get the particle from the pool and set its transform
        GameObject impactParticle = PoolManager.instance.GetObject("Impact");
        impactParticle.transform.position = transform.position;
        impactParticle.transform.rotation = Quaternion.identity;

        ReturnToPool(); // return to pool after use
    }
}