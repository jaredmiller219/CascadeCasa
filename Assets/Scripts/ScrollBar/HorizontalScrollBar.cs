using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class that manages a horizontal scroll bar with dynamically loaded images.
/// </summary>
public class HorizontalScrollBar : MonoBehaviour
{
    [Header("References")]
    /// <summary>
    /// The parent container for the scrollable content.
    /// This is typically a RectTransform that holds all the child elements (images).
    /// </summary>
    public RectTransform content;

    /// <summary>
    /// Prefab used to create individual images in the scroll bar.
    /// This prefab should have an Image component and optionally other components like LayoutElement.
    /// </summary>
    public GameObject imagePrefab;

    [Header("Layout")]
    /// <summary>
    /// Space between images in pixels.
    /// This determines the horizontal gap between each image in the scroll bar.
    /// </summary>
    [Tooltip("Space between images in pixels")]
    public float spacing = 20f;

    /// <summary>
    /// Size of each image in pixels (width and height).
    /// This controls the dimensions of the images displayed in the scroll bar.
    /// </summary>
    [Tooltip("Size of each image in pixels")]
    public Vector2 imageSize = new(200f, 200f);

    [Header("Images")]
    /// <summary>
    /// Array of sprites to be displayed in the scroll bar.
    /// These sprites are used to populate the scroll bar dynamically.
    /// </summary>
    public Sprite[] imageSprites;

    /// <summary>
    /// List to keep track of instantiated image components.
    /// This is used to manage and clear the images when needed.
    /// </summary>
    private readonly List<Image> _scrollImages = new();

    /// <summary>
    /// Unity's Start method, called when the script is first initialized.
    /// Sets up the layout and loads the images into the scroll bar.
    /// </summary>
    private void Start()
    {
        SetupLayout(); // Configures the layout of the scroll bar.
        LoadImagesFromArray(); // Loads the images from the provided sprite array.
    }

    /// <summary>
    /// Configures the layout of the scroll bar's content container.
    /// Ensures the necessary components (HorizontalLayoutGroup and ContentSizeFitter) are present and properly configured.
    /// </summary>
    private void SetupLayout()
    {
        if (content == null) return; // Exit if the content RectTransform is not assigned.

        // Check if the content has a HorizontalLayoutGroup component.
        // If not, add one to manage the horizontal layout of child elements.
        if (!content.TryGetComponent<HorizontalLayoutGroup>(out var layoutGroup))
        {
            layoutGroup = content.gameObject.AddComponent<HorizontalLayoutGroup>();
        }

        // Configure the HorizontalLayoutGroup settings.
        layoutGroup.spacing = spacing; // Set the spacing between child elements.
        layoutGroup.childAlignment = TextAnchor.MiddleLeft; // Align children to the left.
        layoutGroup.padding = new RectOffset(10, 10, 10, 10); // Add padding around the content.
        layoutGroup.childControlWidth = false; // Disable automatic width control for children.
        layoutGroup.childControlHeight = false; // Disable automatic height control for children.
        layoutGroup.childForceExpandWidth = false; // Prevent forced width expansion.
        layoutGroup.childForceExpandHeight = false; // Prevent forced height expansion.

        // Ensure the content has a ContentSizeFitter component.
        // If not, add one to adjust the size of the content based on its children.
        if (!content.TryGetComponent<ContentSizeFitter>(out var contentSizeFitter))
        {
            contentSizeFitter = content.gameObject.AddComponent<ContentSizeFitter>();
        }

        // Configure the ContentSizeFitter to adjust size based on preferred size.
        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    /// <summary>
    /// Loads images from the provided sprite array and adds them to the scroll bar.
    /// Clears any existing images before loading new ones.
    /// </summary>
    private void LoadImagesFromArray()
    {
        if (imageSprites == null) return; // Exit if the imageSprites array is null.

        ClearImages(); // Remove any existing images from the scroll bar.

        // Loop through each sprite in the array and add it to the scroll bar.
        foreach (Sprite sprite in imageSprites)
        {
            AddImage(sprite); // Add an individual image to the scroll bar.
        }

        // Force Unity to update the layout to reflect the changes.
        Canvas.ForceUpdateCanvases(); // Ensure the canvas updates immediately.
        LayoutRebuilder.ForceRebuildLayoutImmediate(content); // Rebuild the layout of the content.
    }

    /// <summary>
    /// Adds a single image to the scroll bar using the provided sprite.
    /// Instantiates a new GameObject from the imagePrefab and configures it.
    /// </summary>
    /// <param name="sprite">The sprite to be displayed in the image.</param>
    private void AddImage(Sprite sprite)
    {
        if (content == null || imagePrefab == null)
        {
            Debug.LogError("Content or image prefab not assigned!"); // Log an error if either is missing.
            return;
        }

        // Instantiate a new image object from the prefab and parent it to the content.
        GameObject imgObj = Instantiate(imagePrefab, content);

        // Check if the instantiated object has an Image component.
        if (imgObj.TryGetComponent<Image>(out var img))
        {
            img.sprite = sprite; // Assign the sprite to the Image component.
            img.preserveAspect = true; // Preserve the aspect ratio of the sprite.

            _scrollImages.Add(img); // Add the Image component to the list of scroll images.

            // Ensure the object has a LayoutElement component.
            // If not, add one to control its layout properties.
            if (!imgObj.TryGetComponent<LayoutElement>(out var layoutElement))
                layoutElement = imgObj.AddComponent<LayoutElement>();

            // Set the preferred width and height for the LayoutElement.
            layoutElement.preferredWidth = imageSize.x;
            layoutElement.preferredHeight = imageSize.y;

            // Add a custom DraggableImage component to make the image draggable.
            imgObj.AddComponent<DraggableImage>();
        }
    }

    /// <summary>
    /// Clears all images currently displayed in the scroll bar.
    /// Destroys the GameObjects associated with the images and clears the list.
    /// </summary>
    private void ClearImages()
    {
        // Loop through the list of images and destroy their GameObjects.
        foreach (var img in _scrollImages.Where(img => img != null))
        {
            Destroy(img.gameObject); // Destroy the GameObject associated with the image.
        }

        _scrollImages.Clear(); // Clear the list of images.
    }
}
