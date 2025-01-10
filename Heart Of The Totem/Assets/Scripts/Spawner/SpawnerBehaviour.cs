using UnityEngine;

public class SpawnerBehavior : MonoBehaviour
{
    private GameObject terrain;
    public string tagName = "Terrain";
    private Transform[] spawnPoints;
    private Transform spawnpoint1, spawnpoint2;
    public WaveInfo[] waves;
    public int waveIndex = 0;

    public void Update()
    {
        terrain = GameObject.FindGameObjectWithTag(tagName);

        FindSpawnPoints();
        if(terrain != null && waveIndex < waves.Length)
        {
            for(int i = 0; i < waves[waveIndex].numberToSpawn; i++)
            {
                Vector3 spawnPosition = ChoseRandomSpawn();
                Instantiate(waves[waveIndex].enemyToSpawn, spawnPosition, Quaternion.identity, terrain.transform);
            }
            waveIndex++;
        }
        else
        {
            return;
        }
    }

    private void FindSpawnPoints()
    {
        Transform spawnPointsParent = terrain.transform.Find("SpawnPoint");
        if(spawnPointsParent != null && spawnPointsParent.childCount >= 2)
        {
            spawnpoint1 = spawnPointsParent.GetChild(0);
            spawnpoint2 = spawnPointsParent.GetChild(1);
        }
    }

    public Vector3 ChoseRandomSpawn()
    {
        if (spawnpoint1 == null || spawnpoint2 == null)
        {
            Debug.Log("Probleme sur les spawnpoints");
            return Vector3.zero;
        }

        Vector3 spawnPoint = Vector3.zero;

        bool spawnOnVerticalEdge = Random.Range(0f, 1f) > 0.5f;

        if (spawnOnVerticalEdge)
        {
            spawnPoint.z = Random.Range(spawnpoint1.position.z, spawnpoint2.position.z);
            spawnPoint.x = Random.Range(0f, 1f) > 0.5f ? spawnpoint1.position.x : spawnpoint2.position.x;
        }
        else
        {
            spawnPoint.x = Random.Range(spawnpoint1.position.x, spawnpoint2.position.x);
            spawnPoint.z = Random.Range(0f, 1f) > 0.5f ? spawnpoint1.position.z : spawnpoint2.position.z;
        }

        spawnPoint.y = spawnpoint1.position.y;

        return spawnPoint;
    }

}

[System.Serializable]
public class WaveInfo
{
    public GameObject enemyToSpawn;
    public int numberToSpawn;

} 