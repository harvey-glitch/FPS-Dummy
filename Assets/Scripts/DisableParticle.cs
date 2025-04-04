using System.Collections;
using UnityEngine;

public class DisableParticle : MonoBehaviour
{
    [Header("PARTICLE")]
    [SerializeField] float lifeTime = 1f; // Total time before returning to pool
    [SerializeField] string objectTag = ""; // The object tag used to return to the pool

    void OnEnable()
    {
        StartCoroutine(StartCountdown());
    }

    // Method to return the projectile to pool after specified time
    IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(lifeTime);
        ReturnToPool();
    }

    void ReturnToPool()
    {
        PoolManager.instance.ReturnObject(objectTag, gameObject);
    }
}
