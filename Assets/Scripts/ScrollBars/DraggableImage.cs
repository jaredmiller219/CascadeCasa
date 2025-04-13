using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Transform originalParent;
    private GameObject insertionPreview;
    private HorizontalScrollBar scrollBar;
    private bool isDragging = false;
    private bool isInWorld = false;
    private int insertIndex = -1;
    private float lastInsertX;
    private readonly float insertPreviewAlpha = 0.3f;
    private readonly float insertionThreshold = 20f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        scrollBar = originalParent.GetComponentInParent<HorizontalScrollBar>();
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

        // Recreate preview on each drag start
        CreateInsertionPreview();
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
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollRect, Input.mousePosition, canvas.worldCamera, out Vector2 localPoint);

        insertIndex = FindNearestInsertionIndex();

        if (insertIndex >= 0)
        {
            insertionPreview.SetActive(true);
            insertionPreview.transform.SetParent(originalParent);
            insertionPreview.transform.SetSiblingIndex(insertIndex);

            var previewRect = insertionPreview.GetComponent<RectTransform>();
            previewRect.sizeDelta = rectTransform.sizeDelta;

            // Set anchors to center
            previewRect.anchorMin = new Vector2(0, 0.5f);
            previewRect.anchorMax = new Vector2(0, 0.5f);
            previewRect.pivot = new Vector2(0, 0.5f);

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

            // Position preview at the insertion point with vertical center alignment
            previewRect.anchoredPosition = new Vector2(
                insertIndex * (rectTransform.rect.width + scrollBar.spacing),
                0
            );
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
