using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompleteTrigger : MonoBehaviour
{
    private bool levelCompleted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!levelCompleted && other.CompareTag("Player"))
        {
            levelCompleted = true;

            LevelUnlockManager manager = FindAnyObjectByType<LevelUnlockManager>();

            if(manager != null)
            {
                manager.ShowLevelCompleteBanner();
            }
            else
            {
                Debug.LogWarning("levelUnlockManager not found in the scene.");
            }
        }
    }
}
