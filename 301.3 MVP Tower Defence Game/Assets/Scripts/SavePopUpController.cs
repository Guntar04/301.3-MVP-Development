using UnityEngine;
using UnityEngine.UI;

public class SaveSystemUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject savePopup;
    public Button closeButton;
    public Button saveButton; // Optional: if you want to connect the Save button directly

    void Start()
    {
        savePopup.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(HidePopup);

        if (saveButton != null)
            saveButton.onClick.AddListener(SaveGame);
    }

    public void SaveGame()
    {
        // Example Save Logic
        PlayerPrefs.SetInt("Coins", 100); // Replace with actual game data
        PlayerPrefs.Save();

        ShowPopup();
    }

    private void ShowPopup()
    {
        savePopup.SetActive(true);
    }

    private void HidePopup()
    {
        savePopup.SetActive(false);
    }
}
