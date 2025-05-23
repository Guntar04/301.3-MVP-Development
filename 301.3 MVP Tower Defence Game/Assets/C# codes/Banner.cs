using UnityEngine;

public class Banner : MonoBehaviour
{

    public GameObject banner;

    public void OnlevelComplete()
    { 
      banner.SetActive(true);

    }

}
