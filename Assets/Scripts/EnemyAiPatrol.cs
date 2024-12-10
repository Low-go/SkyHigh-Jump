using UnityEngine;
using UnityEngine.AI;

public class EnemyAIPatrol : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask groundLayer, playerLayer;
    public float wanderRadius = 10f;
    public float sightRange = 15f;
    public float attackRange = 2f;
    public float lingerTime = 3f;
    public float detectionAngle = 200f;

    private Vector3 lastKnownPosition;
    private bool playerInSight;
    private bool isLingering;
    private Transform player;
    private int damageToGive = 1;

    private enum State { Wandering, Chasing, Lingering, Attacking }
    private State currentState = State.Wandering;

    public BoxCollider handBoxCollider;
    private bool isAttacking;

    Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        DetectPlayer();

        switch (currentState)
        {
            case State.Wandering:
                Wander();
                if (playerInSight)
                {
                    SwitchState(State.Chasing);
                }
                break;

            case State.Chasing:
                ChasePlayer();
                if (!playerInSight)
                {
                    lastKnownPosition = player.position;
                    SwitchState(State.Lingering);
                }
                break;

            case State.Lingering:
                Linger();
                break;

            case State.Attacking:
                AttackPlayer();
                if (!playerInSight || Vector3.Distance(transform.position, player.position) > attackRange)
                {
                    SwitchState(State.Chasing);
                }
                break;
        }
    }

    private void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, sightRange, playerLayer);
        if (hits.Length > 0)
        {
            Transform target = hits[0].transform;
            Vector3 directionToPlayer = (target.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer <= detectionAngle / 2 && !Physics.Linecast(transform.position, target.position, groundLayer))
            {
                playerInSight = true;
                player = target;
                return;
            }
        }

        playerInSight = false;
    }

    private void Wander()
    {
        if (!agent.hasPath)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    private void ChasePlayer()
    {
        if (playerInSight)
        {
            agent.SetDestination(player.position);

            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                SwitchState(State.Attacking);
            }
        }
    }

    private void Linger()
    {
        if (!isLingering)
        {
            isLingering = true;
            agent.SetDestination(lastKnownPosition);
            Invoke(nameof(ResumeWandering), lingerTime);
        }
    }

    private void AttackPlayer()
    {
        // Stop moving while attacking
        agent.SetDestination(transform.position);

        // If not already attacking, trigger the attack animation
        if (!isAttacking)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    public void EnableAttck()
    {
        handBoxCollider.enabled = true;
    }

    public void DisableAttck()
    {
        handBoxCollider.enabled = false;
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Vector3 hitDirection = (other.transform.position - transform.position).normalized;
            FindObjectOfType<HealthManager>().hurtPlayer(damageToGive, hitDirection);
        }
    }

    private void ResumeWandering()
    {
        isLingering = false;
        SwitchState(State.Wandering);
    }

    private void SwitchState(State newState)
    {
        currentState = newState;
        if (newState == State.Attacking || newState == State.Chasing)
        {
            agent.ResetPath(); // Stop wandering
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

