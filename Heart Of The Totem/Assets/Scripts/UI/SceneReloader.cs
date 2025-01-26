using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public void ReloadScene()
    {
        Scene actualScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(actualScene.name);
        Time.timeScale = 1;
    }
}
