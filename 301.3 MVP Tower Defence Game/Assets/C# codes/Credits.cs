using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void LoadCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void LoadMainmenu()
    { 
     SceneManager.LoadScene("Main menuScene");
    }


}
