using UnityEngine;

// This class is responsible for handling the reset popup animation
public class ResetPopup : MonoBehaviour
{
    // Reference to the GameObject that represents the Challenge Reset notification popup
    public GameObject resetPopup;

    // Private reference to the Animator component used to control animations
    private Animator _animator;

    // Unity's Start method is called before the first frame update
    private void Start()
    {
        // Get the Animator component attached to the resetPopup GameObject
        _animator = resetPopup.GetComponent<Animator>();
    }


    /// <summary>
    /// Plays the "Pull" animation on the resetPopup GameObject.
    /// This method is called to trigger the animation when needed.
    /// </summary>
    /// <remarks>
    /// This method checks if the resetPopup GameObject and the Animator component are not null
    /// before attempting to play the animation.
    /// It also ensures that the resetPopup GameObject is active in the scene.
    /// </remarks>
    public void Animate()
    {
        // Check if the resetPopup GameObject or the Animator component is null
        if (resetPopup == null || _animator == null) return;

        // Ensure the resetPopup GameObject is active in the scene
        resetPopup.SetActive(true);

        // Play the "Pull" animation from the Animator, starting at the beginning (time 0f)
        _animator.Play("Pull", 0, 0f);
    }
}
