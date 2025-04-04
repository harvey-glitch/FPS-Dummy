using UnityEngine;

public abstract class HealthManager : MonoBehaviour
{
    [Header("HEALTH")]
    [SerializeField] protected float maxHealth = 100f; // Max health the object can have
    bool m_Died = false; // Flag to check whether the enemy is alive or not
    protected float currentHealth; // Tracks the remaining health

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    // Base method for taking damage
    public virtual void TakeDamage(float damageAmount)
    {
        if (m_Died) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth <= 0)
        {
            OnHealthDepleted();
            m_Died = true;
        }
    }

    // Abstract method for handling logic when health is depleted
    protected abstract void OnHealthDepleted();

}