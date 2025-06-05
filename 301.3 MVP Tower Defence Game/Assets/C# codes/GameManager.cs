using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void OnLevelComplete()
    {
        LevelUnlockManager.UnlockNextLevel();
        SceneManager.LoadScene("Level Select");
    }
    
}
