using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




public class UIPanelManager : MonoBehaviour
{
    GameObject EncyclopediaContent;
    GameObject UpgradesContent;

    private void Start()
    {
        EncyclopediaContent.SetActive(false);
        UpgradesContent.SetActive(false);
    }

    public void ToggleEncyclopedia()
    { 
      EncyclopediaContent.SetActive(!EncyclopediaContent.activeSelf);
        UpgradesContent.SetActive(false);
    }
    public void ToggleUpgrades()
    { 
      UpgradesContent.SetActive(!UpgradesContent.activeSelf);
        EncyclopediaContent.SetActive(false);
    }


}
