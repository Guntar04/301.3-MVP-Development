using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject SettingsPanel;
    public Button soundButton;
    private bool isSoundOn = true;
    void Start()
    {
        SettingsPanel.SetActive(false);
        soundButton.onClick.AddListener(ToggleSound);
    }

    public void OpenSettings()
    { 
     SettingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        SettingsPanel.SetActive(false);
    }

    public void GoHome()
    {        
        SceneManager.LoadScene("Main menu scene");
    }

    void ToggleSound()
    { 
     isSoundOn = !isSoundOn;
        AudioListener.volume = isSoundOn ? 1 : 0;
        soundButton.GetComponentInChildren<Text>().text = isSoundOn ? "Sound: ON" : "Sound: OFF";
    }

    public void ToggleSettings()
    {
        SettingsPanel.SetActive(!SettingsPanel.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
