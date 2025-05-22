using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject MenuPanel;
    [SerializeField] Button pauseButton; // Assign the button in the Inspector

    public static GameOver Main;

    private void Awake()
    {
        Main = this; // Assign the static reference
        MenuPanel.SetActive(false);

        // Add the PauseGame method to the button's OnClick event
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseGame);
        }
    }

    public void ShowGameOver()
    {
        MenuPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void PauseGame()
    {
        Debug.Log("PauseGame method called.");
        Time.timeScale = 0f; // Pause the game
    }
}
