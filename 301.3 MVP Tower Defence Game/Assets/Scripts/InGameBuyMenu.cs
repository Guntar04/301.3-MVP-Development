using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InGameBuyMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI healthUI;
    [SerializeField] TextMeshProUGUI waveUI;
    [SerializeField] TextMeshProUGUI endWaveUI;

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
        gameObject.SetActive(isMenuOpen);

        // If a plot was already selected and it's not the same as the new one, reset it
        if (selectedPlot != null && selectedPlot != plot)
        {
            selectedPlot.CancelSelection();
        }

        if (isMenuOpen)
        {
            selectedPlot = plot;
            transform.position = Camera.main.WorldToScreenPoint(plot.transform.position);
        }
        else
        {
            if (selectedPlot != null)
            {
                selectedPlot.CancelSelection();
                selectedPlot = null;
            }
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

        if (selectedPlot != null)
        {
            selectedPlot.CancelSelection(); // Reset the plot's selection state
            selectedPlot = null;
        }
    }

    public void OnGUI()
    {
        currencyUI.text = LevelManager.Main.currency.ToString();
        healthUI.text = LevelManager.Main.health.ToString();
        waveUI.text = EnemySpawner.Main.currentWave.ToString();
        endWaveUI.text = EnemySpawner.Main.currentWave.ToString();
    }
}
