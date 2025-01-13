using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    void Update()
    {
        UpdateGoldUI();
    }

    public void UpdateGoldUI()
    {
        goldText.text = $"Gold: {Parameters.goldCoin}";
    }
}
