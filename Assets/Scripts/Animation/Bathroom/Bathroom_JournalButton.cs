using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bathroom_JournalButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// The source for the audio
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// The click sound
    /// </summary>
    public AudioClip clickSound;

    /// <summary>
    /// Reference to the GameObject representing the journal image.
    /// </summary>
    [SerializeField]
    private GameObject JournalImage;

    /// <summary>
    /// Handles the pointer down event when the button is pressed.
    /// This method changes the color of the JournalImage to indicate a pressed state.
    /// </summary>
    /// <param name="eventData">The event data associated with the pointer down event.</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        // Light gray
        if (JournalImage != null) JournalImage.GetComponent<Image>().color = new Color32(200, 200, 200, 255);

        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound);
    }

    /// <summary>
    /// Handles the pointer up event when the button is released.
    /// This method changes the color of the JournalImage back to white.
    /// </summary>
    /// <param name="eventData">The event data associated with the pointer up event.</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (JournalImage != null) JournalImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
}
