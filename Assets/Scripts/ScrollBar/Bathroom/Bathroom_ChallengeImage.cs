using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Bathroom_ChallengeImage : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// The default css
    /// </summary>
    public string AssociatedCss { get; set; }

    /// <summary>
    /// The action of when the image is clicked
    /// </summary>
    public static event Action<string> OnAnyImageClicked;

    /// <summary>
    /// The index of the associated css
    /// </summary>
    public int AssociatedIndex;

    /// <summary>
    /// The index of the button in the scroll area.
    /// </summary>
    public int _buttonIndex;

    /// <summary>
    /// The original parent of the image.
    /// </summary>
    private Transform _originalParent;

    /// <summary>
    /// Reference to the horizontal scroll bar.
    /// </summary>
    private Bathroom_HorizontalScrollBar _scrollBar;

    /// <summary>
    /// The notepad reference
    /// </summary>
    private Bathroom_Notepad notepad;

    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="image">The image of the "button"</param>
    /// <param name="associatedCss">The image's default css</param>
    public Bathroom_ChallengeImage(GameObject image, string associatedCss) : base()
    {
        _scrollBar.imagePrefab = image;
        AssociatedCss = associatedCss;
    }

    private void Awake()
    {
        _originalParent = transform.parent;
        _scrollBar = _originalParent.GetComponentInParent<Bathroom_HorizontalScrollBar>();

        notepad = FindFirstObjectByType<Bathroom_Notepad>();
        if (notepad == null) OnAnyImageClicked += notepad.SetCssText;
    }

    /// <summary>
    /// Handles pointer click events on the draggable image.
    /// </summary>
    /// <param name="eventData">Pointer event data containing information about the click.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        _buttonIndex = transform.GetSiblingIndex();
        notepad.buttonindex = _buttonIndex;
        Bathroom_ChallengeImage clickedImage = _scrollBar.GetImageAtIndex(_buttonIndex);
        string imageName = clickedImage.GetComponent<Image>().sprite.name;
        // Debug.Log($"Image: {imageName}, Index: {_buttonIndex}");

        OnAnyImageClicked?.Invoke(AssociatedCss);
    }

    /// <summary>
    /// Called when the object is destroyed. Cleans up resources.
    /// </summary>
    private void OnDestroy()
    {
        if (notepad != null) OnAnyImageClicked -= notepad.SetCssText;
    }
}
