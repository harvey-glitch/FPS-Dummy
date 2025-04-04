using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Speed at which the projectile moves
    [SerializeField] float speed;

    TrailRenderer m_projectileTrail;

    // Tracks the distance the projectile has traveled
    float m_distanceTraveled;

    private void Start()
    {
        m_projectileTrail = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        MoveProjectile();
    }

    void MoveProjectile()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        m_distanceTraveled += speed * Time.deltaTime;

        if (m_distanceTraveled >= 10)
        {
            ResetProjectleData();
            ReturnToPool();
        }
    }

    void ResetProjectleData()
    {
        m_distanceTraveled = 0;

        m_projectileTrail.Clear();
    }

    void ReturnToPool()
    {
        PoolManager.instance.ReturnObject("Projectile", gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
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
        GameObject impactParticle = PoolManager.instance.GetObject("Impact");
        impactParticle.transform.position = transform.position;
        impactParticle.transform.rotation = Quaternion.identity;

        ReturnToPool();
    }
}