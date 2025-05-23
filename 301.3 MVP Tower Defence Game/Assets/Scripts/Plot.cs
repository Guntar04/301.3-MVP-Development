using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;

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
        if (tower != null) return;

        Tower towerToBuild = BuildManager.Main.GetSelectedTower();
        Vector3 offset = new Vector3(0, 0.5f, 0); // Adjust the Y value as needed

        if (towerToBuild.cost > LevelManager.Main.currency)
        {
            Debug.Log("You can't afford this tower");
            return;
        }

        LevelManager.Main.SpendCurrency(towerToBuild.cost);
        
        tower = Instantiate(towerToBuild.prefab, transform.position + offset, Quaternion.identity);
    }
}
