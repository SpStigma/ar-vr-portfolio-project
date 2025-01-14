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

        isPlacing = true;
        towerCost = cost;
        towerToPlace = towerPrefab;

        // Trouver le terrain pour le parentage
        terrain = GameObject.FindGameObjectWithTag(terrainTag);
        if (terrain == null)
        {
            Debug.LogError("Terrain introuvable ! Vérifiez que le tag est correctement configuré.");
            return;
        }

        // Créer une prévisualisation et définir le terrain comme parent
        previewTower = Instantiate(towerPrefab, terrain.transform);

        // Ajuster l'échelle pour qu'elle corresponde à celle du terrain
        Vector3 relativeScale = terrain.transform.localScale;
        previewTower.transform.localScale = Vector3.Scale(previewTower.transform.localScale, relativeScale);

        SetPreviewMaterial(previewTower);

        validateButton.gameObject.SetActive(true); // Activer le bouton de validation
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            Vector2 touchPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;

            // Convertir la position du doigt en un raycast
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayerMask))
            {
                // Déplacer la prévisualisation de la tour à l'endroit touché
                previewTower.transform.position = hit.point;

                // Ajuster la hauteur pour éviter que la tour soit partiellement sous le sol
                AdjustTowerHeight(previewTower, hit.point);
            }
        }
    }

    private void AdjustTowerHeight(GameObject tower, Vector3 position)
    {
        // Récupérer les bounds du modèle 3D pour déterminer sa hauteur
        Renderer renderer = tower.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            // Calculer la moitié de la hauteur de la tour
            float towerHeight = renderer.bounds.size.y / 2;

            // Appliquer un décalage vertical pour éviter que la tour soit immergée
            position.y += towerHeight;

            // Mettre à jour la position de la tour
            tower.transform.position = position;
        }
    }


    private void ValidatePlacement()
    {
        if (previewTower != null && terrain != null)
        {
            // Déduire le coût en or
            Parameters.goldCoin -= towerCost;

            // Finaliser la position de la tour et la définir comme enfant du terrain
            GameObject placedTower = Instantiate(towerToPlace, previewTower.transform.position, previewTower.transform.rotation, terrain.transform);

            Destroy(previewTower); // Supprimer la prévisualisation
            previewTower = null;

            isPlacing = false;
            validateButton.gameObject.SetActive(false); // Désactiver le bouton de validation
        }
    }

    private void SetPreviewMaterial(GameObject tower)
    {
        // Appliquer un matériau transparent ou une couleur pour la prévisualisation
        foreach (Renderer renderer in tower.GetComponentsInChildren<Renderer>())
        {
            Material previewMaterial = new Material(renderer.material);
            Color previewColor = previewMaterial.color;
            previewColor.a = 0.5f; // Rendre semi-transparent
            previewMaterial.color = previewColor;

            renderer.material = previewMaterial;
        }
    }
}
