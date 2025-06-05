using UnityEngine;
using UnityEngine.SceneManagement;

public class BacktoMissionSelect : MonoBehaviour
{
    public void LoadLevelSelect()
    {
        
        SceneManager.LoadScene("Level Select scene");
    }

}
