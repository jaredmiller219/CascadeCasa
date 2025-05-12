using UnityEngine;

public class Bathroom_FurnitureDropdown : MonoBehaviour
{
    /// <summary>
    /// Reference to the dropdown panel GameObject.
    /// </summary>
    public GameObject furnitureDropdown;

    /// <summary>
    /// Reference to the button image GameObject.
    /// </summary>
    public GameObject btnImage;

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
    /// <br />
    /// It manages the visibility of the dropdown panel and animates the button image.
    /// </summary>
    private static readonly int Open = Animator.StringToHash("open");

    private void Start()
    {
        _animator = furnitureDropdown.GetComponent<Animator>();
        switch (true)
        {
            case true when _animator == null:
                Debug.LogError("Animator component not found on furnitureDropdown");
                break;
            case true when btnImage == null:
                Debug.LogError("btnImage not assigned");
                break;
            case true when furnitureDropdown == null:
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
        if (furnitureDropdown == null || _animator == null) return;
        if (audioSource && dropdownSound) audioSource.PlayOneShot(dropdownSound);

        var isOpen = _animator.GetBool(Open);

        // Toggle to opposite of current value
        _animator.SetBool(Open, !isOpen);

        // Rotate the button image's x-axis by 180 degrees
        if (!isOpen) btnImage.transform.Rotate(180, 0, 0);

        // Reset the button image's x-axis back to 0
        else btnImage.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
