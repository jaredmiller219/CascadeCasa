using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class that manages a horizontal scroll bar with dynamically loaded images.
/// </summary>
public class HorizontalScrollBar : MonoBehaviour
{
    /// <summary>
    /// The RectTransform that serves as the parent container for the scrollable content.
    /// </summary>
    [Tooltip("The parent container for the scrollable content.")]
    [Header("References")]
    public RectTransform content; // The parent container for the scrollable content.

    /// <summary>
    /// The prefab used to create individual images in the scroll bar.
    /// </summary>
    [Tooltip("Prefab used to create individual images in the scroll bar.")]
    public GameObject imagePrefab; // Prefab used to create individual images in the scroll bar.

    /// <summary>
    /// The spacing between images in the scroll bar.
    /// </summary>
    [Tooltip("Space between images in pixels.")]
    [Header("Layout")]
    public float spacing;

    /// <summary>
    /// The size of each image in the scroll bar.
    /// </summary>
    [Tooltip("Size of each image in pixels.")]
    public Vector2 imageSize; // Size of each image in pixels.

    /// <summary>
    /// The array of sprites to be displayed in the scroll bar.
    /// </summary>
    [Tooltip("Array of sprites to be displayed in the scroll bar.")]
    [Header("Images")]
    public Sprite[] imageSprites; // Array of sprites to be displayed in the scroll bar.

    /// <summary>
    /// A list to keep track of instantiated image components.
    /// </summary>
    [Tooltip("List to keep track of instantiated image components.")]
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

        // Add a HorizontalLayoutGroup component if it doesn't exist.
        if (!content.TryGetComponent<HorizontalLayoutGroup>(out var layoutGroup))
        {
            layoutGroup = content.gameObject.AddComponent<HorizontalLayoutGroup>();
        }

        // Configure the layout group to align children and set padding.
        layoutGroup.childAlignment = TextAnchor.MiddleLeft; // Align children to the left.
        layoutGroup.padding = new RectOffset(10, 10, 30, 10); // left, right, top, bottom
        layoutGroup.childControlWidth = false; // Disable automatic width control for children.
        layoutGroup.childControlHeight = false; // Disable automatic height control for children.
        layoutGroup.childForceExpandWidth = false; // Prevent forced width expansion.
        layoutGroup.childForceExpandHeight = false; // Prevent forced height expansion.

        // Add a ContentSizeFitter component if it doesn't exist.
        if (!content.TryGetComponent<ContentSizeFitter>(out var contentSizeFitter))
        {
            contentSizeFitter = content.gameObject.AddComponent<ContentSizeFitter>();
        }

        // Configure the ContentSizeFitter to adjust size based on preferred size.
        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        UpdateLayout(); // Update the layout to apply spacing.
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

            // Set the RectTransform size directly.
            RectTransform rectTransform = imgObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(imageSize.x * 2, imageSize.y * 2); // Set the size of the image.

            // Remove LayoutElement if it exists.
            if (imgObj.TryGetComponent<LayoutElement>(out var existingLayout))
            {
                DestroyImmediate(existingLayout); // Remove the LayoutElement component.
            }

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

    /// <summary>
    /// Called when a value in the inspector is changed.
    /// Updates the image sizes if the application is playing.
    /// </summary>
    private void OnValidate()
    {
        if (!Application.isPlaying) return; // Only update if the application is running.
        UpdateImageSizes(); // Update the sizes of all images.
    }

    /// <summary>
    /// Updates the sizes of all images in the scroll bar.
    /// Ensures the RectTransform size matches the specified image size.
    /// </summary>
    private void UpdateImageSizes()
    {
        // Loop through all active images in the scroll bar.
        foreach (var img in _scrollImages)
        {
            if (img != null)
            {
                RectTransform rectTransform = img.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(imageSize.x * 2, imageSize.y * 2); // Update the size.
            }
        }

        // Force layout update to reflect changes.
        if (content != null)
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        }
    }

    /// <summary>
    /// Updates the layout of the scroll bar's content container.
    /// Adjusts the spacing between images.
    /// </summary>
    private void UpdateLayout()
    {
        if (content == null) return; // Exit if the content RectTransform is not assigned.

        // Update the spacing in the HorizontalLayoutGroup.
        if (content.TryGetComponent<HorizontalLayoutGroup>(out var layoutGroup))
        {
            layoutGroup.spacing = spacing; // Set the spacing between images.
        }

        // Force layout update to apply changes.
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }

    /// <summary>
    /// Retrieves the image at the specified index from the scroll bar.
    /// </summary>
    /// <param name="index">The index of the image to retrieve.</param>
    /// <returns>The Image component at the specified index, or null if the index is out of range.</returns>
    public DraggableImage GetImageAtIndex(int index)
    {
        if (index < 0 || index >= _scrollImages.Count)
        {
            Debug.LogError($"Index {index} is out of range!"); // Log an error if the index is invalid.
            return null; // Return null if the index is out of range.
        }

        // return image at the specified index.
        if (_scrollImages[index].TryGetComponent<DraggableImage>(out var draggableImage))
        {
            return draggableImage; // Return the DraggableImage component at the specified index.
        }
        Debug.LogError($"No DraggableImage component found at index {index}!"); // Log an error if no DraggableImage is found.
        return null; // Return null if no DraggableImage is found.
    }
}
