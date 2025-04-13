using UnityEngine;

public class SlidePanelController : MonoBehaviour
{
    public GameObject panel;

    // Animation reference
    private Animator _animator;

    private static readonly int Open = Animator.StringToHash("open");

    private void Start()
    {
        _animator = panel.GetComponent<Animator>();
    }

    public void TogglePanel()
    {
        // toggle the animator's "open" bool
        var isOpen = _animator.GetBool(Open);
        // set the animator's "open" bool to the opposite of its current value
        _animator.SetBool(Open, !isOpen);

    }
}
