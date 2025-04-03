using System.Collections;
using UnityEngine;

public class DisableParticle : MonoBehaviour
{
    // Total time before returning to pool
    [SerializeField] float lifeTime = 1f;
    // The object tag used to return to the pool
    [SerializeField] string objectTag = "";

    void OnEnable()
    {
        // Start the countdown coroutine when the object is enabled
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        // Return the object to the pool for the specified lifetime
        yield return new WaitForSeconds(lifeTime);
        ReturnToPool();
    }

    void ReturnToPool()
    {
        PoolManager.instance.ReturnObject(objectTag, gameObject);
    }
}
