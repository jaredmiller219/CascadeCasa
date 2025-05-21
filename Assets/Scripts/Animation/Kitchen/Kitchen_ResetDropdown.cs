using System.Collections;
using UnityEngine;

public class Kitchen_ResetDropdown : MonoBehaviour
{
    /// <summary>
    /// Reference to the GameObject that represents the reset popup in the scene.
    /// </summary>
    public GameObject resetPopup;

    /// <summary>
    /// The source of the audio
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// The sound to play when the button is clicked
    /// </summary>
    public AudioClip popupSound;

    /// <summary>
    /// Reference to the Animator component attached to the resetPopup GameObject.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Reference to the Notepad component.
    /// </summary>
    private Kitchen_Notepad notepad;

    private void Start()
    {
        _animator = resetPopup.GetComponent<Animator>();
        notepad = FindFirstObjectByType<Kitchen_Notepad>();
    }

    /// <summary>
    /// Plays the "Pull" animation on the resetPopup GameObject.
    /// </summary>
    public void Animate()
    {
        if (audioSource && popupSound) audioSource.PlayOneShot(popupSound);
        if (!resetPopup || !_animator || !notepad) return;

        if (GetNotepadText(notepad.inputField) == "") return;
        resetPopup.SetActive(true);
        _animator.Play("Pull", 0, 0f);
        StartCoroutine(WaitForAnimationToEnd());
    }

    /// <summary>
    /// Coroutine that waits for the animation to finish before deactivating the resetPopup GameObject.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    /// <exception cref="MissingReferenceException">Thrown if the Animator component is missing.</exception>
    private IEnumerator WaitForAnimationToEnd()
    {
        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        var animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);
        _animator.Play("Pull", 0, 0f);
        resetPopup.SetActive(false);
    }

    /// <summary>
    /// Gets the text of the notepad's input field and returns it as a string
    /// </summary>
    /// <param name="notepad">The notepad gameObject</param>
    /// <returns>The text of the component as a string</returns>
    private static string GetNotepadText(GameObject notepad)
    {
        if (!notepad || !notepad.TryGetComponent<TMPro.TMP_InputField>(out var inputField)) return string.Empty;
        return inputField.text;
    }
}
