using UnityEngine;

namespace AnimScripts
{
    public class FurnitureDropdown : MonoBehaviour
    {
        private static readonly int Open = Animator.StringToHash("open");

        // Game Object References
        public GameObject furnitureDropdown;
        public GameObject btnImage;

        // Reference to the animation
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
            // Check if both dont exist
            if (furnitureDropdown == null || _animator == null){
                // if they dont exist, return
                return;
            }

            // toggle the animator's "open" bool
            var isOpen = _animator.GetBool(Open);
            // set the animator's "open" bool to the opposite of its current value
            _animator.SetBool(Open, !isOpen);

            // check if the panel is open,
            // This makes no sense, but for some reason I had to reverse the rotation lines
            // because it should rotate it if the panel is open, not if it is closed...
            if (!isOpen){
                // Rotate the image's x-axis by 180 degrees
                btnImage.transform.Rotate(180, 0, 0);
            }
            else{
                // Reset the image's x-axis-rotation to 0 degrees
                btnImage.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
