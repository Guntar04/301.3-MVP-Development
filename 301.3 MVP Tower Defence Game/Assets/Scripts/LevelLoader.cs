using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{
    public void Openscene()
    {

        SceneManager.LoadScene("level select scene");
    }
 
}
