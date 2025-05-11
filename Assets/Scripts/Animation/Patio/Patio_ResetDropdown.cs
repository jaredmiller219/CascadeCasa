using System.Collections;
using UnityEngine;

public class Patio_ResetPopup : MonoBehaviour
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
    /// Reference to the Notepad component.
    /// </summary>
    private Patio_Notepad notepad;

    /// <summary>
    /// Reference to the Animator component attached to the resetPopup GameObject.
    /// </summary>
    private Animator _animator;

    private void Start()
    {
        _animator = resetPopup.GetComponent<Animator>();
        notepad = FindFirstObjectByType<Patio_Notepad>();
    }

    /// <summary>
    /// Plays the "Pull" animation on the resetPopup GameObject.
    /// </summary>
    public void Animate()
    {
        if (audioSource && popupSound) audioSource.PlayOneShot(popupSound);
        if (resetPopup == null || _animator == null || notepad == null) return;

        if (notepad.inputField.GetComponent<TMPro.TMP_InputField>().text != "")
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
}
