using UnityEngine;

public class Bathroom_FurnitureDropdown : MonoBehaviour
{
    /// <summary>
    /// Reference to the dropdown panel GameObject.
    /// </summary>
    public GameObject furnitureDropdown;
    
    /// <summary>
    /// The source of the audio to play
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// The sound that plays when you click the dropdown button
    /// </summary>
    public AudioClip dropdownSound;

    /// <summary>
    /// Reference to the Animator component for animating the dropdown panel.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// This class handles the dropdown panel for furniture in the game.
    /// </summary>
    private static readonly int Open = Animator.StringToHash("open");

    private void Start()
    {
        _animator = furnitureDropdown.GetComponent<Animator>();
        switch (true)
        {
            case true when !_animator:
                Debug.LogError("Animator component not found on furnitureDropdown");
                break;
            case true when !furnitureDropdown:
                Debug.LogError("furnitureDropdown not assigned");
                break;
        }
    }

    /// <summary>
    /// Toggles the dropdown panel's visibility and animates the button image.
    /// <br />
    /// It also rotates the button image to indicate the panel's state.
    /// </summary>
    public void PullBarDown()
    {
        if (audioSource && dropdownSound) audioSource.PlayOneShot(dropdownSound);
        if (!furnitureDropdown || !_animator) return;

        var isOpen = _animator.GetBool(Open);

        // Toggle to opposite of current value
        _animator.SetBool(Open, !isOpen);
    }
}
