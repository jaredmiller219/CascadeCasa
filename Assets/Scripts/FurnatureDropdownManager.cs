using UnityEngine;

public class FurnitureDropdownManager : MonoBehaviour
{
    private static readonly int Open = Animator.StringToHash("open");
    public GameObject furnitureDropdown;
    public GameObject btnImage;
    private Animator _animator;

    private void Start()
    {
        _animator = furnitureDropdown.GetComponent<Animator>();
        switch (true){
            case true when _animator == null:
                Debug.LogError("No Animator component found on furnitureDropdown!");
                break;
            case true when btnImage == null:
                Debug.LogError("No btnImage assigned!");
                break;
            case true when furnitureDropdown == null:
                Debug.LogError("No furnitureDropdown assigned!");
                break;
        }
    }

    public void PullBarDown()
    {
        if (furnitureDropdown == null || _animator == null){
            return;
        }

        // toggle the animator's "open" bool
        var isOpen = _animator.GetBool(Open);
        // set the animator's "open" bool to the opposite of its current value
        _animator.SetBool(Open, !isOpen);

        if (!isOpen){
            // then rotate the image's x-axis by 180 degrees
            btnImage.transform.Rotate(180, 0, 0);
        }
        else{
            // reset the image's x-axis to 0 degrees
            btnImage.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
