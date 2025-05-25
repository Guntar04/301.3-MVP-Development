using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject losePanel;
    [SerializeField] Button pauseButton; // Assign the button in the Inspector
    [SerializeField] GameObject pausePanel; // Assign the pause panel in the Inspector
    [SerializeField] Button resumeButton; // Assign the resume button in the Inspector
    [SerializeField] Button quitButton01; // Assign the quit button in the Inspector
    [SerializeField] Button quitButton02; // Assign the quit button in the Inspector
    [SerializeField] GameObject winPanel; // Assign the win panel in the Inspector

    public static GameOver Main;

    private void Awake()
    {
        Main = this; // Assign the static reference
        losePanel.SetActive(false);
        pausePanel.SetActive(false); // Ensure the pause panel is initially inactive
        winPanel.SetActive(false); // Ensure the win panel is initially inactive

        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseGame);
        }
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }
        if (quitButton01 != null)
        {
            quitButton01.onClick.AddListener(QuitGame);
        }
    }

    public void ShowGameOver()
    {
        losePanel.SetActive(true);
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
        Time.timeScale = 1f; // Resume the game
    }
    public void QuitGame()
    {
        // Implement your quit logic here, e.g., load the main menu scene
        Debug.Log("Quit Game");
        //Add implementation to quit the game or return to the main menu
    }
    public void WinGame()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }
}
