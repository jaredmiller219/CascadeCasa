using UnityEngine;

// This class is responsible for controlling the sliding panel's animation
public class SlidePanelController : MonoBehaviour
{
    // Reference to the panel GameObject that will be animated
    public GameObject panel;

    // Private reference to the Animator component attached to the panel
    private Animator _animator;

    // Static readonly integer hash for the "open" parameter in the Animator
    // Using a hash instead of a string improves performance
    private static readonly int Open = Animator.StringToHash("open");

    // Unity's Start method is called before the first frame update
    private void Start()
    {
        // Get the Animator component attached to the panel GameObject
        _animator = panel.GetComponent<Animator>();
    }

    // Public method to toggle the panel's open/close state
    public void TogglePanel()
    {
        // Get the current value of the "open" parameter in the Animator
        var isOpen = _animator.GetBool(Open);

        // Set the "open" parameter to the opposite of its current value
        // This effectively toggles the panel's open/close state
        _animator.SetBool(Open, !isOpen);
    }
}
