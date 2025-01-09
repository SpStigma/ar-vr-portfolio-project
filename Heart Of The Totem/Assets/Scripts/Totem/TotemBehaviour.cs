using UnityEngine;

public class TotemBehavior : MonoBehaviour
{
    [Header("Totem Stats")]
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"Le totem a reçu {amount} dégâts. Santé restante : {currentHealth}");

        if (currentHealth <= 0f)
        {
            DestroyTotem();
        }
    }

    void DestroyTotem()
    {
        Destroy(gameObject);
    }
}
