using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KitchenJournal : MonoBehaviour
{
    [SerializeField] private GameObject journalPopup;
    [SerializeField] private Button journalButton;
    private Animator animator;

    public AudioSource audioSource;
    public AudioClip pageFlipSound;

    private void Start()
    {
        if (journalPopup != null)
        {
            journalPopup.SetActive(false);
        }
        animator = journalButton.GetComponent<Animator>();
    }

    public void ToggleJournal()
    {
        // Play sound when toggled
        if (audioSource && pageFlipSound)
            audioSource.PlayOneShot(pageFlipSound);

        journalPopup.SetActive(!journalPopup.activeSelf);
    }

    public void SetHover(bool isHovering)
    {
        if (animator != null)
        {
            animator.SetBool("hover", isHovering);
        }
    }
}
