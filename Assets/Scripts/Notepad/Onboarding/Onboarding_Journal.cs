using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Onboarding_Journal : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
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
    /// A reference to the onboarding room notepad
    /// </summary>
    [SerializeField]
    private Onboarding_Notepad notepad;

    /// <summary>
    /// Represents the hash ID for the "hover" animation parameter used by the Animator.
    /// </summary>
    private static readonly int Hover = Animator.StringToHash("hover");

    private void Start()
    {
        if (journalPopup) journalPopup.SetActive(false);
        animator = journalButton.GetComponent<Animator>();
    }

    /// <summary>
    /// Toggles the visibility state of the journal popup.
    /// If the popup is currently visible, it will be hidden, and vice versa.
    /// Also ensures that the current input in the notepad is saved if necessary.
    /// </summary>
    public void ToggleJournal()
    {
        notepad.SaveCurrentInputIfNeeded();
        journalPopup.SetActive(!journalPopup.activeSelf);
    }

    /// <summary>
    /// Sets the hover state of the journal button.
    /// </summary>
    /// <param name="isHovering">True if the mouse is hovering over the button, false otherwise.</param>
    [UsedImplicitly]
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
        if (JournalImage) JournalImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound);
    }

    /// <summary>
    /// Closes the journal popup if it's currently open.
    /// </summary>
    public void CloseJournal()
    {
        if (!journalPopup || !journalPopup.activeSelf) return;
        notepad.SaveCurrentInputIfNeeded();
        journalPopup.SetActive(false);
    }
}
