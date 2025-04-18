using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JournalButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
// ,IPointerEnterHandler, IPointerExitHandler,
{

    /// <summary>
    /// Reference to the Journal manager script that handles journal-related functionality.
    /// </summary>
    /// <remarks>
    /// This field is serialized so that it can be assigned in the Unity Inspector.
    /// It is used to manage the journal's state and interactions.
    /// </remarks>
    [SerializeField] private Journal journalManager;

    /// <summary>
    /// Reference to the GameObject representing the journal image.
    /// </summary>
    /// <remarks>
    /// This field is serialized so that it can be assigned in the Unity Inspector.
    /// It is used to change the appearance of the journal button when pressed.
    /// </remarks>
    [SerializeField] private GameObject JournalImage;

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     if (journalManager != null)
    //     {
    //         journalManager.SetHover(true);
    //         journalManager.SetToggleStateAfterAnimation("Hover", true);
    //     }
    // }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     if (journalManager != null)
    //     {
    //         journalManager.SetHover(false);
    //         journalManager.SetToggleStateAfterAnimation("Idle", false);
    //     }
    // }

    /// <summary>
    /// Handles the pointer down event when the button is pressed.
    /// This method changes the color of the JournalImage to indicate a pressed state.
    /// </summary>
    /// <remarks>
    /// This method is called when the user presses down on the button.
    /// It changes the color of the JournalImage to a light gray color.
    /// The color change is done using the Color32 structure to specify the RGBA values.
    /// </remarks>
    /// <param name="eventData">The event data associated with the pointer down event.</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (JournalImage != null)
        {
            JournalImage.GetComponent<Image>().color = new Color32(200, 200, 200, 255); // Light gray
        }
    }

    /// <summary>
    /// Handles the pointer up event when the button is released.
    /// This method changes the color of the JournalImage back to white.
    /// </summary>
    /// <remarks>
    /// This method is called when the user releases the button.
    /// It changes the color of the JournalImage back to white.
    /// The color change is done using the Color32 structure to specify the RGBA values.
    /// </remarks>
    /// <param name="eventData">The event data associated with the pointer up event.</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (JournalImage != null)
        {
            JournalImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255); // White
        }
    }
}
