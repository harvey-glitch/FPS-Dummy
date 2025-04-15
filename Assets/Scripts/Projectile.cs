using Unity.Mathematics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("Speed at which the projectile moves")]
    public float speed;

    private TrailRenderer _projectileTrail;

    private float _distanceTraveled;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _projectileTrail = GetComponent<TrailRenderer>();
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.freezeRotation = true;
    }

    private void Update()
    {
        Disable();
    }

    private void Disable()
    {
        _distanceTraveled += speed * Time.deltaTime;

        // disable upon reaching specified distance
        if (_distanceTraveled >= 10)
        {
            ReturnToPool();
        }
    }

    private void FixedUpdate()
    {
        // Move the Rigidbody forward along its transform's forward direction
        _rigidbody.linearVelocity = transform.forward * speed;
    }

    private void ResetProjectleData()
    {
        _distanceTraveled = 0;

        _projectileTrail.Clear();
    }

    void ReturnToPool()
    {
        PoolManager.instance.ReturnObject("Projectile", gameObject);
        ResetProjectleData();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Handle collision logic here
        ContactPoint contact = collision.contacts[0];
        Vector3 hitPoint = contact.point;
        Vector3 hitRotation = contact.normal;

        // Get the particle from the pool and set its transform
        GameObject impactParticle = PoolManager.instance.GetObject("Impact");
        impactParticle.transform.position = hitPoint;
        impactParticle.transform.rotation = Quaternion.LookRotation(hitRotation);

        ReturnToPool(); // Return to pool after use
    }
}