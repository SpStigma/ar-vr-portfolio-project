using UnityEngine;

public class EnemyBehavior : EnemyStats
{
    private Transform totem;
    private float attackCooldown;
    private Rigidbody rb;

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

    public void FindTotem()
    {
        GameObject totemObject = GameObject.FindGameObjectWithTag("Totem");

        if(totemObject != null)
        {
            totem = totemObject.transform;
        }
    }
}
