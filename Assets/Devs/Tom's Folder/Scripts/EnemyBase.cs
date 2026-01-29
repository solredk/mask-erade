using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    public NavMeshAgent agent;

    [SerializeField] private Transform player;

    public LayerMask whatIsGround;

    [SerializeField] public LayerMask WhatIsPlayer;

    //Patrolling

    public Vector3 WalkPoint;
    public bool WalkPointSet;
    public float WalkPointRange;

    //Attacking
    public float TimeBetweenAttacks;
    bool AlreadyAttacked;

    //States
    public float SightRange;
    public float AttackRange;
    public bool PlayerInSightRange;
    public bool PlayerInSightCheck;
    public bool PlayerInAttackRange;


    private float SaveAgentSpeed;

    //

    public Collider[] CheckPlayer;
    public GameObject Player;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    private void Start()
    {
        SaveAgentSpeed = agent.speed;

    }

    public virtual void Patrolling()
    {
        if (!WalkPointSet) StartCoroutine(WalkCoolDown());

        if (WalkPointSet)
        {
            agent.SetDestination(WalkPoint);
        }

        Vector3 distanceToWalkpoint = transform.position - WalkPoint;


        //Walkpoint reached
        if (distanceToWalkpoint.magnitude < 1f)
        {
            WalkPointSet = false;
        }

    }

    public virtual void CheckSurroundings()
    {

        CheckPlayer = Physics.OverlapSphere(transform.position, SightRange, WhatIsPlayer);
        PlayerInSightRange = Physics.CheckSphere(transform.position, SightRange, WhatIsPlayer);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, WhatIsPlayer);

        if(CheckPlayer != null)
        {
            for (int i = 0; i < CheckPlayer.Length; i++)
            {
                Player = CheckPlayer[i].gameObject;
            }
        }

        if (!PlayerInSightRange && !PlayerInAttackRange) Patrolling();
        if (PlayerInSightRange && !PlayerInAttackRange) ChasePlayer();
        if (PlayerInSightRange && PlayerInAttackRange) AttackPlayer();

    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        if (WalkPointSet == false)
        {
            float RandomZ = Random.Range(-WalkPointRange, WalkPointRange);
            float RandomX = Random.Range(-WalkPointRange, WalkPointRange);

            WalkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

            if (Physics.Raycast(WalkPoint, -transform.up, 2f, whatIsGround))
            {
                WalkPointSet = true;
            }
        }

    }

    public virtual void ChasePlayer()
    {
        if (agent.enabled == false)
        {
            agent.enabled = true;
        }

        if (Player != null)
        {
            agent.SetDestination(Player.gameObject.transform.position);
        }

    }

    public virtual void AttackPlayer()
    {

    }
    private void ShootPlayer()
    {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (!AlreadyAttacked)
        {

            AlreadyAttacked = true;
            Invoke(nameof(ResetAttack), 4);

        }

    }

    public void ResetAttack()
    {
        AlreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SightRange);

    }

    public IEnumerator WalkCoolDown()
    {
        yield return new WaitForSeconds(1f);

        SearchWalkPoint();
    }



    private void OnCollisionEnter(Collision collision)
    {

    }
}
