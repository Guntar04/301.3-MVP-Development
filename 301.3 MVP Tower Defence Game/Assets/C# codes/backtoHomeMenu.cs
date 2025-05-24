using UnityEngine;
using UnityEngine.SceneManagement;

public class backtoHomeMenu : MonoBehaviour
{
    
   public void loadMainmenu()
    {
        // Load the home menu scene
        SceneManager.LoadScene("Main menu scene");
    }
}
