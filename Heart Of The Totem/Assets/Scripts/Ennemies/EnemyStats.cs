using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Base Enemy Stats")]
    public float health = 100f;
    public float attackSpeed = 1f;
    public float damage = 10f;
    public float attackRange = 1f;
    public int coin = 2;

    public event System.Action OnDie;

    private bool isDead = false;

    public virtual void TakeDamage(float amount)
    {
        if (isDead)
        {
            return;
        }

        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;


        if (OnDie != null)
        {
            OnDie.Invoke();
        }

        Parameters.goldCoin += coin;
        
        Destroy(gameObject, 0.1f);
    }
}
