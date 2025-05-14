using UnityEngine;

public class Patio_SlidePanelController : MonoBehaviour
{
    /// <summary>
    /// Reference to the panel GameObject that will be animated.
    /// </summary>
    public GameObject panel;

    /// <summary>
    /// The source of the audio
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// The sound to play when the button is clicked
    /// </summary>
    public AudioClip toggleSound;

    /// <summary>
    /// Private reference to the Animator component attached to the panel.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Hash for the "open" parameter in the Animator.
    /// </summary>
    private static readonly int Open = Animator.StringToHash("open");

    private void Start()
    {
        _animator = panel.GetComponent<Animator>();
    }

    /// <summary>
    /// Method to toggle the panel's open/close state.
    /// <br />
    /// Rotate the button image to indicate the current state of the panel.
    /// </summary>
    public void TogglePanel()
    {
        if (audioSource && toggleSound) audioSource.PlayOneShot(toggleSound);

        var isOpen = _animator.GetBool(Open);

        // Set the "open" parameter to the opposite of its current value
        _animator.SetBool(Open, !isOpen);
    }
}
