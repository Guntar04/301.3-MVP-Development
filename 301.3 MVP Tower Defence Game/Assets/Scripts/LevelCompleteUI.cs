using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteTrigger : MonoBehaviour
{
    private bool levelCompleted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !levelCompleted)
        {
            levelCompleted = true;

            // Show the level complete banner
            LevelUnlockManager manager = FindAnyObjectByType<LevelUnlockManager>();
            if (manager != null)
            {
                manager.ShowLevelCompleteBanner();
            }
            else
            {
                Debug.LogWarning("LevelUnlockManager not found in scene.");
            }
        }
    }
}
