using UnityEngine;

public class EnemyHealth : HealthManager
{
    // Reference to the enemy controller class
    Enemy m_enemy;

    void Awake()
    {
        // Get enemy script component
        m_enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(float damageAmount)
    {
        // Call the base class method to reduce health
        base.TakeDamage(damageAmount);

        // Only provoked the enemy once
        if (!m_enemy.isProvoked)
        {
            m_enemy.OnProvoked();
        }
    }

    protected override void OnHealthDepleted()
    {
        gameObject.SetActive(false);
    }
}