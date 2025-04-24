using UnityEngine;

public class ResetPopup : MonoBehaviour
{

    /// <summary>
    /// Reference to the GameObject that represents the reset popup in the scene.
    /// This GameObject should have an Animator component attached to it.
    /// The Animator component is used to control the animation of the reset popup.
    /// </summary>
    public GameObject resetPopup;

    /// <summary>
    /// Reference to the Animator component attached to the resetPopup GameObject.
    /// This component is responsible for playing the animation clips assigned to it.
    /// </summary>
    private Animator _animator;

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
