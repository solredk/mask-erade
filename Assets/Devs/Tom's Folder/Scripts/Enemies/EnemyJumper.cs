using System.Collections;
using System.ComponentModel;
using DG.Tweening;
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
            if(agent.enabled == true)
            {
                agent.SetDestination(Player.transform.position);
            }
        }

    }

    public override void AttackPlayer()
    {
        if (agent.enabled == true)
        {
            agent.enabled = false;
        }
        if (WalkPointSet == true && WalkPointReset != 0)
        {
            WalkPointReset = 0;
            WalkPointResetEnable = false;
        }

        if (M_isGrounded)
        {
            StartCoroutine(LookAt());
        }

        if (Jumped == false && JumpCoolDown <= 0 && M_isGrounded && agent.enabled == false)
        {
            StartCoroutine(JumpMovement(0.1f));
        }

    }

    private IEnumerator JumpMovement(float CoolDown)
    {
        yield return new WaitForSeconds(CoolDown);
        if (Jumped == false && JumpCoolDown <= 0 && M_isGrounded)
        {
            AudioManager.instance.Play("ScreamJump", 1);
            Jumped = true;
            JumpCoolDown = 1.5f;
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 300);
            GetComponent<Rigidbody>().AddForce(Vector3.up * 200);
            WalkPointSet = false;
            RaycastHitRange = RayCastHitRangeAmount;
        }
    }
    public override void Patrolling()
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
        if (JumpCoolDownReset == false)
        {
            JumpCoolDownReset = true;
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

        LookRotation.x = transform.rotation.x;
        LookRotation.z = transform.rotation.z;
        //LookRotation.eulerAngles = new Vector3(transform.rotation.x, LookRotation.y, transform.rotation.z);

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, time);
            time += Time.deltaTime * Speed;

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "WhatIsGround" && RaycastHitRange != 0.80f)
        {
            AudioManager.instance.Play("SlamGround", 0.5f);
            RaycastHitRange = 0.80f;
            StartCoroutine(JumpCoolDownCounter(0.5f));
        }
    }

    private IEnumerator JumpCoolDownCounter(float CoolDown)
    {
        yield return new WaitForSeconds(0.5f);
        JumpCoolDown = 0;
        if (JumpCoolDown <= 0 && Jumped == true)
        {
            Jumped = false;
        }
        yield return null;
    }
}
