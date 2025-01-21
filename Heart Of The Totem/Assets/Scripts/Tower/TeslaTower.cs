using UnityEngine;
using System.Collections;

public class TeslaTower : TowerStats
{
    [Header("Tesla Tower Specific Stats")]
    public float chainRange = 2f;
    public float chainDelay = 0.2f;

    private Transform target;
    private float attackCooldown = 0f;

    void Update()
    {
        if (target == null) 
        {
            FindTarget();
        }


        if (target != null && Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            if (attackCooldown <= 0f)
            {
                Attack(target);
                StartCoroutine(ChainAttack(target));
                attackCooldown = 1f / attackSpeed;
            }
        }

        attackCooldown -= Time.deltaTime;
    }

    private void FindTarget()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);

        foreach (var hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy"))
            {
                target = hit.transform;
                break;
            }
        }
    }

    private void Attack(Transform enemy)
    {
        if (enemy == null) return;

        EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
        if (enemyBehavior != null)
        {
            enemyBehavior.TakeDamage(damage);
            enemyBehavior.Freeze();
        }
    }

    private IEnumerator ChainAttack(Transform enemy)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange + chainRange);

        foreach (var hit in hitEnemies)
        {
            if (hit != null && hit.CompareTag("Enemy") && hit.transform != enemy)
            {
                EnemyBehavior enemyBehavior = hit.GetComponent<EnemyBehavior>();
                if (enemyBehavior != null)
                {
                    if (enemyBehavior.gameObject != null)
                    {
                        enemyBehavior.TakeDamage(damage);
                        enemyBehavior.Freeze();
                    }
                }
                yield return new WaitForSeconds(chainDelay);
            }
        }
    }
}
