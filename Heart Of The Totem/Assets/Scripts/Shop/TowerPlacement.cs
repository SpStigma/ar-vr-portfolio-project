using UnityEngine;
using UnityEngine.UI;

public class TowerPlacementManager : MonoBehaviour
{
    public static TowerPlacementManager Instance;   // Singleton
    public LayerMask placementLayerMask;            // Couches valides pour le placement
    public Button validateButton;                   // Bouton pour valider le placement
    public string terrainTag = "Terrain";           // Tag du terrain

    private GameObject previewTower;                // Prévisualisation de la tour
    private GameObject towerToPlace;                // Tour à placer
    private GameObject terrain;                     // Terrain détecté
    private int towerCost;                          // Coût de la tour à placer
    private bool isPlacing = false;                 // Indique si le mode placement est actif

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        validateButton.gameObject.SetActive(false); // Désactiver le bouton de validation au départ
        validateButton.onClick.AddListener(ValidatePlacement);
    }

    void Update()
    {
        if (isPlacing)
        {
            HandleTouchInput();
        }
    }

    public void StartPlacement(GameObject towerPrefab, int cost)
    {
        if (Parameters.goldCoin < cost)
        {
            Debug.Log("Pas assez d'or !");
            return;
        }

        if(previewTower != null)
        {
            Destroy(previewTower);
        }

        isPlacing = true;
        towerCost = cost;
        towerToPlace = towerPrefab;

        // Trouver le terrain pour le parentage
        terrain = GameObject.FindGameObjectWithTag(terrainTag);
        if (terrain == null)
        {
            return;
        }

        previewTower = Instantiate(towerPrefab, terrain.transform);

        Vector3 relativeScale = terrain.transform.localScale;
        previewTower.transform.localScale = Vector3.Scale(previewTower.transform.localScale, relativeScale);

        SetPreviewMaterial(previewTower);

        validateButton.gameObject.SetActive(true);
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            Vector2 touchPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayerMask))
            {
                previewTower.transform.position = hit.point;

                AdjustTowerHeight(previewTower, hit.point);
            }
        }
    }

    private void AdjustTowerHeight(GameObject tower, Vector3 position)
    {
        Renderer renderer = tower.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            float towerHeight = renderer.bounds.size.y / 2;

            position.y += towerHeight;

            tower.transform.position = position;
        }
    }


    private void ValidatePlacement()
    {
        if (previewTower != null && terrain != null)
        {
            Parameters.goldCoin -= towerCost;

            GameObject placedTower = Instantiate(towerToPlace, previewTower.transform.position, previewTower.transform.rotation, terrain.transform);

            Destroy(previewTower);
            previewTower = null;

            isPlacing = false;
            validateButton.gameObject.SetActive(false);
        }
    }

    private void SetPreviewMaterial(GameObject tower)
    {
        foreach (Renderer renderer in tower.GetComponentsInChildren<Renderer>())
        {
            Material previewMaterial = new Material(renderer.material);
            Color previewColor = previewMaterial.color;
            previewColor.a = 0.5f;
            previewMaterial.color = previewColor;

            renderer.material = previewMaterial;
        }
    }
}
