using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Button soundButton;
    private bool isSoundOn = true;
    void Start()
    {
        settingsPanel.SetActive(false);
        soundButton.onClick.AddListener(ToggleSound);
    }

    public void OpenSettings()
    { 
     settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
