using UnityEngine;

public class LivingRoom_FurnitureDropdown : MonoBehaviour
{
    /// <summary>
    /// Reference to the dropdown panel GameObject.
    /// </summary>
    public GameObject furnitureDropdown;

    /// <summary>
    /// Reference to the button's image (the arrow)
    /// </summary>
    public GameObject btnImage;

    /// <summary>
    /// The source of the audio
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// The sound to play when the button is clicked
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
                Debug.LogError("No Animator component found on furnitureDropdown!");
                break;
            case true when !btnImage:
                Debug.LogError("No btnImage found");
                break;
            case true when !furnitureDropdown:
                Debug.LogError("No furnitureDropdown assigned!");
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
        if (!furnitureDropdown || !_animator) return;
        if (audioSource && dropdownSound) audioSource.PlayOneShot(dropdownSound);

        var isOpen = _animator.GetBool(Open);

        // Toggle the "open" parameter to the opposite of its current value
        _animator.SetBool(Open, !isOpen);

        if (!isOpen) btnImage.transform.Rotate(180, 0, 0);
        else btnImage.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
