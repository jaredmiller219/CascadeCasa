using UnityEngine;

public class ResetPopupManager : MonoBehaviour
{
    public GameObject ResetPopup;
    private Animator _animator;

    private void Start()
    {
        _animator = ResetPopup.GetComponent<Animator>();
    }

    public void Animate()
    {
        if (ResetPopup == null || _animator == null) return;
        ResetPopup.SetActive(true);
        _animator.Play("Pull", 0, 0f);
    }
}
