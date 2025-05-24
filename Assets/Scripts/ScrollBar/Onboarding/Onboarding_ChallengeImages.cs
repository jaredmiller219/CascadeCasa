using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class Onboarding_ChallengeImage : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// The index of the button in the scroll area.
    /// </summary>
    public int buttonIndex;

    /// <summary>
    ///  The previous button index
    /// </summary>
    [UsedImplicitly]
    public int PreviousButtonIndex = -1;

    /// <summary>
    /// The default css
    /// </summary>
    public string AssociatedCss { get; set; }

    /// <summary>
    /// The current css
    /// </summary>
    [UsedImplicitly]
    public string CurrentCss { get; set; }

    /// <summary>
    /// The image click action
    /// </summary>
    public static event Action<string> OnAnyImageClicked;

    /// <summary>
    /// The index related to the css
    /// </summary>
    [UsedImplicitly]
    public int AssociatedIndex;

    /// <summary>
    /// if the challenge was completed
    /// </summary>
    public bool Completed { get; set; }

    /// <summary>
    /// Whether the image is locked or not
    /// </summary>
    [UsedImplicitly]
    public bool Locked { get; set; }

    /// <summary>
    /// Event triggered when the associated challenge image is interacted with.
    /// </summary>
    public event Action OnInteracted;

    /// <summary>
    /// The original parent of the image.
    /// </summary>
    private Transform _originalParent;

    /// <summary>
    /// Reference to the horizontal scroll bar.
    /// </summary>
    private Onboarding_HorizontalScrollBar _scrollBar;

    /// <summary>
    /// The notepad reference
    /// </summary>
    private static Onboarding_Notepad _notepad;

    /// <summary>
    /// Determines whether the challenge image is currently interactable or not.
    /// </summary>
    private bool _interactable = true;

    /// <summary>
    /// Image was clicked
    /// </summary>
    /// <param name="css">The CSS of the image</param>
    public static void NotifyImageClicked(string css) => OnAnyImageClicked?.Invoke(css);

    /// <summary>
    /// Sets the interactability state of the object.
    /// </summary>
    /// <param name="state">The state to set interactability to.
    /// True for interactable, false for non-interactable.</param>
    public void SetInteractable(bool state) => _interactable = state;

    /// <summary>
    /// Handles pointer click events on the draggable image.
    /// </summary>
    /// <param name="eventData">Pointer event data containing information about the click.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_interactable || Completed || !_scrollBar) return;
        if (_notepad.currentChallengeIndex == -1) _notepad.ResetCurrentChallenge();
        if (_notepad.buttonIndex >= 0) _notepad.SaveTextForIndex(_notepad.buttonIndex);

        // ---------------- For debug only --------------------------
        // Onboarding_ChallengeImage clickedImage = _scrollBar.GetImageAtIndex(_buttonIndex);
        // string imageName = clickedImage.GetComponent<Image>().sprite.name;
        // Debug.Log($"Image: {imageName}\nIndex: {_buttonIndex}");
        // ----------------------------------------------------------

        var clickedIndex = transform.GetSiblingIndex();
        _scrollBar.HandleImageClick(clickedIndex, CurrentCss ?? AssociatedCss);
        PreviousButtonIndex = buttonIndex;

        OnInteracted?.Invoke();
    }

    private void Awake()
    {
        _originalParent = transform.parent;
        _scrollBar = _originalParent.GetComponentInParent<Onboarding_HorizontalScrollBar>();

        _notepad = FindFirstObjectByType<Onboarding_Notepad>();
        if (_notepad) OnAnyImageClicked += _notepad.SetCssText;

        Completed = false;
        Locked = true;
    }

    /// <summary>
    /// Called when the object is destroyed. Cleans up resources.
    /// </summary>
    private void OnDestroy()
    {
        if (_notepad) OnAnyImageClicked -= _notepad.SetCssText;
    }

}
