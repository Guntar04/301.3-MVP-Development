using UnityEngine;
using TMPro;

public class InGameBuyMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI healthUI;
    [SerializeField] GameObject MenuPanel;

    public static InGameBuyMenu Main { get; private set; }

    private bool isMenuOpen = false;
    private Plot selectedPlot;

    private void Awake()
    {
        Main = this;
        isMenuOpen = false;
        gameObject.SetActive(false); // Start with the menu disabled
    }

    public void ToggleMenu(Plot plot)
    {
        isMenuOpen = !isMenuOpen;
        gameObject.SetActive(isMenuOpen); // Enable or disable the menu GameObject

        if (isMenuOpen)
        {
            selectedPlot = plot; // Store the selected plot
            Vector2 mousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        }
        else
        {
            selectedPlot = null; // Clear the selected plot
        }
    }

    public void SelectTower(int towerIndex)
    {
        if (selectedPlot != null)
        {
            BuildManager.Main.SetSelectedTower(towerIndex, selectedPlot);
        }
    }

    public void CloseMenu()
    {
        isMenuOpen = false;
        gameObject.SetActive(false); // Disable the menu GameObject
    }

    public void OnGUI()
    {
        currencyUI.text = LevelManager.Main.currency.ToString();
        healthUI.text = LevelManager.Main.health.ToString();
    }
}
