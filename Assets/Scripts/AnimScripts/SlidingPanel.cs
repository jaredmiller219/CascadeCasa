using UnityEngine;

public class SlidePanelController : MonoBehaviour
{
    // public float slideSpeed = 10f;
    // public float visibleTabWidth = 40f;
    // private RectTransform _panelRect;
    // private Vector2 _openPos;
    // private Vector2 _closedPos;
    // private bool _isOpen;

    public GameObject Panel;

    // Animation reference
    private Animator _animator;

    private static readonly int Open = Animator.StringToHash("open");

    private void Start()
    {
        _animator = Panel.GetComponent<Animator>();
    }

    public void TogglePanel()
    {
        // toggle the animator's "open" bool
        var isOpen = _animator.GetBool(Open);
        // set the animator's "open" bool to the opposite of its current value
        _animator.SetBool(Open, !isOpen);

    }
}
