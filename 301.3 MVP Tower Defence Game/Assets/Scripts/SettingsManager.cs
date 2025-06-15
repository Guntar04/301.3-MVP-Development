using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject SettingsPanel;
    public GameObject OptionsPanel; // 👈 Add this in the Inspector
    public Button soundButton;

    private bool isSoundOn = true;

    void Start()
    {
        SettingsPanel.SetActive(false);
        if (OptionsPanel != null) OptionsPanel.SetActive(false);

        soundButton.onClick.AddListener(ToggleSound);
    }

    public void OpenSettings()
    {
        SettingsPanel.SetActive(true);
        if (OptionsPanel != null) OptionsPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        SettingsPanel.SetActive(false);
        if (OptionsPanel != null) OptionsPanel.SetActive(false);
    }

    public void GoHome()
    {
        SceneManager.LoadScene("Main menu scene"); // Replace with your actual scene name
    }

    void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        AudioListener.volume = isSoundOn ? 1 : 0;
        soundButton.GetComponentInChildren<Text>().text = isSoundOn ? "Sound: ON" : "Sound: OFF";
    }

    public void ToggleSettings()
    {
        bool show = !SettingsPanel.activeSelf;
        SettingsPanel.SetActive(show);
        if (OptionsPanel != null) OptionsPanel.SetActive(false);
    }

    public void GoToOptions()
    {
        if (SettingsPanel != null) SettingsPanel.SetActive(false);
        if (OptionsPanel != null) OptionsPanel.SetActive(true);
    }
}
