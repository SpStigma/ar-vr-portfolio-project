using UnityEngine;

public class TowerBasic :TowerStats
{
    private Transform target;

    private float attackCooldown;

    void Start()
    {
        attackCooldown = 1f / attackSpeed;
    }

    void Update()
    {
        FindTarget();

        if (target != null)
        {
            attackCooldown -= Time.deltaTime;

            if (attackCooldown <= 0f)
            {
                AttackTarget();
                attackCooldown = 1f / attackSpeed;
            }
        }
    }

    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                target = hit.transform;
                return;
            }
        }

        target = null;
    }

    void AttackTarget()
    {
        EnemyBehavior enemy = target.GetComponent<EnemyBehavior>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}