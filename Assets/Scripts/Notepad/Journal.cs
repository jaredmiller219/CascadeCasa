using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    [SerializeField] private GameObject journalPopup;
    [SerializeField] private Button journalButton;

    private Animator animator;

    // private bool canToggle = false;

    private void Start()
    {
        if (journalPopup != null)
        {
            journalPopup.SetActive(false);
        }
        animator = journalButton.GetComponent<Animator>();
        // canToggle = false;
    }

    public void ToggleJournal()
    {
        // if (!canToggle) return;
        journalPopup.SetActive(!journalPopup.activeSelf);
    }

    // public void SetToggleStateAfterAnimation(string stateName, bool setToggle)
    // {
    //     StopAllCoroutines();
    //     StartCoroutine(WaitForAnimationToEnd(stateName, setToggle));
    // }

    // private IEnumerator WaitForAnimationToEnd(string stateName, bool setToggle)
    // {
    //     while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
    //     {
    //         yield return null;
    //     }

    //     while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
    //     {
    //         yield return null;
    //     }

    //     canToggle = setToggle;
    // }

    public void SetHover(bool isHovering)
    {
        if (animator != null)
        {
            animator.SetBool("hover", isHovering);
        }
    }
}
