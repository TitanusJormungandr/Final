using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GiantWolfController : MonoBehaviour
{
    //Define the wolf states
    public enum State { Patrol, Chase, Attack }
    public State currentState;

    //Player reference and configuration variables
    public Transform player;
    public float patrolRadius = 10f;
    public float patrolDelay = 2f;
    public float detectionRange = 12f;
    public float attackRange = 2f;
    public float fieldOfViewAngle = 120f;
    public float attackCooldown = 1.5f;

    //Internal state tracking
    private Vector3 spawnPoint;
    private NavMeshAgent agent;
    private Animator animator;

    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spawnPoint = transform.position;

        //Start in patrol mode
        ChangeState(State.Patrol);
    }

    void Update()
    {
        if (isAttacking) return;

        //Distance and visibility checks
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        bool canSeePlayer = distance <= detectionRange && angle <= fieldOfViewAngle * 0.5f;

        //State transitions based on player's position and visibility
        switch (currentState)
        {
            case State.Patrol:
                if (canSeePlayer)
                    ChangeState(State.Chase);
                break;

            case State.Chase:
                if (!canSeePlayer)
                    ChangeState(State.Patrol);
                else if (distance <= attackRange)
                    ChangeState(State.Attack);
                break;

            case State.Attack:
                if (distance > attackRange && !canSeePlayer)
                    ChangeState(State.Chase);
                else if (canSeePlayer && distance <= attackRange)
                    ChangeState(State.Patrol);
                break;
        }

    }

    //Handles stopping coroutines and starting the correct one for the new state
    void ChangeState(State newState)
    {
        StopAllCoroutines();
        currentState = newState;

        //Start new behavior based on state
        switch (currentState)
        {
            case State.Patrol:
                StartCoroutine(PatrolRoutine());
                break;
            case State.Chase:
                StartCoroutine(ChaseRoutine());
                break;
            case State.Attack:
                StartCoroutine(AttackRoutine());
                break;
        }
    }

    //Patrols within NavMesh
    IEnumerator PatrolRoutine()
    {
        while (currentState == State.Patrol)
        {
            //Pick a random point in a circle around the spawn location
            Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
            Vector3 target = spawnPoint + new Vector3(randomPoint.x, 0, randomPoint.y);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(target, out hit, patrolRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                animator.SetBool("Patrol", true);
                animator.SetBool("Chase", false);
            }

            yield return new WaitForSeconds(patrolDelay);
        }
    }

    //Constantly update the destination to the player's position
    IEnumerator ChaseRoutine()
    {
        animator.SetBool("Chase", true);
        while (currentState == State.Chase)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            yield return null;
            
        }

    }

    //Stops the wolf from moving and starts attack routine
    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        agent.isStopped = true;
        animator.SetBool("Attack", true);

        //Damage logic
        player.GetComponent<PlayerController>()?.TakeDamage(50);

        yield return new WaitForSeconds(1.0f);

        //Cooldown before next attack based on damage issues 
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }
}