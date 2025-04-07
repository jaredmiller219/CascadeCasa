using UnityEngine;

public class FurnatureDropdownManager : MonoBehaviour
{
    public GameObject furnatureDropdown;
    public GameObject btnImage;
    private Animator _animator;

    private void Start()
    {
        _animator = furnatureDropdown.GetComponent<Animator>();
        if (_animator == null){
            Debug.LogError("No Animator component found on furnatureDropdown!");
        }
        if (btnImage == null){
            Debug.LogError("No btnImage assigned!");
        }
        if (furnatureDropdown == null){
            Debug.LogError("No furnatureDropdown assigned!");
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
