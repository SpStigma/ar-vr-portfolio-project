using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private GameObject totem;
    public GameObject defeatPanel;

    public void Start()
    {
        totem = GameObject.FindGameObjectWithTag("Totem");
    }

    public void Update()
    {
        if(totem == null)
        {
            defeatPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResetParameters()
    {
        Parameters.goldCoin = 0;
    }
}
