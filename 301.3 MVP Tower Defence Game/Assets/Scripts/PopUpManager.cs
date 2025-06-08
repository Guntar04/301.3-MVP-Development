using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public GameObject upgradesPanel;
    public GameObject encyclopediaPanel;

    // Open Methods
    public void OpenUpgrades()
    {
        upgradesPanel.SetActive(true);
    }

    public void OpenEncyclopedia()
    {
        encyclopediaPanel.SetActive(true);
    }

    // Close Methods
    public void CloseUpgrades()
    {
        upgradesPanel.SetActive(false);
    }

    public void CloseEncyclopedia()
    {
        encyclopediaPanel.SetActive(false);
    }
}
