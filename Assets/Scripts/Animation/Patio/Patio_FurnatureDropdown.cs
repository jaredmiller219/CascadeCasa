using UnityEngine;

public class Patio_FurnitureDropdown : MonoBehaviour
{

    /// <summary>
    /// This class handles the dropdown panel for furniture in the game.
    /// It manages the visibility of the dropdown panel and animates the button image.
    /// </summary>
    private static readonly int Open = Animator.StringToHash("open");

    /// <summary>
    /// Reference to the dropdown panel GameObject.
    /// </summary>
    public GameObject furnitureDropdown;

    /// <summary>
    /// Reference to the button image GameObject.
    /// </summary>
    public GameObject btnImage;

    /// <summary>
    /// Reference to the Animator component for animating the dropdown panel.
    /// </summary>
    private Animator _animator;

    public AudioSource audioSource; // $$$$
    public AudioClip dropdownSound; // $$$$

    private void Start()
    {
        // Get the Animator component attached to the furnitureDropdown GameObject
        _animator = furnitureDropdown.GetComponent<Animator>();

        // Validate that all required references are assigned
        switch (true)
        {
            case true when _animator == null:
                // Log an error if the Animator component is missing
                Debug.LogError("No Animator component found on furnitureDropdown!");
                break;
            case true when btnImage == null:
                // Log an error if the btnImage reference is not assigned
                Debug.LogError("No btnImage assigned!");
                break;
            case true when furnitureDropdown == null:
                // Log an error if the furnitureDropdown reference is not assigned
                Debug.LogError("No furnitureDropdown assigned!");
                break;
        }
    }

    /// <summary>
    /// Toggles the dropdown panel's visibility and animates the button image.
    /// This method is called to pull the bar down and show/hide the dropdown panel.
    /// </summary>
    /// <remarks>
    /// This method checks if the furnitureDropdown and Animator component are not null
    /// before attempting to toggle the panel's visibility.
    /// It also rotates the button image to indicate the panel's state.
    /// </remarks>
    public void PullBarDown()
    {
        // Check if the furnitureDropdown or Animator reference is missing
        if (furnitureDropdown == null || _animator == null) return;

        if (audioSource && dropdownSound) // $$$$
        {
            audioSource.PlayOneShot(dropdownSound); // $$$$
        }

        // Get the current value of the "open" parameter in the Animator
        var isOpen = _animator.GetBool(Open);

        // Toggle the "open" parameter to the opposite of its current value
        _animator.SetBool(Open, !isOpen);

        // Check if the panel is currently closed (isOpen is false)
        if (!isOpen)
        {
            // Rotate the button image's x-axis by 180 degrees to indicate the panel is open
            btnImage.transform.Rotate(180, 0, 0);
        }
        else
        {
            // Reset the button image's rotation to its original state (0 degrees on all axes)
            btnImage.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
