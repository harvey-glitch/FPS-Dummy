using UnityEngine;

public class EnemyHealth : HealthManager
{
    Enemy m_enemy; // Reference to the enemy script in parent object
    Ragdoll m_ragdoll; // " " "

    void Awake()
    {
        m_enemy = GetComponentInParent<Enemy>();
        m_ragdoll = GetComponentInParent<Ragdoll>();
    }

    public override void TakeDamage(float damageAmount)
    {
        // Call the base class method to reduce health
        base.TakeDamage(damageAmount);

        // Only provoked the enemy once
        if (!m_enemy.isProvoked)
        {
            m_enemy.InitializedEnemy();
        }
    }

    protected override void OnHealthDepleted()
    {
        m_ragdoll.EnableRagdoll(true);
        m_ragdoll.ApplyRagdollForce(30.0f);
        m_enemy.DisableEnemy();
    }
}