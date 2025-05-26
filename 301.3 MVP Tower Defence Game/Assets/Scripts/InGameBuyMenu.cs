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
    [SerializeField] TextMeshProUGUI finalHealthUI;

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

        if (isMenuOpen)
        {
            selectedPlot = plot;
            transform.position = Camera.main.WorldToScreenPoint(plot.transform.position);
        }
        else
        {
            selectedPlot = null;
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
        finalHealthUI.text = LevelManager.Main.health.ToString();
    }
}
