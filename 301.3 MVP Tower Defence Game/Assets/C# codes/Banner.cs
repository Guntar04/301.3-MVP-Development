using UnityEngine;

public class Banner : MonoBehaviour
{

    public GameObject LevelCompleteBanner;
    public float displaytime = 3f;

    public void OnlevelComplete()
    { 
      LevelCompleteBanner.SetActive(true);

    }

    public void ShowBanner()
    {
        LevelCompleteBanner.SetActive(true);
        Invoke("HideBanner", displaytime);
    }


    void HideBanner()
    { 
     LevelCompleteBanner.SetActive(false);

    }

    
}
