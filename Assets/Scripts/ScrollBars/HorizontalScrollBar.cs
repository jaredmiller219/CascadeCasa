using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HorizontalScrollBar : MonoBehaviour
{
    [Header("References")]
    public RectTransform content;
    public GameObject imagePrefab;

    [Header("Layout")]
    [Tooltip("Space between images in pixels")]
    public float spacing = 20f;
    [Tooltip("Size of each image in pixels")]
    public Vector2 imageSize = new(200f, 200f);

    [Header("Images")]
    public Sprite[] imageSprites;
    private readonly List<Image> scrollImages = new List<Image>();

    private void Start()
    {
        SetupLayout();
        LoadImagesFromArray();
    }

    private void SetupLayout()
    {
        if (content == null) return;

        var layoutGroup = content.GetComponent<HorizontalLayoutGroup>();
        if (layoutGroup == null)
        {
            layoutGroup = content.gameObject.AddComponent<HorizontalLayoutGroup>();
        }

        // Update layout group settings
        layoutGroup.spacing = spacing;
        layoutGroup.childAlignment = TextAnchor.MiddleLeft;
        layoutGroup.padding = new RectOffset(10, 10, 10, 10);
        layoutGroup.childControlWidth = false;
        layoutGroup.childControlHeight = false;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;

        // Ensure content size fitter is present
        var contentSizeFitter = content.GetComponent<ContentSizeFitter>();
        if (contentSizeFitter == null)
        {
            contentSizeFitter = content.gameObject.AddComponent<ContentSizeFitter>();
        }
        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;  // Added vertical fit
    }

    private void LoadImagesFromArray()
    {
        if (imageSprites == null) return;
        ClearImages(); // Clear existing images first

        foreach (Sprite sprite in imageSprites)
        {
            AddImage(sprite);
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }

    public void AddImage(Sprite sprite)
    {
        if (content == null || imagePrefab == null)
        {
            Debug.LogError("Content or image prefab not assigned!");
            return;
        }

        GameObject imgObj = Instantiate(imagePrefab, content);
        Image img = imgObj.GetComponent<Image>();
        if (img != null)
        {
            img.sprite = sprite;
            img.preserveAspect = true; // Changed to true to maintain aspect ratio
            scrollImages.Add(img);

            var layoutElement = imgObj.GetComponent<LayoutElement>();
            if (layoutElement == null)
                layoutElement = imgObj.AddComponent<LayoutElement>();

            layoutElement.preferredWidth = imageSize.x;
            layoutElement.preferredHeight = imageSize.y;
        }
    }

    public void ClearImages()
    {
        foreach (var img in scrollImages)
        {
            if (img != null)
            {
                Destroy(img.gameObject);
            }
        }
        scrollImages.Clear();
    }
}
