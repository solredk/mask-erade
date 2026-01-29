using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyJumper : EnemyBase
{
    private RaycastHit Hit;
    [SerializeField] private float Speed;
    private bool Jumped;
    private float JumpCoolDown;
    public bool M_isGrounded;
    private float RaycastHitRange = 0.13f;
    public float RayCastHitRangeAmount = 0;

    private bool JumpCoolDownReset = true;

    [SerializeField] private Collider Jumper;

    private float WalkPointReset;

    private bool WalkPointResetEnable;

    private void Start()
    {
        RaycastHitRange = 0.80f;
    }

    private void Update()
    {
        CheckSurroundings();


        Vector3 _dirDown = (transform.position - transform.up) - transform.position;
        _dirDown.Normalize();
        Ray ray = new Ray(transform.position, _dirDown);
        if (Physics.Raycast(ray, out Hit, RaycastHitRange, whatIsGround))
        {
            M_isGrounded = true;
        }
        else
        {
            M_isGrounded = false;
        }

        Debug.DrawRay(ray.origin, ray.direction * RaycastHitRange, Color.red);

        if (JumpCoolDown > 0)
        {
            JumpCoolDown -= Time.deltaTime;
        }
        if (JumpCoolDown < 0 && Jumped == true)
        {
            Jumped = false;
            //AudioManager.instance.gameObject.GetComponent<AudioSource>().volume = 0.50f;
            //AudioManager.instance.Play("SharkPound");
        }

        if (Jumped == true && JumpCoolDown > 0 && M_isGrounded == false)
        {
            RaycastHitRange = 0.80f;
        }

        if (WalkPointReset > 0)
        {
            WalkPointReset -= Time.deltaTime;
        }

    }

    public override void ChasePlayer()
    {
        if (agent.enabled == false && M_isGrounded)
        {
            agent.enabled = true;
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }

        if (Jumped == true)
        {
            Jumped = false;
            JumpCoolDown = 0f;
        }

        if (Player != null)
        {
            agent.SetDestination(Player.gameObject.transform.position);
        }

    }

    public override void AttackPlayer()
    {
        if (WalkPointSet == true && WalkPointReset != 0)
        {
            WalkPointReset = 0;
            WalkPointResetEnable = false;
        }

        if (Jumper.enabled == false)
        {
            Jumper.enabled = true;
        }

        if (M_isGrounded)
        {
            StartCoroutine(LookAt());
            //animator.SetBool("Attack", false);
        }

        //if (Jumped && M_isGrounded)
        //{
        //    StartCoroutine(JumpCoolDownCounter());
        //}
        if (Jumped == false && JumpCoolDown <= 0 && M_isGrounded)
        {
            agent.enabled = false;
            WalkPointSet = false;
            RaycastHitRange = RayCastHitRangeAmount;
            gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 300);
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 200f);
            Jumped = true;
            JumpCoolDown = 1.5f;
        }

    }

    public override void Patrolling()
    {
        if (Jumped == true)
        {
            Jumped = false;
            JumpCoolDown = 0f;
        }

        if (Jumper.enabled == true)
        {
            Jumper.enabled = false;
        }

        if (JumpCoolDownReset == false)
        {
            JumpCoolDownReset = true;
        }

        if (agent.enabled == false && M_isGrounded)
        {
            agent.enabled = true;
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
        if (!WalkPointSet) StartCoroutine(WalkCoolDown());

        if (WalkPointSet)
        {
            agent.SetDestination(WalkPoint);
        }

        if (WalkPointSet == true && WalkPointReset <= 0 && WalkPointResetEnable == false)
        {
            WalkPointResetEnable = true;
            WalkPointReset = 5f;
        }

        if (WalkPointSet == true && WalkPointReset <= 0 && WalkPointResetEnable == true)
        {
            WalkPointSet = false;
            WalkPointResetEnable = false;
        }



        Vector3 distanceToWalkpoint = transform.position - WalkPoint;


        //Walkpoint reached
        if (distanceToWalkpoint.magnitude < 1f)
        {
            WalkPointSet = false;
            WalkPointReset = 0;
            WalkPointResetEnable = false;
        }

    }
    private IEnumerator LookAt()
    {
        Quaternion LookRotation = Quaternion.LookRotation(Player.transform.position - transform.position);

        float time = 0;

        while (time < 1)
        {
            Debug.Log("Work");
            transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, time);

            time += Time.deltaTime * Speed;

            yield return null;
        }
    }
}
