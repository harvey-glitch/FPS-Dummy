using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] private Transform pointOrigin;
    [SerializeField] private float damage;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layer;

    public void DealDamage()
    {
        Collider[] hitTargets = Physics.OverlapSphere(pointOrigin.position, radius, layer);

        foreach (Collider target in hitTargets)
        {
            HealthManager health = target.GetComponent<HealthManager>();

            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log(health);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(pointOrigin.position, radius);
    }
}
