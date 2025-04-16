using UnityEngine;
using UnityEngine.EventSystems;

public class JournalButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Journal journalManager;

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
}
