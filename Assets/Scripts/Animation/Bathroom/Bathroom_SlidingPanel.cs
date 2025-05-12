using UnityEngine;

public class Bathroom_SlidePanelController : MonoBehaviour
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
    /// The sound to play when you click the button
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

    /// <summary>
    /// Reference to the button image GameObject that will be rotated.
    /// </summary>
    [SerializeField]
    private GameObject btnImage;

    private void Start()
    {
        _animator = panel.GetComponent<Animator>();
    }

    /// <summary>
    /// Method to toggle the panel's open/close state.
    /// <br />
    /// It checks the current state of the panel and updates the Animator parameter accordingly.
    /// Rotates the button image to indicate the current state of the panel.
    /// </summary>
    public void TogglePanel()
    {
        if (audioSource && toggleSound) audioSource.PlayOneShot(toggleSound);
        var isOpen = _animator.GetBool(Open);

        // Set the "open" parameter to the opposite of its current value
        _animator.SetBool(Open, !isOpen);

        // Rotate x-axis by 180 degrees
        if (isOpen) btnImage.transform.Rotate(180, 0, 0);

        // Reset to original state
        else btnImage.transform.Rotate(-180, 0, 0);
    }
}
