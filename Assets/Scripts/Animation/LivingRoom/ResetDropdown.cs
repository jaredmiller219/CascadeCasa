using System.Collections;
using UnityEngine;

public class ResetPopup : MonoBehaviour
{

    /// <summary>
    /// Reference to the GameObject that represents the reset popup in the scene.
    /// This GameObject should have an Animator component attached to it.
    /// The Animator component is used to control the animation of the reset popup.
    /// </summary>
    public GameObject resetPopup;

    /// <summary>
    /// Reference to the Animator component attached to the resetPopup GameObject.
    /// This component is responsible for playing the animation clips assigned to it.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Reference to the Notepad component that is used to check if there is any text in the notepad.
    /// This is used to determine whether to play the animation or not.
    /// </summary>
    private Notepad notepad;

    private void Start()
    {
        // Get the Animator component attached to the resetPopup GameObject
        _animator = resetPopup.GetComponent<Animator>();

        // Get the Notepad component attached to the same GameObject
        notepad = FindFirstObjectByType<Notepad>();
    }


    /// <summary>
    /// Plays the "Pull" animation on the resetPopup GameObject.
    /// This method is called to trigger the animation when needed.
    /// </summary>
    /// <remarks>
    /// This method checks if the resetPopup GameObject and the Animator component are not null
    /// before attempting to play the animation.
    /// It also ensures that the resetPopup GameObject is active in the scene.
    /// </remarks>
    public void Animate()
    {
        // Check if the resetPopup GameObject or the Animator component is null
        if (resetPopup == null || _animator == null || notepad == null) return;

        // the there is nothing in the notepad aka no text is set, then dont play the animation
        if (notepad.inputField.GetComponent<TMPro.TMP_InputField>().text != "")
        {
            // Ensure the resetPopup GameObject is active in the scene
            resetPopup.SetActive(true);

            // Play the "Pull" animation from the Animator, starting at the beginning (time 0f)
            _animator.Play("Pull", 0, 0f);

            // Start a coroutine to wait for the animation to finish
            StartCoroutine(WaitForAnimationToEnd());
        }
    }

    /// <summary>
    /// Coroutine that waits for the animation to finish before deactivating the resetPopup GameObject.
    /// </summary>
    /// <remarks>
    /// This coroutine retrieves the length of the currently playing animation clip
    /// and waits for that duration before deactivating the resetPopup GameObject.
    /// It uses the AnimatorStateInfo to get the length of the animation.
    /// </remarks>
    /// <returns>An IEnumerator for the coroutine.</returns>
    /// <exception cref="MissingReferenceException">Thrown if the Animator component is missing.</exception>
    private IEnumerator WaitForAnimationToEnd()
    {
        // Get the length of the animation clip
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        // Wait for the animation to finish
        yield return new WaitForSeconds(animationLength);

        // Now deactivate the popup
        resetPopup.SetActive(false);
    }
}
