using UnityEngine;

// This class is responsible for controlling the sliding panel's animation
public class Bedroom2_SlidePanelController : MonoBehaviour
{
    /// <summary>
    /// Reference to the panel GameObject that will be animated.
    /// </summary>
    public GameObject panel;

    /// <summary>
    /// Private reference to the Animator component attached to the panel.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Hash for the "open" parameter in the Animator.
    /// This is used to optimize the performance of the Animator.
    /// The hash is generated using Animator.StringToHash for better performance.
    /// </summary>
    private static readonly int Open = Animator.StringToHash("open");

    /// <summary>
    /// Reference to the button image GameObject that will be rotated.
    /// This is used to indicate the open/close state of the panel.
    /// </summary>
    /// <remarks>
    /// The button image will rotate 180 degrees when the panel is opened or closed.
    /// </remarks>
    [SerializeField]
    private GameObject btnImage;

    // Unity's Start method is called before the first frame update
    private void Start()
    {
        // Get the Animator component attached to the panel GameObject
        _animator = panel.GetComponent<Animator>();
    }

    /// <summary>
    /// Method to toggle the panel's open/close state.
    /// This method is called when the user interacts with the UI element (e.g., button).
    /// It checks the current state of the panel and updates the Animator parameter accordingly.
    /// It also rotates the button image to indicate the current state of the panel.
    /// </summary>
    /// <remarks>
    /// The method uses the Animator's "open" parameter to control the animation.
    /// The button image is rotated 180 degrees when the panel is opened or closed.
    /// </remarks>
    public void TogglePanel()
    {
        // Get the current value of the "open" parameter in the Animator
        var isOpen = _animator.GetBool(Open);

        // Set the "open" parameter to the opposite of its current value
        // This effectively toggles the panel's open/close state
        _animator.SetBool(Open, !isOpen);

        if (isOpen)
        {
            // Rotate the button image's x-axis by 180 degrees (Opened)
            btnImage.transform.Rotate(180, 0, 0);
        }
        else
        {
            // Reset the rotation of the button image to its original state (Closed)
            btnImage.transform.Rotate(-180, 0, 0);
        }
    }
}
