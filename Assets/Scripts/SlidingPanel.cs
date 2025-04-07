using UnityEngine;

public class SlidePanelController : MonoBehaviour
{
    public float slideSpeed = 10f;
    public float visibleTabWidth = 40f;

    private RectTransform panelRect;
    private Vector2 openPos;
    private Vector2 closedPos;
    private bool isOpen = false;

    void Start()
    {
        panelRect = GetComponent<RectTransform>();

        openPos = new Vector2(178.99f, panelRect.anchoredPosition.y);
        float panelWidth = panelRect.rect.width;
        closedPos = new Vector2(178.99f - panelWidth + visibleTabWidth, panelRect.anchoredPosition.y);

        panelRect.anchoredPosition = closedPos;
    }

    public void TogglePanel()
    {
        StopAllCoroutines();
        StartCoroutine(Slide(isOpen ? closedPos : openPos));
        isOpen = !isOpen;
    }

    System.Collections.IEnumerator Slide(Vector2 target)
    {
        while (Vector2.Distance(panelRect.anchoredPosition, target) > 0.1f)
        {
            panelRect.anchoredPosition = Vector2.Lerp(
                panelRect.anchoredPosition,
                target,
                Time.deltaTime * slideSpeed
            );
            yield return null;
        }
        panelRect.anchoredPosition = target;
    }
}
