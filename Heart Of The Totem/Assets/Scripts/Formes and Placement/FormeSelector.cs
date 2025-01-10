using System.Collections.Generic;
using UnityEngine;

public class FormeSelector : MonoBehaviour
{
    public List<GameObject> spawnableFormes;
    private GameObject selectedForme;

    public GameObject GetSelectedForme()
    {
        return selectedForme;
    }

    public void SelectForme(int formeIndex)
    {
        if (formeIndex >= 0 && formeIndex < spawnableFormes.Count)
        {
            selectedForme = spawnableFormes[formeIndex];
            Debug.Log($"Forme sélectionnée : {selectedForme.name}");
        }
        else
        {
            Debug.Log("Index de forme invalide !");
        }
    }
}
