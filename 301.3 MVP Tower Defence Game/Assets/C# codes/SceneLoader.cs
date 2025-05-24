using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits scene");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main menu scene");
    }
}
