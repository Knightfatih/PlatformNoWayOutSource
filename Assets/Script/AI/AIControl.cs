using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject ragdoll;
    [SerializeField] float runSpeed;
    [SerializeField] float attackDistance;
    [SerializeField] GameObject attackCheck;
    [SerializeField] LayerMask playerMask;
    NavMeshAgent agent;
    Animator anim;
    Health health;
    Vector3 direction;
    public enum State { Idle, Attack, Chase, Dead };
    State state = State.Idle;
    
    float VisualDistance = 50f;

    public bool enemyDead;
    public float amount = 10;

    public float damageTimer = 0f;
    public float timeBetweenDamage = 1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        CheckForTarget();
    }

    void Update()
    {
        if (enemyDead)
        {
            state = State.Dead;
        }

        switch (state)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Chase:
                ChaseState();
                break;
            case State.Attack:
                AttackState(direction);
                break;
            case State.Dead:
                DeadState();
                break;
        }
    }

    void CheckForTarget()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            return;
        }
    }

    void IdleState()
    {
        if (CanSeePlayer())
        {
            state = State.Chase;
        }
    }

    void ChaseState()
    {
        agent.SetDestination(player.transform.position);
        agent.stoppingDistance = attackDistance;
        TurnOffAnims();
        agent.speed = runSpeed;
        anim.SetBool("isRunning", true);
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            state = State.Attack;
        }
    }

    void AttackState(Vector3 direction)
    {
        if(Physics.CheckSphere(attackCheck.transform.position, attackDistance, playerMask))
        {
            Health health = player.GetComponent<Health>();
            damageTimer += Time.deltaTime;
            if (damageTimer > timeBetweenDamage)
            {
                health.TakeDamage(amount, direction);
                AudioManager.instance.PlaySound2D("Enemy Attack");
                damageTimer = 0f;
            }
        }
        else
        {
            damageTimer = 0f;
        }
        TurnOffAnims();
        anim.SetBool("isAttacking", true);
        if (DistanceToPlayer() > agent.stoppingDistance + 2)
        {
            state = State.Chase;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.transform.position, attackDistance);
    }

    void DeadState()
    {
        //gameObject.SetActive(false);
    }

    float DistanceToPlayer()
    {
        return Vector3.Distance(player.transform.position, this.transform.position);
    }

    bool CanSeePlayer()
    {
        if (DistanceToPlayer() < VisualDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void TurnOffAnims()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isDying", false);
    }

}
