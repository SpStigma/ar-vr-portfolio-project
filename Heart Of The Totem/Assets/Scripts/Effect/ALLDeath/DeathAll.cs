using UnityEngine;
using System.Collections;

public class DeathAll : MonoBehaviour
{
    public string targetTag = "Enemy";

    public void OnDestroyButtonClicked()
    {
        StartCoroutine(DestroyAllEntitiesWithDelay());
    }

    private IEnumerator DestroyAllEntitiesWithDelay()
    {
        yield return new WaitForSeconds(1f);

        GameObject[] entities = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject entity in entities)
        {
            var enemyBehavior = entity.GetComponent<EnemyBehavior>();

            if(enemyBehavior != null)
            {
                enemyBehavior.TakeDamage(1000);
            }
        }
        Parameters.goldCoin += 10;
    }
}
