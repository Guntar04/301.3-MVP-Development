using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    public static Plot Main;

    public GameObject tower;
    private Color startColor;

    public bool isSelectingTower;

     private void Awake()
    {
        Main = this;
    }

    private void Start()
    {
        startColor = sr.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sr.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        sr.color = startColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (tower != null) return; // If a tower is already placed, do nothing

        if (!isSelectingTower)
        {
            // Open the menu and pass this plot to the menu
            InGameBuyMenu.Main.ToggleMenu(this);
            isSelectingTower = true;
        }
    }

    public void PlaceTower()
    {
        Tower towerToBuild = BuildManager.Main.GetSelectedTower();
        Vector3 offset = new Vector3(0, 0.5f, 0); // Adjust the Y value as needed

        if (towerToBuild.cost > LevelManager.Main.currency)
        {
            Debug.Log("You can't afford this tower");
            isSelectingTower = false; // Reset the selection state
            return; // Exit without placing a tower or resetting the flag
        }
        else
        {
            // Deduct currency and place the tower
            LevelManager.Main.SpendCurrency(towerToBuild.cost);

            tower = Instantiate(towerToBuild.prefab, transform.position + offset, Quaternion.identity);

            Debug.Log("Tower placed successfully!");

            // Close the menu after placing the tower
            InGameBuyMenu.Main.CloseMenu();
            isSelectingTower = false; // Reset the selection state only after successful placement
        }
    }

    public void CancelSelection()
    {
        isSelectingTower = false;
    }
}
