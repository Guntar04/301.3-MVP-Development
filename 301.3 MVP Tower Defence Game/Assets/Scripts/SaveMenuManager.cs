using UnityEngine;

public class SaveMenuManager : MonoBehaviour
{
    public GameObject saveMenuPanel;

    public void OpenSaveMenu()
    {
        saveMenuPanel.SetActive(true);
    }

    public void CloseSaveMenu()
    {
        saveMenuPanel.SetActive(false);
    }
}
