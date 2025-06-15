using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelUnlockManager : MonoBehaviour
{
    public Button[] levelFlagButtons;
    public GameObject levelCompleteBanner;
    public int totalLevels = 6;

    void Start()
    {
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);

        for (int i = 0; i < levelFlagButtons.Length; i++)
        {
            int levelIndex = i + 1;
            Button button = levelFlagButtons[i];

            if (levelIndex <= unlockedLevels)
            {
                button.interactable = true;

                // Animate only the newest unlocked level
                if (levelIndex == unlockedLevels)
                {
                    StartCoroutine(PopIn(button.gameObject));
                }
            }
            else
            {
                button.interactable = false;
            }
        }

        if (levelCompleteBanner != null)
            levelCompleteBanner.SetActive(false);
    }

    // Call this at the end of a level when it's completed
    public void ShowLevelCompleteBanner()
    {
        if (levelCompleteBanner != null)
        {
            levelCompleteBanner.SetActive(true);
        }
    }

    // Called by the "Continue" button in the banner
    public void OnContinueToMap()
    {
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);

        if (unlockedLevels < totalLevels)
        {
            PlayerPrefs.SetInt("UnlockedLevels", unlockedLevels + 1);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene("Level select screen");
    }

    // Optional: reset button to test
    public void ResetProgress()
    {
        PlayerPrefs.SetInt("UnlockedLevels", 1);
        PlayerPrefs.Save();
    }

    IEnumerator PopIn(GameObject button)
    {
        RectTransform rt = button.GetComponent<RectTransform>();
        Vector3 originalScale = rt.localScale;

        rt.localScale = Vector3.zero;
        float duration = 0.4f;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            rt.localScale = Vector3.Lerp(Vector3.zero, originalScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        rt.localScale = originalScale;
    }
}
