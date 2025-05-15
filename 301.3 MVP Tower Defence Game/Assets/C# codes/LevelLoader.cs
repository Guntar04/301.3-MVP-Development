using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{
    void start()
    {


    }
    public void Openscene()
    {

        SceneManager.LoadScene("Level 1");
    }
 
}
