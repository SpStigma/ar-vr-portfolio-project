using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpawnerBehavior : MonoBehaviour
{
    private GameObject terrain;
    public string tagName = "Terrain";
    private Transform[] spawnPoints;
    private Transform spawnpoint1, spawnpoint2;

    public WaveInfo[] waves;
    private int waveIndex = 0;

    public Button nextWaveButton;
    private bool isWaveInProgress = false;

    private int activeEnemies = 0;

    void Start()
    {
        if (nextWaveButton != null)
        {
            nextWaveButton.onClick.AddListener(StartNextWave);
            nextWaveButton.gameObject.SetActive(false);
        }
        
        terrain = GameObject.FindGameObjectWithTag(tagName);
        FindSpawnPoints();
        StartNextWave();
    }

    void Update()
    {
        terrain = GameObject.FindGameObjectWithTag(tagName);
        FindSpawnPoints();

        if (isWaveInProgress && activeEnemies == 0)
        {
            isWaveInProgress = false;
            if (nextWaveButton != null)
            {
                nextWaveButton.gameObject.SetActive(true);
            }
        }
    }

    public void StartNextWave()
    {
        if (terrain == null || waveIndex >= waves.Length)
            return;

        if (nextWaveButton != null)
        {
            nextWaveButton.gameObject.SetActive(false);
        }

        isWaveInProgress = true;
        StartCoroutine(SpawnWave(waves[waveIndex]));
        waveIndex++;
    }

    private IEnumerator SpawnWave(WaveInfo wave)
    {
        foreach (var enemyInfo in wave.enemies)
        {
            for (int i = 0; i < enemyInfo.numberToSpawn; i++)
            {
                Vector3 spawnPosition = ChoseRandomSpawn();
                GameObject enemy = Instantiate(enemyInfo.enemyPrefab, spawnPosition, Quaternion.identity, terrain.transform);
                activeEnemies++;

                EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.OnDie += HandleEnemyDeath;
                }

                yield return new WaitForSeconds(wave.spawnDelay);
            }
        }
    }

    private void HandleEnemyDeath()
    {
        activeEnemies--;
    }

    private void FindSpawnPoints()
    {
        if (terrain == null)
            return;

        Transform spawnPointsParent = terrain.transform.Find("SpawnPoint");
        if (spawnPointsParent != null && spawnPointsParent.childCount >= 2)
        {
            spawnpoint1 = spawnPointsParent.GetChild(0);
            spawnpoint2 = spawnPointsParent.GetChild(1);
        }
    }

    public Vector3 ChoseRandomSpawn()
    {
        if (spawnpoint1 == null || spawnpoint2 == null)
        {
            Debug.LogError("Spawn points are not properly set.");
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
    public EnemyInfo[] enemies;
    public float spawnDelay = 1f;
}

[System.Serializable]
public class EnemyInfo
{
    public GameObject enemyPrefab;
    public int numberToSpawn;
}
