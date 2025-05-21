using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bathroom_Journal : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private static readonly int Hover = Animator.StringToHash("hover");

    /// <summary>
    /// Reference to the journal popup GameObject that appears when the journal button is clicked.
    /// </summary>
    public GameObject journalPopup;

    /// <summary>
    /// The source of the audio
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// The audio sound
    /// </summary>
    public AudioClip clickSound;

    /// <summary>
    /// Reference to the journal button GameObject.
    /// </summary>
    [SerializeField]
    private Button journalButton;

    /// <summary>
    /// Reference to the animator component attached to the journal button.
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Reference to the GameObject representing the journal image.
    /// </summary>
    [SerializeField]
    private GameObject JournalImage;

    /// <summary>
    /// A reference to the Bathroom notepad
    /// </summary>
    [SerializeField]
    private Bathroom_Notepad notepad;

    private void Start()
    {
        if (journalPopup) journalPopup.SetActive(false);
        animator = journalButton.GetComponent<Animator>();
    }

    /// <summary>
    /// Toggles the visibility of the journal popup.
    /// <br />
    /// This method is called when the journal button is clicked.
    /// </summary>
    /// <param name="isActive">True to show the journal popup, false to hide it.</param>
    public void ToggleJournal()
    {
        notepad.SaveCurrentInputIfNeeded();
        journalPopup.SetActive(!journalPopup.activeSelf);
    }

    /// <summary>
    /// Sets the hover state of the journal button.
    /// </summary>
    /// <param name="isHovering">True if the mouse is hovering over the button, false otherwise.</param>
    public void SetHover(bool isHovering)
    {
        if (animator) animator.SetBool(Hover, isHovering);
    }

    /// <summary>
    /// Handles the pointer down event when the button is pressed.
    /// This method changes the color of the JournalImage to indicate a pressed state.
    /// </summary>
    /// <param name="eventData">The event data associated with the pointer down event.</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        // Light gray
        if (JournalImage) JournalImage.GetComponent<Image>().color = new Color32(200, 200, 200, 255);
    }

    /// <summary>
    /// Handles the pointer up event when the button is released.
    /// <br />
    /// This method changes the color of the JournalImage back to white.
    /// </summary>
    /// <param name="eventData">The event data associated with the pointer up event.</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        // White
        if (JournalImage) JournalImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound);
    }

    /// <summary>
    /// Closes the journal popup if it's currently open.
    /// </summary>
    public void CloseJournal()
    {
        if (journalPopup && journalPopup.activeSelf)
        {
            notepad.SaveCurrentInputIfNeeded();
            journalPopup.SetActive(false);
        }
    }
}
