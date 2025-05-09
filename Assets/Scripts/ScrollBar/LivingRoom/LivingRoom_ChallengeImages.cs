using UnityEngine;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// This class allows an image to be draggable within a UI canvas and provides functionality
/// for inserting the dragged image into a horizontal scroll bar at a specific position.
/// </summary>
public class LivingRoom_ChallengeImage : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// The index of the button in the scroll area.
    /// </summary>
    public int _buttonIndex;

    public int _previousbuttonindex = -1; // Ensure it's clearly uninitialized at start

    /// <summary>
    ///
    /// </summary>
    public string AssociatedCss { get; set; }

    public string CurrentCss { get; set; }

    /// <summary>
    ///
    /// </summary>
    public static event Action<string> OnAnyImageClicked;

    /// <summary>
    ///
    /// </summary>
    public int AssociatedIndex;

    /// <summary>
    /// if the challenge was completed
    /// </summary>
    public bool Completed { get; set; }

    public bool Locked { get; set; }

    /// <summary>
    /// The original parent of the image.
    /// </summary>
    private Transform _originalParent;

    /// <summary>
    /// Reference to the horizontal scroll bar.
    /// </summary>
    private LivingRoom_HorizontalScrollBar _scrollBar;

    /// <summary>
    ///
    /// </summary>
    private LivingRoom_Notepad notepad;

    /// <summary>
    ///
    /// </summary>
    /// <param name="image"></param>
    /// <param name="associatedCss"></param>
    public void Init(GameObject image, string associatedCss)
    {
        _scrollBar.imagePrefab = image;
        AssociatedCss = associatedCss;
    }


    public void NotifyImageClicked(string css)
    {
        OnAnyImageClicked?.Invoke(css);
    }

    /// <summary>
    /// Handles pointer click events on the draggable image.
    /// </summary>
    /// <param name="eventData">Pointer event data containing information about the click.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Completed && _scrollBar != null)
        {
            // Save the current input
            if (notepad.buttonindex >= 0) notepad.SaveTextForIndex(notepad.buttonindex);

            // ---------------- For debug only --------------------------
            // LivingRoom_ChallengeImage clickedImage = _scrollBar.GetImageAtIndex(_buttonIndex);
            // string imageName = clickedImage.GetComponent<Image>().sprite.name;
            // Debug.Log($"Image: {imageName}\nIndex: {_buttonIndex}");
            // ----------------------------------------------------------

            int clickedIndex = transform.GetSiblingIndex();
            _scrollBar.HandleImageClick(clickedIndex, CurrentCss ?? AssociatedCss);

            // Update after the check
            _previousbuttonindex = _buttonIndex;
        }
    }

    /// <summary>
    /// Called when the script is initialized. Caches references and sets up the insertion preview.
    /// </summary>
    private void Awake()
    {
        _originalParent = transform.parent;
        _scrollBar = _originalParent.GetComponentInParent<LivingRoom_HorizontalScrollBar>();

        notepad = FindFirstObjectByType<LivingRoom_Notepad>();
        if (notepad != null) OnAnyImageClicked += notepad.SetCssText;

        Completed = false;
        Locked = true;
    }

    /// <summary>
    /// Called when the object is destroyed. Cleans up resources.
    /// </summary>
    private void OnDestroy()
    {
        if (notepad != null) OnAnyImageClicked -= notepad.SetCssText;
    }
}
