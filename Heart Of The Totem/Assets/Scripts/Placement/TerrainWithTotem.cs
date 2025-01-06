using UnityEngine;

public class TerrainWithTotem : MonoBehaviour
{
    public GameObject totemPrefab;
    private GameObject totemInstance;

    void Start()
    {
        PlaceTotemAtCenter();
    }

    private void PlaceTotemAtCenter()
    {
        if (totemPrefab == null)
        {
            return;
        }

        Vector3 terrainCenter = GetComponent<Renderer>().bounds.center;
        Vector3 terrainSize = GetComponent<Renderer>().bounds.size;

        Vector3 totemPosition = new Vector3(terrainCenter.x, terrainCenter.y + terrainSize.y / 2 + 1f, terrainCenter.z);

        totemInstance = Instantiate(totemPrefab, totemPosition, Quaternion.identity, transform);
    }
}

