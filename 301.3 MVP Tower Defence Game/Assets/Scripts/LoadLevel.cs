using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    // This method can be called from button OnClick() with a specific level number
    public void LoadLevelByNumber(int levelNumber)
    {
        string sceneName = "Level " + levelNumber;

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene '" + sceneName + "' is not in the build settings!");
        }
    }
}