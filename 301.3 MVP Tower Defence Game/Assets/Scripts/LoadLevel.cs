using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUnlockManager : MonoBehaviour
{
    public void LoadLevel(int levelNumber)
    {
        string sceneName = "Level " + levelNumber;

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene " + sceneName + " not found in build settings!");
        }
    }
}