using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void loadCredits()
    {
        SceneManager.LoadScene("Credits scene");
    }
}
