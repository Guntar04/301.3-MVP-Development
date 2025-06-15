using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Optional Game Data")]
    public TowerData towerData;
    public EnemyData enemyData;

    [Header("UI References")]
    public GameObject tooltipPanel;
    public Text tooltipText;
    public Vector3 offset = new Vector3(10f, -10f, 0f);

    private static TooltipDisplay _singleton;

    private void Awake()
    {
        // Auto-assign the first instance for static reference (optional)
        if (_singleton == null)
        {
            _singleton = this;
            tooltipPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (tooltipPanel && tooltipPanel.activeSelf)
        {
            tooltipPanel.transform.position = Input.mousePosition + offset;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipText == null || tooltipPanel == null) return;

        string info = "";

        if (towerData != null)
        {
            info = $"<b>{towerData.towerName}</b>\n" +
                   $"<color=green>Strength:</color> {towerData.strength}\n" +
                   $"<color=red>Weakness:</color> {towerData.weakness}";
        }
        else if (enemyData != null)
        {
            info = $"<b>{enemyData.enemyName}</b>\n" +
                   $"<color=green>Strength:</color> {enemyData.strength}\n" +
                   $"<color=red>Weakness:</color> {enemyData.weakness}";
        }

        tooltipText.text = info;
        tooltipPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipPanel)
            tooltipPanel.SetActive(false);
    }
}
