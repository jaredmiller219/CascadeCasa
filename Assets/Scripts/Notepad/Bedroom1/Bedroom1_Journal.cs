using UnityEngine;
using UnityEngine.UI;

public class Bedroom1_Journal : MonoBehaviour
{
    /// <summary>
    /// Reference to the journal popup GameObject that appears when the journal is opened.
    /// </summary>
    public GameObject journalPopup;

    /// <summary>
    /// Reference to the journal button GameObject that opens the journal.
    /// </summary>
    [SerializeField]
    private Button journalButton;

    /// <summary>
    /// Reference to the animator component attached to the journal button.
    /// </summary>
    private Animator animator;

    private void Start()
    {
        if (journalPopup != null) journalPopup.SetActive(false);
        animator = journalButton.GetComponent<Animator>();
        // canToggle = false;
    }

    /// <summary>
    /// Toggles the visibility of the journal popup.
    /// This method is called when the journal button is clicked.
    /// It checks if the journal popup is currently active and toggles its state accordingly.
    /// </summary>
    /// <param name="isActive">True to show the journal popup, false to hide it.</param>
    public void ToggleJournal()
    {
        // if (!canToggle) return;
        journalPopup.SetActive(!journalPopup.activeSelf);
    }

    /// <summary>
    /// Sets the hover state of the journal button.
    /// This method is called to indicate whether the mouse is hovering over the button.
    /// </summary>
    /// <param name="isHovering">True if the mouse is hovering over the button, false otherwise.</param>
    public void SetHover(bool isHovering)
    {
        if (animator != null) animator.SetBool("hover", isHovering);
    }
}
