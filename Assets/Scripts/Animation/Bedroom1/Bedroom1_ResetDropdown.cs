using System.Collections;
using UnityEngine;

public class Bedroom1_ResetDropdown : MonoBehaviour
{
    /// <summary>
    /// Reference to the resetPopup GameObject.
    /// </summary>
    public GameObject resetPopup;

    /// <summary>
    /// The source of the audio
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// The sound to play when you click the button
    /// </summary>
    public AudioClip popupSound;

    /// <summary>
    /// Reference to the Animator component attached to the resetPopup GameObject.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Reference to the Notepad component.
    /// </summary>
    private Bedroom1_Notepad notepad;

    private void Start()
    {
        _animator = resetPopup.GetComponent<Animator>();
        notepad = FindFirstObjectByType<Bedroom1_Notepad>();
    }

    /// <summary>
    /// Plays the "Pull" animation on the resetPopup GameObject.
    /// </summary>
    public void Animate()
    {
        if (audioSource && popupSound) audioSource.PlayOneShot(popupSound);
        if (resetPopup == null || _animator == null || notepad == null) return;

        // If there's text in the notepad, play the animation
        if (GetNotepadText(notepad.inputField) != "")
        {
            resetPopup.SetActive(true);
            _animator.Play("Pull", 0, 0f);
            StartCoroutine(WaitForAnimationToEnd());
        }
    }

    /// <summary>
    /// Coroutine that waits for the animation to finish before deactivating the resetPopup GameObject.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    /// <exception cref="MissingReferenceException">Thrown if the Animator component is missing.</exception>
    private IEnumerator WaitForAnimationToEnd()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);
        _animator.Play("Pull", 0, 0f);
        resetPopup.SetActive(false);
    }

    /// <summary>
    /// Gets the text of the notepad's input field and returns it as a string
    /// </summary>
    /// <param name="notepad">The notepad gameobject</param>
    /// <returns>The text of the component as a string</returns>
    private string GetNotepadText(GameObject notepad)
    {
        if (notepad == null) return string.Empty;
        if (!notepad.TryGetComponent<TMPro.TMP_InputField>(out var inputField)) return string.Empty;
        return inputField.text;
    }
}
