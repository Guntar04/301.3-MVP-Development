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

        GameObject towerToBuild = BuildManager.Main.GetSelectedTower();
        Vector3 offset = new Vector3(0, 0.5f, 0); // Adjust the Y value as needed
        tower = Instantiate(towerToBuild, transform.position + offset, Quaternion.identity);
    }
}
