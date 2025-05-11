using UnityEngine;

public class Kitchen_FurnitureDropdown : MonoBehaviour
{
    /// <summary>
    /// The source of the audio
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// The sound to play when the button is clicked
    /// </summary>
    public AudioClip dropdownSound;

    /// <summary>
    /// Reference to the dropdown panel GameObject.
    /// </summary>
    public GameObject furnitureDropdown;

    /// <summary>
    /// Reference to the button image GameObject.
    /// </summary>
    public GameObject btnImage;

    /// <summary>
    /// Reference to the Animator component attached to the furnitureDropdown GameObject.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// This class handles the behavior of a dropdown panel in the kitchen furniture UI.
    /// It allows the player to toggle the visibility of the dropdown panel and animate the button image.
    /// </summary>
    private static readonly int Open = Animator.StringToHash("open");

    private void Start()
    {
        _animator = furnitureDropdown.GetComponent<Animator>();
        switch (true)
        {
            case true when _animator == null:
                Debug.LogError("No Animator component found on furnitureDropdown!");
                break;
            case true when btnImage == null:
                Debug.LogError("No btnImage assigned!");
                break;
            case true when furnitureDropdown == null:
                Debug.LogError("No furnitureDropdown assigned!");
                break;
        }
    }

    /// <summary>
    /// Toggles the dropdown panel's visibility and animates the button image.
    /// </summary>
    public void PullBarDown()
    {
        if (furnitureDropdown == null || _animator == null) return;
        if (audioSource && dropdownSound) audioSource.PlayOneShot(dropdownSound);

        var isOpen = _animator.GetBool(Open);

        // Toggle the "open" parameter to the opposite of its current value
        _animator.SetBool(Open, !isOpen);

        // Rotate the button image's x-axis by 180 degrees to indicate the panel is open
        if (!isOpen) btnImage.transform.Rotate(180, 0, 0);

        // Reset the button image's rotation to its original state (0 degrees on all axes)
        else btnImage.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
