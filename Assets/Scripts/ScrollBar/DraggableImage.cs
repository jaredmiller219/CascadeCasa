using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// This class allows an image to be draggable within a UI canvas and provides functionality
// for inserting the dragged image into a horizontal scroll bar at a specific position.
public class DraggableImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // References to the canvas, rect transform, and other necessary components
    private Canvas _canvas; // The parent canvas of the draggable image
    private RectTransform _rectTransform; // The RectTransform of the draggable image
    private Vector2 _originalPosition; // The original position of the image before dragging
    private Transform _originalParent; // The original parent of the image
    private GameObject _insertionPreview; // A visual preview of where the image will be inserted
    private HorizontalScrollBar _scrollBar; // Reference to the horizontal scroll bar
    private bool _isDragging; // Whether the image is currently being dragged
    private bool _isInWorld; // Whether the image has been dropped outside the scroll area
    private int _insertIndex = -1; // The index where the image will be inserted
    private float _lastInsertX; // The last X position used for insertion calculations

    // Constants for insertion preview appearance and behavior
    private const float InsertPreviewAlpha = 0.3f; // Transparency of the insertion preview
    private const float InsertionThreshold = 20f; // Minimum movement threshold for updating insertion index

    // Called when the script is initialized
    private void Awake()
    {
        // Cache references to necessary components
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _originalPosition = _rectTransform.anchoredPosition;
        _originalParent = transform.parent;
        _scrollBar = _originalParent.GetComponentInParent<HorizontalScrollBar>();

        // Create the insertion preview object
        CreateInsertionPreview();
    }

    // Creates a visual preview object for showing where the image will be inserted
    private void CreateInsertionPreview()
    {
        // Destroy any existing preview object
        if (_insertionPreview != null)
        {
            Destroy(_insertionPreview);
        }

        // Create a new GameObject for the preview
        _insertionPreview = new GameObject("InsertionPreview");
        _insertionPreview.transform.SetParent(_canvas.transform);

        // Add an Image component to the preview and set its color
        var previewImage = _insertionPreview.AddComponent<Image>();
        previewImage.color = new Color(0.8f, 0.8f, 1f, InsertPreviewAlpha);

        // Set the size of the preview to match the draggable image
        var previewRect = _insertionPreview.GetComponent<RectTransform>();
        previewRect.sizeDelta = _rectTransform.sizeDelta;

        // Initially hide the preview
        _insertionPreview.SetActive(false);
    }

    // Called when the user begins dragging the image
    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;

        // Move the image to the canvas so it can be dragged freely
        transform.SetParent(_canvas.transform);
        transform.SetAsLastSibling();

        // Recreate the insertion preview at the start of each drag
        CreateInsertionPreview();
    }

    // Called while the user is dragging the image
    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;

        // Update the position of the image based on the drag delta
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;

        // Check if the image is near the scroll area
        if (IsNearScrollArea())
        {
            // Update the insertion preview if near the scroll area
            UpdateInsertionPreview();
        }
        else
        {
            // Hide the insertion preview if not near the scroll area
            _insertionPreview.SetActive(false);
        }
    }

    // Determines if the image is near the scroll area
    private bool IsNearScrollArea()
    {
        // Get the bounds of the scroll area
        var scrollRect = _originalParent.GetComponent<RectTransform>();

        // Check if the mouse position is within the bounds of the scroll area
        return RectTransformUtility.RectangleContainsScreenPoint(scrollRect, Input.mousePosition, _canvas.worldCamera);
    }

    // Updates the position and visibility of the insertion preview
    private void UpdateInsertionPreview()
    {
        if (_insertionPreview == null) return;

        // Get the local mouse position relative to the scroll area
        var scrollRect = _originalParent.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollRect, Input.mousePosition, _canvas.worldCamera, out _);

        // Find the nearest valid insertion index
        _insertIndex = FindNearestInsertionIndex();

        if (_insertIndex >= 0)
        {
            // Show and position the insertion preview at the calculated index
            _insertionPreview.SetActive(true);
            _insertionPreview.transform.SetParent(_originalParent);
            _insertionPreview.transform.SetSiblingIndex(_insertIndex);

            var previewRect = _insertionPreview.GetComponent<RectTransform>();
            previewRect.sizeDelta = _rectTransform.sizeDelta;

            // Set anchors and pivot for vertical center alignment
            previewRect.anchorMin = new Vector2(0, 0.5f);
            previewRect.anchorMax = new Vector2(0, 0.5f);
            previewRect.pivot = new Vector2(0, 0.5f);

            // Shift images to the right of the insertion point
            for (var i = _insertIndex; i < _originalParent.childCount; i++)
            {
                if (_originalParent.GetChild(i).gameObject != _insertionPreview.gameObject)
                {
                    var childRect = _originalParent.GetChild(i).GetComponent<RectTransform>();
                    childRect.anchoredPosition = new Vector2((i + 1) * (_rectTransform.rect.width + _scrollBar.spacing), 0);
                }
            }

            // Position the preview at the insertion point
            previewRect.anchoredPosition = new Vector2(
                _insertIndex * (_rectTransform.rect.width + _scrollBar.spacing),
                0
            );
        }
        else
        {
            // Hide the preview and reset positions if no valid insertion point is found
            _insertionPreview.SetActive(false);
            var currentIndex = 0;
            for (var i = 0; i < _originalParent.childCount; i++)
            {
                var child = _originalParent.GetChild(i);
                if (child.gameObject != gameObject && child.gameObject != _insertionPreview.gameObject)
                {
                    var childRect = child.GetComponent<RectTransform>();
                    childRect.anchoredPosition = new Vector2(currentIndex * (_rectTransform.rect.width + _scrollBar.spacing), 0);
                    currentIndex++;
                }
            }
        }
    }

    // Finds the nearest valid insertion index based on the mouse position
    private int FindNearestInsertionIndex()
    {
        var scrollRect = _originalParent.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollRect, Input.mousePosition, _canvas.worldCamera, out var localMousePos);

        // Only update the index if the mouse has moved beyond a threshold
        if (Mathf.Abs(localMousePos.x - _lastInsertX) < InsertionThreshold)
        {
            return _insertIndex; // Keep the current index if movement is small
        }

        _lastInsertX = localMousePos.x;

        // Iterate through the children to find the nearest insertion point
        for (var i = 0; i < _originalParent.childCount; i++)
        {
            var childRect = _originalParent.GetChild(i).GetComponent<RectTransform>();
            if (childRect.gameObject != gameObject && childRect.gameObject != _insertionPreview.gameObject)
            {
                var insertPoint = childRect.anchoredPosition.x;
                if (localMousePos.x < insertPoint)
                {
                    return i;
                }
            }
        }

        // If no valid point is found, return the last index
        return _originalParent.childCount;
    }

    // Called when the user stops dragging the image
    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;

        if (!IsOutsideScrollArea())
        {
            // Snap the image to the insertion point if inside the scroll area
            _isInWorld = false;
            transform.SetParent(_originalParent);

            if (_insertIndex >= 0)
            {
                transform.SetSiblingIndex(_insertIndex);
            }
            else
            {
                // Return to the original position if no valid insertion point
                _rectTransform.anchoredPosition = _originalPosition;
            }

            // Force a layout rebuild to update the UI
            LayoutRebuilder.ForceRebuildLayoutImmediate(_originalParent as RectTransform);
        }
        else if (!_isInWorld)
        {
            // Place the image in the world if dragged outside the scroll area
            _isInWorld = true;
        }

        // Hide the insertion preview
        if (_insertionPreview != null)
        {
            _insertionPreview.SetActive(false);
        }
    }

    // Determines if the image is outside the scroll area
    private bool IsOutsideScrollArea()
    {
        // Get the bounds of the outer panel (parent of the scroll content)
        var outerPanel = _originalParent.parent.GetComponent<RectTransform>();
        if (outerPanel == null) return false;

        // Get the panel bounds in screen space
        var corners = new Vector3[4];
        outerPanel.GetWorldCorners(corners);

        // Check if the mouse position is outside the panel bounds
        var left = corners[0].x;
        var right = corners[2].x;
        var top = corners[1].y;
        var bottom = corners[0].y;

        return Input.mousePosition.x < left ||
               Input.mousePosition.x > right ||
               Input.mousePosition.y < bottom ||
               Input.mousePosition.y > top;
    }

    // Called when the object is destroyed
    private void OnDestroy()
    {
        // Destroy the insertion preview if it exists
        if (_insertionPreview != null)
        {
            Destroy(_insertionPreview);
        }
    }
}
