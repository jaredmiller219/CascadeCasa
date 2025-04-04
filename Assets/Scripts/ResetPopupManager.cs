using UnityEngine;

public class ResetPopupManager : MonoBehaviour
{
    public GameObject ResetPopup;
    private Animator animator;

    void Start()
    {
        animator = ResetPopup.GetComponent<Animator>();
    }

    public void Animate()
    {
        if (ResetPopup != null && animator != null)
        {
            ResetPopup.SetActive(true);
            animator.Play("Pull", 0, 0f);
        }
    }
}
