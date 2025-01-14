using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerShop : MonoBehaviour
{
    public GameObject towerPrefab;
    public int towerCost = 5;
    public TextMeshProUGUI costText;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);

        if (costText != null)
        {
            costText.text = $"{towerCost} Gold";
        }
    }

    void OnButtonClick()
    {
        TowerPlacementManager.Instance.StartPlacement(towerPrefab, towerCost);
    }
}
