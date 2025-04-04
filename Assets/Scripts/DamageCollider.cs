using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("COLLIDER")]
    [SerializeField] private Transform pointOrigin; // Origin of the collider
    [SerializeField] private float damage; // Amount of damage can inflict
    [SerializeField] private float radius; // Max radius of the collider
    [SerializeField] private LayerMask layer; // Layer the collider can detect

    public void DealDamage()
    {
        // Store all detected objects within the radius and store in array     
        Collider[] hitTargets = Physics.OverlapSphere(pointOrigin.position, radius, layer);

        foreach (Collider target in hitTargets)
        {
            // Loop though objects and check if one consist of health script
            HealthManager health = target.GetComponent<HealthManager>();

            if (health != null)
            {
                // Call the function to deal damage
                health.TakeDamage(damage);
            }
        }
    }

    // Make the collider for dealing damage visible
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pointOrigin.position, radius);
    }
}
