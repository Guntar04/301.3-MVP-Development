using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUnlockManager : MonoBehaviour
{
    public Button[] levelFlagButtons;           // Drag LevelFlags 1-6 here
    public GameObject levelCompleteBanner;      // Drag LevelCompleteBanner here
    public int totalLevels = 6;

    void Start()
    {
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);

        // Enable buttons only for unlocked levels
        for (int i = 0; i < levelFlagButtons.Length; i++)
        {
            int levelIndex = i + 1;
            Button button = levelFlagButtons[i];

            if (levelIndex <= unlockedLevels)
            {
                button.interactable = true;
                int copy = levelIndex; // Needed for lambda capture
                button.onClick.AddListener(() => LoadLevel(copy));
            }
            else
            {
                button.interactable = false;
            }
        }

        // Hide banner initially
        if (levelCompleteBanner != null)
            levelCompleteBanner.SetActive(false);
    }

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

    public static void UnlockNextLevel()
    {
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);
        int totalLevels = SceneManager.sceneCountInBuildSettings;

        if (unlockedLevels < totalLevels)
        {
            PlayerPrefs.SetInt("UnlockedLevels", unlockedLevels + 1);
            PlayerPrefs.Save();
        }
    }

    public void ShowLevelCompleteBanner()
    {
        if (levelCompleteBanner != null)
        {
            levelCompleteBanner.SetActive(true);
        }
    }

    public void OnContinueToMap()
    {
        UnlockNextLevel();
        SceneManager.LoadScene("Level select screen");
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("UnlockedLevels", 1);
        PlayerPrefs.Save();
    }
}
