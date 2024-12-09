using UnityEngine;
using UnityEngine.AI;

public class EnemyAiPatrol : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    public BoxCollider handBoxCollider;

    public float enemySpeed = 3.5f;
    public float enemyChaseSpeed = 5f;
    public float patrolRange = 10f;
    public float sightRange = 10f;
    public float attackRange = 2f;
    public float waypointSettleTime = 3f;
    public float playerDetectionAngle = 180f;

    private Vector3 destPoint;
    private bool walkPointSet;
    private float waypointTimer;
    private int damageToGive = 1;
    private Transform currentTarget;
    private bool isAttacking;

    Animator animator;

    private enum EnemyState
    {
        Patrolling,
        Chasing,
        Attacking
    }
    private EnemyState currentState = EnemyState.Patrolling;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = enemySpeed;
        agent.stoppingDistance = attackRange;
    }

    void Update()
    {
        Collider[] playerInSight = Physics.OverlapSphere(transform.position, sightRange, playerLayer);

        // Ensure we have a player and check visibility
        bool canSeePlayer = playerInSight.Length > 0 &&
                            IsPlayerInFieldOfView(playerInSight[0].transform);

        bool isPlayerInAttackRange = playerInSight.Length > 0 &&
            Vector3.Distance(transform.position, playerInSight[0].transform.position) <= attackRange;

        switch (currentState)
        {
            case EnemyState.Patrolling:
                // As soon as player is detected, switch to chasing
                if (canSeePlayer)
                {
                    currentState = EnemyState.Chasing;
                    currentTarget = playerInSight[0].transform;
                    agent.speed = enemyChaseSpeed; // Immediately switch to chase speed
                    break;
                }
                PatrolBehavior();
                break;

            case EnemyState.Chasing:
                // More aggressive chasing logic
                if (canSeePlayer)
                {
                    if (isPlayerInAttackRange)
                    {
                        currentState = EnemyState.Attacking;
                    }
                    else
                    {
                        ChaseBehavior(currentTarget);
                    }
                }
                else
                {
                    // Keep chasing last known player position for a bit
                    ChaseBehavior(currentTarget);
                }
                break;

            case EnemyState.Attacking:
                if (!isPlayerInAttackRange)
                {
                    currentState = EnemyState.Chasing;
                    isAttacking = false;
                    break;
                }

                AttackBehavior();
                break;
        }
    }

    bool IsPlayerInFieldOfView(Transform playerTransform)
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Increase the angle check and add more lenient line of sight check
        return angleToPlayer <= playerDetectionAngle / 2 &&
               (!Physics.Linecast(transform.position, playerTransform.position, groundLayer) ||
                Vector3.Distance(transform.position, playerTransform.position) < attackRange);
    }

    void PatrolBehavior()
    {
        if (!walkPointSet)
        {
            SearchForDestination();
        }
        else
        {
            agent.SetDestination(destPoint);

            waypointTimer += Time.deltaTime;
            if (waypointTimer >= waypointSettleTime)
            {
                walkPointSet = false;
                waypointTimer = 0;
            }
        }
    }

    void SearchForDestination()
    {
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        destPoint = new Vector3(
            transform.position.x + randomX,
            transform.position.y,
            transform.position.z + randomZ
        );

        RaycastHit hit;
        if (Physics.Raycast(destPoint, Vector3.down, out hit, 100f, groundLayer))
        {
            destPoint = hit.point;
            walkPointSet = true;
        }
    }

    void ChaseBehavior(Transform playerTransform)
    {
        agent.speed = enemyChaseSpeed;
        agent.SetDestination(playerTransform.position);
    }

    void AttackBehavior()
    {
        agent.speed = enemySpeed;
        agent.SetDestination(transform.position);

        if (!isAttacking)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    void EnableAttck()
    {
        handBoxCollider.enabled = true;
    }

    void DisableAttck()
    {
        handBoxCollider.enabled = false;
        isAttacking = false;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Vector3 hitDirection = (other.transform.position - transform.position).normalized;
            FindObjectOfType<HealthManager>().hurtPlayer(damageToGive, hitDirection);
        }
    }
}