using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    private Animator animator;
    private NavMeshAgent agent;
    private Collider collider;
    private Rigidbody rb;
    public GameManager manager;

    private AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip deathSound;

    public GameObject player;

    private Transform target;
    public ParticleSystem hitParticles;

    public LayerMask targetLayer;

    public float traceDistance = 20f;
    public float attackDistance = 1f;

    public float timer = 0f;
    public float attackInterval = 0.5f;

    public float damage = 10f;
    public float moveSpeed = 3.5f;
    public float health = 100;

    private bool hasFirstAttack = false;

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
        audioSource = GetComponent<AudioSource>();

        agent.speed = moveSpeed;
        Health = health;
        hasFirstAttack = false;
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

        if (target != null && Vector3.Distance(transform.position, target.position) > attackDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        var lookAt = target.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        timer += Time.deltaTime;

        if (!hasFirstAttack || timer >= attackInterval)
        {

            var damagable = target.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.OnDamage(damage, target.position, -target.forward);
            }

            timer = 0f;

            hasFirstAttack = true;

        }

    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
            return;

        base.OnDamage(damage, hitPoint, hitNormal);

        audioSource.PlayOneShot(hurtSound, SoundManager.Instance.sfxVolume);

        hitParticles.transform.position = hitPoint;
        hitParticles.Play();
    }

    protected override void Die()
    {
        base.Die();

        CurrentStatus = Status.Die;

        GameObject gameControllerObj = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObj != null)
        {
            GameManager gameManager = gameControllerObj.GetComponent<GameManager>();
            gameManager.AddScore(100);
        }

        audioSource.PlayOneShot(deathSound, SoundManager.Instance.sfxVolume);
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
