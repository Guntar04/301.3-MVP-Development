using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] Button pauseButton; // Assign the button in the Inspector
    [SerializeField] GameObject pausePanel; // Assign the pause panel in the Inspector
    [SerializeField] Button resumeButton; // Assign the resume button in the Inspector
    [SerializeField] Button quitButton; // Assign the quit button in the Inspector

    public static GameOver Main;

    private void Awake()
    {
        Main = this; // Assign the static reference
        menuPanel.SetActive(false);
        pausePanel.SetActive(false); // Ensure the pause panel is initially inactive

        // Add the PauseGame method to the button's OnClick event
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseGame);
        }
        // Add the ResumeGame method to the resume button's OnClick event
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }
        // Add the QuitGame method to the quit button's OnClick event
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    public void ShowGameOver()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Pause the game
    }
    public void QuitGame()
    {
        // Implement your quit logic here, e.g., load the main menu scene
        Debug.Log("Quit Game");
        //Add implementation to quit the game or return to the main menu
    }
}
