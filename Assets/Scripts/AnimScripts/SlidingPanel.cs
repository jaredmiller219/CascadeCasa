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
        // _panelRect = GetComponent<RectTransform>();

        // _openPos = new Vector2(178.99f, _panelRect.anchoredPosition.y);
        // var panelWidth = _panelRect.rect.width;
        // _closedPos = new Vector2(178.99f - panelWidth + visibleTabWidth, _panelRect.anchoredPosition.y);

        // _panelRect.anchoredPosition = _closedPos;
    }

    public void TogglePanel()
    {
        // StopAllCoroutines();
        // StartCoroutine(Slide(_isOpen ? _closedPos : _openPos));
        // _isOpen = !_isOpen;
        // toggle the animator's "open" bool
        var isOpen = _animator.GetBool(Open);
        // set the animator's "open" bool to the opposite of its current value
        _animator.SetBool(Open, !isOpen);

    }

    // private System.Collections.IEnumerator Slide(Vector2 target)
    // {
    //     while (Vector2.Distance(_panelRect.anchoredPosition, target) > 0.1f)
    //     {
    //         _panelRect.anchoredPosition = Vector2.Lerp(
    //             _panelRect.anchoredPosition,
    //             target,
    //             Time.deltaTime * slideSpeed
    //         );
    //         yield return null;
    //     }
    //     _panelRect.anchoredPosition = target;
    // }
}
