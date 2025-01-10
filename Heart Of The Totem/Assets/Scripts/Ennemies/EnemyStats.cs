using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Base Enemy Stats")]
    public float health = 100f;
    public float attackSpeed = 1f;
    public float damage = 10f;
    public float attackRange = 1f * Parameters.objectScale.magnitude;
    public float coin = 2f;

    public virtual void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log($"{gameObject.name} est mort.");
        Destroy(gameObject);
    }
}
