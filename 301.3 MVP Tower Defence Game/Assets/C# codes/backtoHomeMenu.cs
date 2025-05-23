using UnityEngine;
using UnityEngine.SceneManagement;

public class backtoHomeMenu : MonoBehaviour
{
    
   public void loadHomeMenu()
    {
        // Load the home menu scene
        SceneManager.LoadScene("Home menu scene");
    }
}
