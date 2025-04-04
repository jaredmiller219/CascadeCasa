using UnityEngine;

public class ResetPopupManager : MonoBehaviour
{
    public GameObject resetPopup;
    private Animator _animator;

    private void Start()
    {
        _animator = resetPopup.GetComponent<Animator>();
    }

    public void Animate()
    {
        if (resetPopup == null || _animator == null) return;
        resetPopup.SetActive(true);
        _animator.Play("Pull", 0, 0f);
        // resetPopup.SetActive(false);
    }
}
