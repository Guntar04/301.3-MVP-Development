using UnityEngine;

public class UIPanelManager : MonoBehaviour
{
    public GameObject EncyclopediaPanel;
    public GameObject UpgradesPanel;

    public float transitionTime = 0.3f;

    private void Start()
    {
        if (EncyclopediaPanel) EncyclopediaPanel.SetActive(false);
        if (UpgradesPanel) UpgradesPanel.SetActive(false);
    }

    public void ToggleEncyclopedia()
    {
        if (EncyclopediaPanel)
            StartCoroutine(TogglePanel(EncyclopediaPanel));
    }

    public void ToggleUpgrades()
    {
        if (UpgradesPanel)
            StartCoroutine(TogglePanel(UpgradesPanel));
    }

    private System.Collections.IEnumerator TogglePanel(GameObject panel)
    {
        bool isActive = panel.activeSelf;

        if (!isActive)
        {
            panel.SetActive(true);
            CanvasGroup cg = panel.GetComponent<CanvasGroup>();
            if (cg == null) cg = panel.AddComponent<CanvasGroup>();
            cg.alpha = 0;
            panel.transform.localScale = Vector3.zero;

            float time = 0;
            while (time < transitionTime)
            {
                float t = time / transitionTime;
                cg.alpha = Mathf.Lerp(0, 1, t);
                panel.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
                time += Time.deltaTime;
                yield return null;
            }

            cg.alpha = 1;
            panel.transform.localScale = Vector3.one;
        }
        else
        {
            CanvasGroup cg = panel.GetComponent<CanvasGroup>();
            if (cg == null) cg = panel.AddComponent<CanvasGroup>();

            float time = 0;
            while (time < transitionTime)
            {
                float t = time / transitionTime;
                cg.alpha = Mathf.Lerp(1, 0, t);
                panel.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
                time += Time.deltaTime;
                yield return null;
            }

            cg.alpha = 0;
            panel.transform.localScale = Vector3.zero;
            panel.SetActive(false);
        }
    }
}
