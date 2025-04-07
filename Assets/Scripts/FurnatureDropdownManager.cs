using UnityEngine;

public class FurnatureDropdownManager : MonoBehaviour
{
    public GameObject furnatureDropdown;
    private Animator _animator;

    private void Start()
    {
        _animator = furnatureDropdown.GetComponent<Animator>();
        if (_animator == null){
            Debug.LogError("No Animator component found on furnatureDropdown!");
        }
    }

    public void PullBarDown()
    {
        if (furnatureDropdown == null || _animator == null){
            return;
        }

        bool isOpen = _animator.GetBool("open");
        _animator.SetBool("open", !isOpen);
    }
}
