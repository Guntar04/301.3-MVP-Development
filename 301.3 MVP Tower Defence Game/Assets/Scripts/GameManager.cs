using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void ExitToMap()
    {
        SceneManager.LoadScene("Level select scene"); 
    }

}