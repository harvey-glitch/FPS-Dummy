using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    enum States { idle, chase, attack } // States for AI

    [Header("ENEMY")]
    [SerializeField] float detectionRange= 10.0f; // The distance AI can detect target.
    [SerializeField] float attackDistance = 2.0f; // The distance AI can attack target.
    [SerializeField] LayerMask obstacleMask; // A layer mask that block the AI's line of sight.
    [HideInInspector] public bool isProvoked; // Flag to indicate if the AI is provoked or not
    [SerializeField] States currentState = States.idle; // Tracks the current state of the AI
    NavMeshAgent m_agent; // Reference to the NavMeshAgent component
    Animator m_animator; // Reference to the Animator component
    Transform m_target; // Reference to the player transform
    bool m_isAttacking;  // Flag to indicate whether the AI is currently in the middle of an attack.
    Vector3 m_targetLastPosition; // Stores the last known position of the target
    float m_nextUpdateTime; // The next time when the AI should update its chase destination.

    void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();

        // Get the player transform
        m_target = FindAnyObjectByType<PlayerControl>().transform;
    }

    void Update()
    {
        StateTransition();
    }

    void StateTransition()
    {
        HandleAnimations();

        switch (currentState)
        {
            case States.idle:
                SearchTarget();
                break;
            case States.chase:
                ChaseTarget();

                if (GetTargetDistance() <= attackDistance)
                    ChangeState(States.attack, true);
                break;
            case States.attack:
                AttackTarget();
                break;
        }

        LookAtTheTarget();
    }

    void SearchTarget()
    {
        // Check if the target is within the detection distance
        if (GetTargetDistance() <= detectionRange)
        {
            // Check if the target is visible
            if (!IsTargetObstructed())
            InitializedEnemy();
        }
    }

    void ChaseTarget()
    {
        // Update the destination based on interval
        if (Time.time >= m_nextUpdateTime)
        {
            // Check if the player has moved significantly since the last update
            if (Vector3.Distance(m_targetLastPosition, m_target.position) > 1.0f)
            {
                UpdateDestination();
            }
            // schedule the next update based on the update interval
            m_nextUpdateTime = Time.time + 0.30f;
        }
    }

    void AttackTarget()
    {   
        if (!m_isAttacking)
        {
            m_isAttacking = true;
            m_animator.SetTrigger("Attack");
        }
    }

    void LookAtTheTarget()
    {
        // Only starts rotating when provoked
        if (!isProvoked) return;

        // Get the direction from the AI to the target
        Vector3 directionToTarget = GetTargetDirection();
        directionToTarget.y = 0; // Ignore height difference

        // Check if the rotation difference is significant
        float directionDifference = Vector3.Dot(transform.forward.normalized, directionToTarget.normalized);

        // Adjust for threhold (1.0 means perfectly aligned)
        if (directionDifference < 0.99f)
        {
            // Rotate the enemy to face the target
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f);
        }
    }

    void HandleAnimations()
    {
        // Set the Speed parameter using agent's speed in the blend tree
        m_animator.SetFloat("Speed", m_agent.velocity.magnitude);
    }

public void OnAttackFinished()
{
    if (GetTargetDistance() > attackDistance)
    {
        ChangeState(States.chase, false);
    }
    m_isAttacking = false;
}

    void UpdateDestination()
    {
        // Update the player's last known position
        m_targetLastPosition = m_target.position;

        // Update the enemy destination
        m_agent.SetDestination(m_target.position);
    }

    void ChangeState(States newState, bool stopMovement)
    {
        currentState = newState;
        m_agent.isStopped = stopMovement;
    }

    public void InitializedEnemy()
    {
        isProvoked = true;
        ChangeState(States.chase, false);
        UpdateDestination();
    }

    #region UTILITY METHODS
    float GetTargetDistance()
    {
        // return the distance between the AI and target
        return Vector3.Distance(transform.position, m_target.position);
    }

    Vector3 GetTargetDirection()
    {
        // return the direction from the AI to the target
        return (m_target.position - transform.position).normalized;
    }

    bool IsTargetObstructed()
    {
        // Check if theres an obstacle block the player
        return Physics.Raycast(transform.position, GetTargetDirection(), GetTargetDistance(), obstacleMask);
    }

    public void DisableEnemy()
    {
        m_agent.enabled = false;
        this.enabled = false;
    }
    #endregion
}