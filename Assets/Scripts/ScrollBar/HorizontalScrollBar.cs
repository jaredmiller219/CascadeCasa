using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalScrollBar : MonoBehaviour
{
    [Header("References")]
    public RectTransform content; // The parent container for the scrollable content
    public GameObject imagePrefab; // Prefab used to create individual images in the scroll bar

    [Header("Layout")]
    [Tooltip("Space between images in pixels")]
    public float spacing = 20f; // Spacing between images in the horizontal layout
    [Tooltip("Size of each image in pixels")]
    public Vector2 imageSize = new(200f, 200f); // Width and height of each image

    [Header("Images")]
    public Sprite[] imageSprites; // Array of sprites to be displayed in the scroll bar
    private readonly List<Image> _scrollImages = new(); // List to keep track of instantiated image components

    private void Start()
    {
        // Initialize the layout and load the images when the script starts
        SetupLayout();
        LoadImagesFromArray();
    }

    private void SetupLayout()
    {
        // Ensure the content RectTransform is assigned
        if (content == null) return;

        // Check if the content has a HorizontalLayoutGroup component, and add one if it doesn't
        if (!content.TryGetComponent<HorizontalLayoutGroup>(out var layoutGroup))
        {
            layoutGroup = content.gameObject.AddComponent<HorizontalLayoutGroup>();
        }

        // Configure the HorizontalLayoutGroup settings
        layoutGroup.spacing = spacing; // Set the spacing between child elements
        layoutGroup.childAlignment = TextAnchor.MiddleLeft; // Align children to the left
        layoutGroup.padding = new RectOffset(10, 10, 10, 10); // Add padding around the content
        layoutGroup.childControlWidth = false; // Prevent automatic width control
        layoutGroup.childControlHeight = false; // Prevent automatic height control
        layoutGroup.childForceExpandWidth = false; // Prevent forced width expansion
        layoutGroup.childForceExpandHeight = false; // Prevent forced height expansion

        // Ensure the content has a ContentSizeFitter component, and add one if it doesn't
        if (!content.TryGetComponent<ContentSizeFitter>(out var contentSizeFitter))
        {
            contentSizeFitter = content.gameObject.AddComponent<ContentSizeFitter>();
        }

        // Configure the ContentSizeFitter to adjust size based on preferred size
        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    private void LoadImagesFromArray()
    {
        // Ensure the imageSprites array is not null
        if (imageSprites == null) return;

        // Clear any existing images before loading new ones
        ClearImages();

        // Loop through each sprite in the array and add it to the scroll bar
        foreach (Sprite sprite in imageSprites)
        {
            AddImage(sprite);
        }

        // Force Unity to update the layout to reflect the changes
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }

    private void AddImage(Sprite sprite)
    {
        // Ensure the content and imagePrefab are assigned
        if (content == null || imagePrefab == null)
        {
            Debug.LogError("Content or image prefab not assigned!");
            return;
        }

        // Instantiate a new image object from the prefab and parent it to the content
        GameObject imgObj = Instantiate(imagePrefab, content);

        // Check if the instantiated object has an Image component
        if (imgObj.TryGetComponent<Image>(out var img))
        {
            // Assign the sprite to the Image component and preserve its aspect ratio
            img.sprite = sprite;
            img.preserveAspect = true;

            // Add the Image component to the list of scroll images
            _scrollImages.Add(img);

            // Ensure the object has a LayoutElement component, and add one if it doesn't
            if (!imgObj.TryGetComponent<LayoutElement>(out var layoutElement))
                layoutElement = imgObj.AddComponent<LayoutElement>();

            // Set the preferred width and height for the LayoutElement
            layoutElement.preferredWidth = imageSize.x;
            layoutElement.preferredHeight = imageSize.y;

            // Add a DraggableImage component to make the image draggable (custom behavior)
            imgObj.AddComponent<DraggableImage>();
        }
    }

    private void ClearImages()
    {
        // Loop through the list of images and destroy their GameObjects
        foreach (var img in _scrollImages.Where(img => img != null))
        {
            Destroy(img.gameObject);
        }

        // Clear the list of images
        _scrollImages.Clear();
    }
}
