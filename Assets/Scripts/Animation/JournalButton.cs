using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JournalButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Journal journalManager;

    [SerializeField] private GameObject JournalImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (journalManager != null)
        {
            journalManager.SetHover(true);
            journalManager.SetToggleStateAfterAnimation("Hover", true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (journalManager != null)
        {
            journalManager.SetHover(false);
            journalManager.SetToggleStateAfterAnimation("Idle", false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (JournalImage != null)
        {
            JournalImage.GetComponent<Image>().color = new Color32(200, 200, 200, 255); // Light gray
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (JournalImage != null)
        {
            JournalImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255); // White
        }
    }
}
