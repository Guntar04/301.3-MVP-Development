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
    [SerializeField] Button speedButton; // Assign the speed button in the Inspector
    [SerializeField] private Image speedButtonImage; // Assign the default speed button sprite in the Inspector
    [SerializeField] private Sprite[] speedButtonSprites; // Assign the speed button sprites in the Inspector

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
        if (speedButton != null)
        {
            speedButton.onClick.AddListener(SpeedOfGame);
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

    public void SpeedOfGame()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 2f; // Speed up the game
            if (speedButtonImage != null && speedButtonSprites != null && speedButtonSprites.Length > 1)
            {
                speedButtonImage.sprite = speedButtonSprites[1]; // Switch to 2x image (element 1)
            }
        }
        else
        {
            Time.timeScale = 1f; // Slow down the game
            if (speedButtonImage != null && speedButtonSprites != null && speedButtonSprites.Length > 0)
            {
                speedButtonImage.sprite = speedButtonSprites[0]; // Switch to 1x image (element 0)
            }
        }
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
