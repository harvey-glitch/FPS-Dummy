using UnityEngine;

public class HitBox : MonoBehaviour
{
    HealthManager m_health; // Refernce to the enemy health script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_health = GetComponentInParent<HealthManager>();
    }

    public void ApplyDamage(float damage)
    {
        m_health.TakeDamage(damage);
    }

}
