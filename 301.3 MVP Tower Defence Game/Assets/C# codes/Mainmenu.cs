using UnityEngine;
using UnityEngine.SceneManagement;  

public class Mainmenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("PLAY");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }

}
