using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Kitchen_ChallengeImage : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// The index of the button in the scroll area.
    /// </summary>
    public int buttonIndex;

    /// <summary>
    ///  The previous button index
    /// </summary>
    public int PreviousButtonIndex = -1;

    /// <summary>
    /// The default css
    /// </summary>
    public string AssociatedCss { get; set; }

    /// <summary>
    /// The current css
    /// </summary>
    public string CurrentCss { get; set; }

    /// <summary>
    /// The image click action
    /// </summary>
    public static event Action<string> OnAnyImageClicked;

    /// <summary>
    /// The index related to the css
    /// </summary>
    public int AssociatedIndex;

    /// <summary>
    /// if the challenge was completed
    /// </summary>
    public bool Completed { get; set; }

    /// <summary>
    /// Whether the image is locked or not
    /// </summary>
    public bool Locked { get; set; }

    /// <summary>
    /// The original parent of the image.
    /// </summary>
    private Transform _originalParent;

    /// <summary>
    /// Reference to the horizontal scroll bar.
    /// </summary>
    private Kitchen_HorizontalScrollBar _scrollBar;

    /// <summary>
    /// The notepad reference
    /// </summary>
    private static Kitchen_Notepad _notepad;

    /// <summary>
    /// Initialize the image
    /// </summary>
    /// <param name="image">The image</param>
    /// <param name="associatedCss">The css for the associated image</param>
    public void Init(GameObject image, string associatedCss)
    {
        _scrollBar.imagePrefab = image;
        AssociatedCss = associatedCss;
    }

    /// <summary>
    /// Image was clicked
    /// </summary>
    /// <param name="css"></param>
    public static void NotifyImageClicked(string css)
    {
        OnAnyImageClicked?.Invoke(css);
    }

    /// <summary>
    /// Handles pointer click events on the draggable image.
    /// </summary>
    /// <param name="eventData">Pointer event data containing information about the click.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Completed || !_scrollBar) return;
        if (_notepad.currentChallengeIndex == -1) _notepad.ResetCurrentChallenge();
        if (_notepad.buttonIndex >= 0) _notepad.SaveTextForIndex(_notepad.buttonIndex);

        // ---------------- For debug only --------------------------
        // Kitchen_ChallengeImage clickedImage = _scrollBar.GetImageAtIndex(_buttonIndex);
        // string imageName = clickedImage.GetComponent<Image>().sprite.name;
        // Debug.Log($"Image: {imageName}\nIndex: {_buttonIndex}");
        // ----------------------------------------------------------

        var clickedIndex = transform.GetSiblingIndex();
        _scrollBar.HandleImageClick(clickedIndex, CurrentCss ?? AssociatedCss);

        // Update after the check
        PreviousButtonIndex = buttonIndex;
    }

    private void Awake()
    {
        _originalParent = transform.parent;
        _scrollBar = _originalParent.GetComponentInParent<Kitchen_HorizontalScrollBar>();

        _notepad = FindFirstObjectByType<Kitchen_Notepad>();
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
