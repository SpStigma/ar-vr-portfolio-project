using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpawnerBehavior : MonoBehaviour
{
    private GameObject terrain;
    public string tagName = "Terrain";
    private Transform spawnpoint1, spawnpoint2;

    public WaveInfo[] waves;
    private int waveIndex = 0;

    public Button nextWaveButton;
    public GameObject panelShop;
    public GameObject victoryPanel;

    private bool isWaveInProgress = false;
    private int activeEnemies = 0;
    private int enemiesRemainingToSpawn = 0;

    void Start()
    {

        if (nextWaveButton != null)
        {
            nextWaveButton.onClick.AddListener(StartNextWave);
        }

        terrain = GameObject.FindGameObjectWithTag(tagName);
        if (terrain == null)
        {
            Debug.LogError("Terrain not found! Check your tag name.");
        }
        else
        {
            FindSpawnPoints();
            StartNextWave();
        }
    }

    void Update()
    {

        if (isWaveInProgress && enemiesRemainingToSpawn == 0 && activeEnemies == 0)
        {

            isWaveInProgress = false;

            if (waveIndex >= waves.Length)
            {
                if (victoryPanel != null)
                {
                    victoryPanel.SetActive(true);
                }
            }
            else
            {
                if (panelShop != null)
                {
                    panelShop.SetActive(true);
                }
            }
        }
    }

    public void StartNextWave()
    {
        if (terrain == null || waveIndex >= waves.Length || isWaveInProgress)
        {
            return;
        }

        isWaveInProgress = true;
        enemiesRemainingToSpawn = 0;

        foreach (var enemyInfo in waves[waveIndex].enemies)
        {
            enemiesRemainingToSpawn += enemyInfo.numberToSpawn;
        }

        Debug.Log($"Starting wave {waveIndex + 1}. Enemies to spawn: {enemiesRemainingToSpawn}");

        if (panelShop != null)
        {
            panelShop.SetActive(false);
            Debug.Log("Shop panel deactivated.");
        }

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
                enemiesRemainingToSpawn--;


                EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.OnDie += HandleEnemyDeath;
                }

                yield return new WaitForSeconds(wave.spawnDelay);
            }
        }

        Debug.Log("Wave spawning completed.");
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
        else
        {
            Debug.LogError("Not enough spawn points found under Terrain.");
        }
    }

    public Vector3 ChoseRandomSpawn()
    {
        Vector3 spawnPoint = Vector3.zero;
        
        // Déterminer s'il faut spawn sur le bord vertical ou horizontal
        bool spawnOnVerticalEdge = Random.Range(0f, 1f) > .5f;
        
        if (spawnOnVerticalEdge)
        {
            // Choisir aléatoirement la coordonnée z dans l'intervalle entre les spawnpoints
            spawnPoint.z = Random.Range(spawnpoint1.position.z, spawnpoint2.position.z);
            
            // Choisir aléatoirement de spawn à la position max ou min en x
            if (Random.Range(0f, 1f) > .5f)
            {
                spawnPoint.x = spawnpoint1.position.x;
            }
            else
            {
                spawnPoint.x = spawnpoint2.position.x;
            }
        }
        else
        {
            // Choisir aléatoirement la coordonnée x dans l'intervalle entre les spawnpoints
            spawnPoint.x = Random.Range(spawnpoint1.position.x, spawnpoint2.position.x);
            
            // Choisir aléatoirement de spawn à la position max ou min en z
            if (Random.Range(0f, 1f) > .5f)
            {
                spawnPoint.z = spawnpoint1.position.z;
            }
            else
            {
                spawnPoint.z = spawnpoint2.position.z;
            }
        }

        // Fixer la position y à la valeur minSpawn.position.y ou ajuster si nécessaire
        spawnPoint.y = spawnpoint1.position.y;  // Ou, si nécessaire, randomiser en y également

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
