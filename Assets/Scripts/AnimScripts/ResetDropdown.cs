using UnityEngine;

namespace AnimScripts
{
    public class ResetPopup : MonoBehaviour
    {
        // Reference to the Challenge Reset notification
        public GameObject resetPopup;

        // Animation reference
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
        }
    }
}
