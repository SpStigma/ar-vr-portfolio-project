using UnityEngine;

public class EnemyBehavior : EnemyStats
{
    private Transform totem;
    private float attackCooldown;
    private Rigidbody rb;
    private bool isFrozen = false;
    private float freezeDuration = .5f;
    private float freezeTimer = 0f;

    void Start()
    {
        attackCooldown = attackSpeed;
        rb = GetComponent<Rigidbody>();
        attackRange *= Parameters.objectScale.magnitude;
        FindTotem();
    }

    void Update()
    {
        if (totem == null)
        {
            FindTotem();
            return;
        }

        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0f)
            {
                isFrozen = false;
                KeepGoing();

            }
        }

        float distanceToTotem = Vector3.Distance(transform.position, totem.position);

        if (distanceToTotem <= attackRange)
        {
            AttackTotem();
            StopEnemy();
        }
    }

    void AttackTotem()
    {
        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0f)
        {
            attackCooldown = attackSpeed;

            TotemBehavior totemBehavior = totem.GetComponent<TotemBehavior>();
            if (totemBehavior != null)
            {
                totemBehavior.TakeDamage(damage);
            }
        }
    }

    public void StopEnemy()
    {
        MoveTowardsTarget mv = GetComponent<MoveTowardsTarget>();
        mv.enabled = false;
    }

    public void KeepGoing()
    {
        MoveTowardsTarget mv = GetComponent<MoveTowardsTarget>();
        mv.enabled = true;
    }

    public void FindTotem()
    {
        GameObject totemObject = GameObject.FindGameObjectWithTag("Totem");

        if(totemObject != null)
        {
            totem = totemObject.transform;
        }
    }

    public void Freeze()
    {
        isFrozen = true;
        freezeTimer = freezeDuration;
        StopEnemy();
    }
}
