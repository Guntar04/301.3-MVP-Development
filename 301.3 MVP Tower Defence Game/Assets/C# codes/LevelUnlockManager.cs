using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUnlockManager : MonoBehaviour
{
    public GameObject LevelFlagsPrefab;
    public Transform FlagParent;
    public int totalLevels = 6;

    void Start()
    {
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);

        for (int i = 1; i <= unlockedLevels && i <= totalLevels; i++)
        { 
         SpawnFlag(i);
        }
    }

    void SpawnFlag(int LevelNumber)
    {
        GameObject flag = Instantiate(LevelFlagsPrefab, FlagParent);
        flag.GetComponentInChildren<Text>().text = "Level 1" + LevelNumber;
        flag.GetComponent<Button>().onClick.AddListener(() => LoadLevel(LevelNumber));
    }



    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene("Level 1" + levelNumber);
    }

    public static void UnlockNextLevel()
    {
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (unlockedLevels < totalScenes)
        {
            unlockedLevels++;
            PlayerPrefs.SetInt("UnlockedLevels", unlockedLevels);
            PlayerPrefs.Save();
        }

    
    }

    public void RestProgrees()
    {
        PlayerPrefs.SetInt("UnlockedLevels", 1);
        PlayerPrefs.Save();
    }
    

}

