using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;

    private Transform target;

    public void SetTarget(Transform newTarget, int newDamage)
    {
        target = newTarget;
        damage = newDamage;
    }

    public void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if(Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            HitTarget();
        }
    }


    public void HitTarget()
    {
        EnemyBehavior enemy = target.GetComponent<EnemyBehavior>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
