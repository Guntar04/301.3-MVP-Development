using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this line for TextMeshPro

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject losePanel;
    [SerializeField] Button pauseButton; // Assign the button in the Inspector
    [SerializeField] GameObject pausePanel; // Assign the pause panel in the Inspector
    [SerializeField] Button resumeButton; // Assign the resume button in the Inspector
    [SerializeField] Button quitButton01; // Assign the quit button in the Inspector
    [SerializeField] Button quitButton02; // Assign the quit button in the Inspector
    [SerializeField] GameObject winPanel; // Assign the win panel in the Inspector
    [SerializeField] private GameObject[] starIcons; // Array of 3 star GameObjects

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

        int stars = StarSystem.Main.CalculateStars(LevelManager.Main.health);

        // Hide all star icons first
        for (int i = 0; i < starIcons.Length; i++)
            starIcons[i].SetActive(false);

        // Show only the correct star icon (index is stars-1, since 1 star = element 0, etc.)
        if (stars > 0 && stars <= starIcons.Length)
            starIcons[stars - 1].SetActive(true);
    }
}
