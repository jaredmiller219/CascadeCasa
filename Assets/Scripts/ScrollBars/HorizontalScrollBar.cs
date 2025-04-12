using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

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
    private readonly List<Image> scrollImages = new();

    private void Start()
    {
        SetupLayout();
        LoadImagesFromArray();
    }

    private void SetupLayout()
    {
        if (content == null) return;

        if (!content.TryGetComponent<HorizontalLayoutGroup>(out var layoutGroup))
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
        if (!content.TryGetComponent<ContentSizeFitter>(out var contentSizeFitter))
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
        if (imgObj.TryGetComponent<Image>(out var img))
        {
            img.sprite = sprite;
            img.preserveAspect = true;
            scrollImages.Add(img);

            if (!imgObj.TryGetComponent<LayoutElement>(out var layoutElement))
                layoutElement = imgObj.AddComponent<LayoutElement>();

            layoutElement.preferredWidth = imageSize.x;
            layoutElement.preferredHeight = imageSize.y;

            // Add dragging functionality
            imgObj.AddComponent<DraggableImage>();
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

public class DraggableImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Transform originalParent;
    private bool isDragging = false;
    private bool isInWorld = false;
    private GameObject insertionPreview;
    private float insertPreviewAlpha = 0.3f;
    private int insertIndex = -1;
    private HorizontalScrollBar scrollBar; // Add this field
    private float lastInsertX; // Add this field
    private float insertionThreshold = 20f; // Add this field for minimum movement required

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        scrollBar = originalParent.GetComponentInParent<HorizontalScrollBar>(); // Get reference to scroll bar
        CreateInsertionPreview();
    }

    private void CreateInsertionPreview()
    {
        if (insertionPreview != null)
        {
            Destroy(insertionPreview);
        }

        insertionPreview = new GameObject("InsertionPreview");
        insertionPreview.transform.SetParent(canvas.transform);
        var previewImage = insertionPreview.AddComponent<Image>();
        previewImage.color = new Color(0.8f, 0.8f, 1f, insertPreviewAlpha);
        var previewRect = insertionPreview.GetComponent<RectTransform>();
        previewRect.sizeDelta = rectTransform.sizeDelta;
        insertionPreview.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        CreateInsertionPreview(); // Recreate preview on each drag start
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        // Check if we're near the scroll area
        if (IsNearScrollArea())
        {
            UpdateInsertionPreview();
        }
        else
        {
            insertionPreview.SetActive(false);
        }
    }

    private bool IsNearScrollArea()
    {
        // Get the scroll area bounds
        var scrollRect = originalParent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(scrollRect, Input.mousePosition, canvas.worldCamera);
    }

    private void UpdateInsertionPreview()
    {
        if (insertionPreview == null) return;

        var scrollRect = originalParent.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollRect, Input.mousePosition, canvas.worldCamera, out localPoint);

        insertIndex = FindNearestInsertionIndex();

        if (insertIndex >= 0)
        {
            insertionPreview.SetActive(true);
            insertionPreview.transform.SetParent(originalParent);
            insertionPreview.transform.SetSiblingIndex(insertIndex);

            var previewRect = insertionPreview.GetComponent<RectTransform>();
            previewRect.sizeDelta = rectTransform.sizeDelta;

            // Shift images right of insertion point by one position only
            for (int i = insertIndex; i < originalParent.childCount; i++)
            {
                if (originalParent.GetChild(i).gameObject != insertionPreview.gameObject)
                {
                    var childRect = originalParent.GetChild(i).GetComponent<RectTransform>();
                    // Calculate position based on index, not by adding offset
                    childRect.anchoredPosition = new Vector2((i + 1) * (rectTransform.rect.width + scrollBar.spacing), 0);
                }
            }

            // Position preview at the insertion point
            previewRect.anchoredPosition = new Vector2(insertIndex * (rectTransform.rect.width + scrollBar.spacing), 0);
        }
        else
        {
            insertionPreview.SetActive(false);
            // Reset positions
            int currentIndex = 0;
            for (int i = 0; i < originalParent.childCount; i++)
            {
                var child = originalParent.GetChild(i);
                if (child.gameObject != gameObject && child.gameObject != insertionPreview.gameObject)
                {
                    var childRect = child.GetComponent<RectTransform>();
                    childRect.anchoredPosition = new Vector2(currentIndex * (rectTransform.rect.width + scrollBar.spacing), 0);
                    currentIndex++;
                }
            }
        }
    }

    private int FindNearestInsertionIndex()
    {
        var scrollRect = originalParent.GetComponent<RectTransform>();
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollRect, Input.mousePosition, canvas.worldCamera, out localMousePos);

        // Only update if mouse has moved beyond threshold
        if (Mathf.Abs(localMousePos.x - lastInsertX) < insertionThreshold)
        {
            return insertIndex; // Keep current index if movement is small
        }

        lastInsertX = localMousePos.x;

        // Find nearest valid insertion point
        for (int i = 0; i < originalParent.childCount; i++)
        {
            var childRect = originalParent.GetChild(i).GetComponent<RectTransform>();
            if (childRect.gameObject != gameObject && childRect.gameObject != insertionPreview.gameObject)
            {
                float insertPoint = childRect.anchoredPosition.x;
                if (localMousePos.x < insertPoint)
                {
                    return i;
                }
            }
        }

        return originalParent.childCount;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (!IsOutsideScrollArea())
        {
            // Always snap to insertion point when inside panel
            isInWorld = false;
            transform.SetParent(originalParent);

            if (insertIndex >= 0)
            {
                transform.SetSiblingIndex(insertIndex);
            }
            else
            {
                // If no valid insertion point, return to original position
                rectTransform.anchoredPosition = originalPosition;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(originalParent as RectTransform);
        }
        else if (!isInWorld)
        {
            // Place in world if dragged outside panel
            isInWorld = true;
        }

        if (insertionPreview != null)
        {
            insertionPreview.SetActive(false);
        }
    }

    private bool IsOutsideScrollArea()
    {
        // Get the outer panel (parent of the scroll content)
        var outerPanel = originalParent.parent.GetComponent<RectTransform>();
        if (outerPanel == null) return false;

        // Get the panel bounds in screen space
        Vector3[] corners = new Vector3[4];
        outerPanel.GetWorldCorners(corners);

        // Check if mouse position is outside the panel bounds
        float left = corners[0].x;
        float right = corners[2].x;
        float top = corners[1].y;
        float bottom = corners[0].y;

        return Input.mousePosition.x < left ||
               Input.mousePosition.x > right ||
               Input.mousePosition.y < bottom ||
               Input.mousePosition.y > top;
    }

    private void OnDestroy()
    {
        if (insertionPreview != null)
        {
            Destroy(insertionPreview);
        }
    }
}
