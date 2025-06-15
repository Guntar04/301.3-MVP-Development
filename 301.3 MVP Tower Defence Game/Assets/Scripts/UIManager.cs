using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Main;

    [Header("UI Panels")]
    public GameObject encyclopediaPanel;
    public GameObject upgradesPanel;

    private bool isHoveringUI;

    private void Awake()
    {
        Main = this;
        Debug.Log("UIManager initialized.");
    }

    public void SetHoveringState(bool state)
    {
        isHoveringUI = state;
    }

    public bool IsHoveringUI()
    {
        return isHoveringUI;
    }

    public void ShowEncyclopedia()
    {
        
        encyclopediaPanel.SetActive(true);
        upgradesPanel.SetActive(false);
    }

    
    public void ShowUpgrades()
    {
        
        upgradesPanel.SetActive(true);
        encyclopediaPanel.SetActive(false);
    }

    // ↩️ Go back to Level Select
    public void ShowLevelSelect()
    {
        encyclopediaPanel.SetActive(false);
        upgradesPanel.SetActive(false);
    }
}
