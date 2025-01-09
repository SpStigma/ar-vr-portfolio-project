using UnityEngine;

public class MoveTowardsTarget : MonoBehaviour
{
    public string targetTag = "Totem";
    public float speed = 5f;

    private GameObject totem;

    public void Start()
    {
        totem = GameObject.FindGameObjectWithTag(targetTag);
    }

    void Update()
    {
        totem = GameObject.FindGameObjectWithTag(targetTag);
        if (totem != null)
        {
            // Faire regarder le GameObject vers la cible
            Vector3 direction = (totem.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, speed * Time.deltaTime);

            // Déplacement vers la cible
            transform.position = Vector3.MoveTowards(
                transform.position, 
                totem.transform.position, 
                speed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, totem.transform.position) < 0.1f)
            {
                Debug.Log("Cible atteinte !");
            }
        }
        else
        {
            return;
        }
    }
}
