using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    [SerializeField] private GameObject journalPopup;
    [SerializeField] private Button journalButton;

    private Animator animator;

    // private bool canToggle = false;

    private void Start()
    {
        if (journalPopup != null)
        {
            journalPopup.SetActive(false);
        }
        animator = journalButton.GetComponent<Animator>();
        // canToggle = false;
    }

    /// <summary>
    /// Toggles the visibility of the journal popup.
    /// This method is called when the journal button is clicked.
    /// It checks if the journal popup is currently active and toggles its state accordingly.
    /// <example>
    /// <code>
    /// // Example usage:
    /// journal.ToggleJournal();
    /// </code>
    /// </example>
    /// </summary>
    /// <remarks>
    /// This method uses the SetActive method to show or hide the journal popup.
    /// It assumes that the journalPopup GameObject is assigned in the inspector.
    /// </remarks>
    /// <param name="isActive">True to show the journal popup, false to hide it.</param>
    public void ToggleJournal()
    {
        // if (!canToggle) return;
        journalPopup.SetActive(!journalPopup.activeSelf);
    }

    // public void SetToggleStateAfterAnimation(string stateName, bool setToggle)
    // {
    //     StopAllCoroutines();
    //     StartCoroutine(WaitForAnimationToEnd(stateName, setToggle));
    // }

    // private IEnumerator WaitForAnimationToEnd(string stateName, bool setToggle)
    // {
    //     while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
    //     {
    //         yield return null;
    //     }

    //     while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
    //     {
    //         yield return null;
    //     }

    //     canToggle = setToggle;
    // }

    /// <summary>
    /// Sets the hover state of the journal button.
    /// This method is called to indicate whether the mouse is hovering over the button.
    /// <example>
    /// <code>
    /// // Example usage:
    /// journal.SetHover(true); // Set hover state to true
    /// journal.SetHover(false); // Set hover state to false
    /// </code>
    /// </example>
    /// </summary>
    /// <remarks>
    /// This method uses an animator to set the hover state of the button.
    /// It assumes that the animator has a parameter named "hover" to control the hover animation.
    /// </remarks>
    /// <param name="isHovering">True if the mouse is hovering over the button, false otherwise.</param>
    public void SetHover(bool isHovering)
    {
        if (animator != null)
        {
            animator.SetBool("hover", isHovering);
        }
    }
}
