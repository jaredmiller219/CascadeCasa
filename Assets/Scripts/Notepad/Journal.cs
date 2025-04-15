using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The <c>Journal</c> class is responsible for managing the behavior of a journal popup in the game.
/// It provides functionality to toggle the visibility of the journal popup UI element.
/// </summary>
public class Journal : MonoBehaviour
{
    /// <summary>
    /// A reference to the GameObject representing the journal popup.
    /// This is the UI element that will be shown or hidden when toggling the journal.
    /// </summary>
    [SerializeField]
    private GameObject journalPopup;

    /// <summary>
    /// A reference to the Button that triggers the journal toggle action.
    /// This button is expected to be linked in the Unity Editor.
    /// </summary>
    [SerializeField]
    private Button journalButton;


    /// <summary>
    /// Initializes the journal by setting up the button listener.
    /// This method is called when the script instance is being loaded.
    /// It sets up the button to call the <c>ToggleJournal</c> method when clicked.
    /// </summary>
    /// <remarks>
    /// The <c>Start</c> method is called before the first frame update.
    /// It is a good place to initialize variables and set up references.
    /// </remarks>
    public void Start()
    {
        // set the journal to inactive at the start
        if (journalPopup != null)
        {
            journalPopup.SetActive(false);
        }
    }

    /// <summary>
    /// Toggles the visibility of the journal popup.
    /// If the journal popup is currently active, it will be deactivated.
    /// If it is inactive, it will be activated.
    /// </summary>
    /// <remarks>
    /// This method checks if the <c>journalPopup</c> is not null before attempting to toggle its state.
    /// If the <c>journalPopup</c> is null, no action will be performed.
    /// </remarks>
    public void ToggleJournal()
    {
        if (journalPopup != null)
        {
            // Toggle the active state of the journal popup
            journalPopup.SetActive(!journalPopup.activeSelf);
        }
    }
}
