using UnityEngine;
using UnityEngine.AI;

public class EnemyAiPatrol : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    [SerializeField] LayerMask groundLayer, playerLayer;
    Animator animator;
    public BoxCollider handBoxCollider;
    private int damageToGive = 1;

    [SerializeField] float enemySpeed = 3.5f;
    [SerializeField] float enemyChaseSpeed = 5f;

    // patrol
    Vector3 destPoint;
    bool walkPointSet;
    [SerializeField] float range;

    // state change
    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttackRange;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemySpeed;
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSight && !playerInAttackRange) Patrol();
        if (playerInSight && !playerInAttackRange) Chase();
        if (playerInSight && playerInAttackRange) Attack();
    }

    void Patrol()
    {
        agent.speed = enemySpeed;
        if (!walkPointSet) SearchForDest();
        if (walkPointSet) agent.SetDestination(destPoint);
        if (Vector3.Distance(transform.position, destPoint) < 10) walkPointSet = false;
    }

    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);
        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkPointSet = true;
        }
    }

    void Chase()
    {
        agent.speed = enemyChaseSpeed;
        agent.SetDestination(player.transform.position);
    }

    void Attack()
    {
        agent.speed = enemySpeed; // Slow down during attack
        //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("AttackImadethis"))
        //{
        //    animator.SetTrigger("Attack");
        //    agent.SetDestination(transform.position);
        //}

        animator.SetTrigger("Attack");
        agent.SetDestination(transform.position);
    }

    void EnableAttck()
    {
        handBoxCollider.enabled = true;
    }

    void DisableAttck()
    {
        handBoxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);
        var player = other.GetComponent<PlayerController>();
        Debug.Log("Player component found: " + (player != null));
        if (player != null)
        {
            Debug.Log("HIT");
            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection = hitDirection.normalized;
            FindObjectOfType<HealthManager>().hurtPlayer(damageToGive, hitDirection);
        }

    }
}