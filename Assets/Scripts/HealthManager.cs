using UnityEngine;

public abstract class HealthManager : MonoBehaviour
{
    // Max health the object can have
    [SerializeField] protected float maxHealth = 100f;

    // Tracks the remaining health
    protected float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    // Base method for taking damage
    public virtual void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
            OnHealthDepleted();
    }

    // Abstract method for handling logic when health is depleted
    protected abstract void OnHealthDepleted();

}