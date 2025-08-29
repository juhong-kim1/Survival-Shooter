using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using System.Collections;

public class Enemy : LivingEntity
{
    private Animator animator;
    private NavMeshAgent agent;
    private Collider collider;
    private Rigidbody rb;

    private Transform target;

    public LayerMask targetLayer;

    public float traceDistance = 10f;
    public float attackDistance = 1f;

    public float timer = 0f;
    public float attackInterval = 1f;

    public float damage = 10f;

    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die
    }

    private Status currentStatus;

    public Status CurrentStatus
    {
        get { return currentStatus; }
        set
        {
            var prevStatus = currentStatus;
            currentStatus = value;

            switch (CurrentStatus)
            {
                case Status.Idle:
                    animator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Trace:
                    animator.SetBool("HasTarget", true);
                    agent.isStopped = false;
                    break;
                case Status.Attack:
                    animator.SetBool("HasTarget", false);
                    agent.isStopped = true;
                    break;
                case Status.Die:
                    animator.SetTrigger("Die");
                    agent.isStopped = true;
                    break;
            }
        }
    }


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }


    protected override void OnEnable()
    {
        base.OnEnable();

        collider.enabled = true;
        CurrentStatus = Status.Idle;
    }

    private void Update()
    {
        switch (CurrentStatus)
        {
            case Status.Idle:
                UpdateIdleStatus();
                break;
            case Status.Trace:
                UpdateTraceStatus();
                break;
            case Status.Attack:
                UpdateAttackStatus();
                break;
            case Status.Die:
                UpdateDieStatus();
                break;
        }

    }

    public void UpdateIdleStatus()
    {
        if (target != null &&
            Vector3.Distance(transform.position, target.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
        }

        target = FindTarget(traceDistance);
    }

    public void UpdateTraceStatus()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }
        if (target == null || Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            CurrentStatus = Status.Idle;
            return;
        }

        agent.SetDestination(target.position);
    }

    public void UpdateAttackStatus()
    {
        if (target == null)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        if (target != null || Vector3.Distance(transform.position, target.position) > attackDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        var lookAt = target.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        timer += Time.deltaTime;

        if (timer >= attackInterval)
        {
            var damagable = target.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.OnDamage(damage, target.position, -target.forward);
            }

            timer = 0f;
        }

    }

    public void UpdateDieStatus()
    {

    }

    protected override void Die()
    {
        base.Die();

        CurrentStatus = Status.Die;

    }

    protected Transform FindTarget(float radius)
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, targetLayer.value);
        if (colliders.Length == 0)
        {
            return null;
        }

        var target = colliders.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();

        return target.transform;
    }

    public void StartSinking()
    {
        collider.enabled = false;
        rb.isKinematic = false;
        agent.enabled = false;
    }


}
