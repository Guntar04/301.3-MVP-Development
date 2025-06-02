using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Main;

    [Header("References")]
    [SerializeField] private Tower[] towers;

    private int selectedTower = -1; // Default to no tower selected

    private void Awake()
    {
        Main = this;
    }

    public Tower GetSelectedTower()
    {
        if (selectedTower == -1)
        {
            Debug.LogError("No tower selected!");
            return null;
        }
        return towers[selectedTower];
    }

    public void SetSelectedTower(int _selectedTower, Plot selectedPlot)
    {
        selectedTower = _selectedTower;
        Debug.Log("Selected tower: " + towers[selectedTower].name);

        // Place the tower on the selected plot
        if (selectedPlot != null)
        {
            selectedPlot.PlaceTower();
        }
    }
}
