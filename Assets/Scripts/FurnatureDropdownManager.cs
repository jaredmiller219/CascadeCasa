using UnityEngine;

public class FurnatureDropdownManager : MonoBehaviour
{
    public GameObject furnatureDropdown;
    public GameObject btnImage;
    private Animator _animator;

    private void Start()
    {
        _animator = furnatureDropdown.GetComponent<Animator>();
        switch (true){
            case true when _animator == null:
                Debug.LogError("No Animator component found on furnatureDropdown!");
                break;
            case true when btnImage == null:
                Debug.LogError("No btnImage assigned!");
                break;
            case true when furnatureDropdown == null:
                Debug.LogError("No furnatureDropdown assigned!");
                break;
        }
    }

    public void PullBarDown()
    {
        if (furnatureDropdown == null || _animator == null){
            return;
        }

        bool isOpen = _animator.GetBool("open");
        _animator.SetBool("open", !isOpen);

        if (!isOpen){
            // then rotate the image's x axis by 180 degrees
            btnImage.transform.Rotate(180, 0, 0);
        }
        else{
            // reset the image's x axis to 0 degrees
            btnImage.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
