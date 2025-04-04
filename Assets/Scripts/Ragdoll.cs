using Unity.VisualScripting;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] m_rigidbody;
    Animator m_animator;
    
    void Start()
    {
        m_rigidbody = GetComponentsInChildren<Rigidbody>();
        m_animator = GetComponentInParent<Animator>();

        InitializeRagdoll();
    }

    public void InitializeRagdoll()
    {
        EnableRagdoll(false);
        AddHitBoxOnColliders();
    }

    private void AddHitBoxOnColliders()
    {
        foreach(Rigidbody rigidbody in m_rigidbody)
        {
            rigidbody.AddComponent<HitBox>();
        }
    }

    public void EnableRagdoll(bool state)
    {
        foreach(Rigidbody rigidbody in m_rigidbody)
        {
            rigidbody.isKinematic = !state;
        }

        m_animator.enabled = !state;
    }

    public void ApplyRagdollForce(float force)
    {
        Rigidbody rigidbody = m_animator.GetBoneTransform(HumanBodyBones.Spine).GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            // Create direction based on enemy opposite direction with upward offset
            Vector3 direction = -transform.forward + Vector3.up * 0.5f;

            // Apply the normalized direction to the rigidbody 
            rigidbody.AddForce(direction.normalized * force, ForceMode.VelocityChange);
        }
    }
}
