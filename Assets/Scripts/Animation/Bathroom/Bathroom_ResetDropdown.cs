using System.Collections;
using UnityEngine;

public class Bathroom_ResetDropdown : MonoBehaviour
{

    /// <summary>
    /// Reference to the GameObject that represents the reset popup in the scene.
    /// <br />
    /// This GameObject should have an Animator component attached to it.
    /// </summary>
    public GameObject resetPopup;

    /// <summary>
    /// Reference to the Animator component attached to the resetPopup GameObject.
    /// <br />
    /// This component is responsible for playing the animation clips assigned to it.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// The source of the audio
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// The sound to play when you click the popup
    /// </summary>
    public AudioClip popupSound;

    /// <summary>
    /// Reference to the Notepad component that is used to check if there is any text in the notepad.
    /// This is used to determine whether to play the animation or not.
    /// </summary>
    private Bathroom_Notepad notepad;

    private void Start()
    {
        _animator = resetPopup.GetComponent<Animator>();
        notepad = FindFirstObjectByType<Bathroom_Notepad>();
    }

    /// <summary>
    /// Plays the "Pull" animation on the resetPopup GameObject.
    /// This method is called to trigger the animation when needed.
    /// </summary>
    public void Animate()
    {
        if (audioSource && popupSound) audioSource.PlayOneShot(popupSound);
        if (resetPopup == null || _animator == null || notepad == null) return;

        // if there's nothing in the notepad, don't play animation
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
    private IEnumerator WaitForAnimationToEnd()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;
        yield return new WaitForSeconds(animationLength);
        _animator.Play("Pull", 0, 0f);
        resetPopup.SetActive(false);
    }
}
