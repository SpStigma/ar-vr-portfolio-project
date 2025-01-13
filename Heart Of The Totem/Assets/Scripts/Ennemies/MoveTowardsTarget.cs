using UnityEngine;

public class MoveTowardsTarget : MonoBehaviour
{
    public string targetTag = "Totem";
    public float speed = 5f;

    private GameObject totem;

    void Update()
    {
        totem = GameObject.FindGameObjectWithTag(targetTag);
        if (totem != null)
        {
            Vector3 direction = (totem.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, speed * Time.deltaTime);

            transform.position = Vector3.MoveTowards(
                transform.position, 
                totem.transform.position, 
                speed  * Parameters.objectScale.magnitude  * Time.deltaTime
            );
        }
        else
        {
            return;
        }
    }
}
