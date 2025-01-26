using UnityEngine;

public class MoveTowardsTarget : MonoBehaviour
{
    public string targetTag = "Totem"; // Tag de la cible
    public float baseSpeed = 2.0f;     // Vitesse de base
    public float speedMultiplier = 0.5f; // Multiplicateur ajustable dans l'inspecteur

    private GameObject totem;
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        totem = GameObject.FindGameObjectWithTag(targetTag);
        if (totem != null)
        {
            Vector3 direction = (totem.transform.position - transform.position).normalized;

            direction.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, baseSpeed * Time.deltaTime);

            float adjustedSpeed = baseSpeed * speedMultiplier;

            transform.position = Vector3.MoveTowards(
                transform.position,
                totem.transform.position,
                adjustedSpeed * Time.deltaTime
            );

            animator.SetBool("isRunning", true);
        }
    }
}
