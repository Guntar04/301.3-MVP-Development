using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] Button pauseButton; // Assign the button in the Inspector
    [SerializeField] GameObject pausePanel; // Assign the pause panel in the Inspector

    public static GameOver Main;

    private void Awake()
    {
        Main = this; // Assign the static reference
        menuPanel.SetActive(false);

        // Add the PauseGame method to the button's OnClick event
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseGame);
        }
    }

    public void ShowGameOver()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void PauseGame()
    {
        Debug.Log("PauseGame method called.");
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }
}
