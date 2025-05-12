using UnityEngine;

public class Bedroom2_FurnitureDropdown : MonoBehaviour
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
    /// The source of the audio
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// The sound to play when you click the button
    /// </summary>
    public AudioClip dropdownSound;

    /// <summary>
    /// This class handles the dropdown panel for furniture in the game.
    /// </summary>
    private static readonly int Open = Animator.StringToHash("open");

    /// <summary>
    /// Reference to the Animator component for animating the dropdown panel.
    /// </summary>
    private Animator _animator;

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
