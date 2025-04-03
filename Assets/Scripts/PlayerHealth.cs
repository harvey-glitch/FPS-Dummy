using UnityEngine;

public class PlayerHealth : HealthManager
{
    public override void TakeDamage(float damageAmount)
    {
        // Call the base class method to reduce health
        base.TakeDamage(damageAmount);
    }

    protected override void OnHealthDepleted()
    {
        gameObject.SetActive(false);
    }
}