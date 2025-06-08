using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUnlockManager : MonoBehaviour
{
    public GameObject levelFlagPrefab;         // Assign your level flag prefab in Inspector
    public Transform flagParent;               // Assign a UI layout group or empty parent in Inspector
    public GameObject levelCompleteBanner;     // Assign the level complete UI banner in Inspector
    public int totalLevels = 6;

    void Start()
    {
        // Only spawn flags in the LevelSelect scene
        if (SceneManager.GetActiveScene().name == "Level select screen")
        {
            SpawnLevelFlags();
        }

        // Hide the banner at the start
        if (levelCompleteBanner != null)
            levelCompleteBanner.SetActive(false);
    }

    void SpawnLevelFlags()
    {
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);

        for (int i = 1; i <= unlockedLevels && i <= totalLevels; i++)
        {
            GameObject flag = Instantiate(levelFlagPrefab, flagParent);
            flag.GetComponentInChildren<Text>().text = "Level " + i;
            int levelToLoad = i; // local copy for lambda
            flag.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelToLoad));
        }
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

        if (unlockedLevels < SceneManager.sceneCountInBuildSettings)
        {
            unlockedLevels++;
            PlayerPrefs.SetInt("UnlockedLevels", unlockedLevels);
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
