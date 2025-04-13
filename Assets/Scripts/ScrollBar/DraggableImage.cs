using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// This class allows an image to be draggable within a UI canvas and provides functionality
/// for inserting the dragged image into a horizontal scroll bar at a specific position.
/// </summary>
public class DraggableImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// The parent canvas of the draggable image.
    /// </summary>
    private Canvas _canvas;

    /// <summary>
    /// The RectTransform of the draggable image.
    /// </summary>
    private RectTransform _rectTransform;

    /// <summary>
    /// The original position of the image before dragging.
    /// </summary>
    private Vector2 _originalPosition;

    /// <summary>
    /// The original parent of the image.
    /// </summary>
    private Transform _originalParent;

    /// <summary>
    /// A visual preview of where the image will be inserted.
    /// </summary>
    private GameObject _insertionPreview;

    /// <summary>
    /// Reference to the horizontal scroll bar.
    /// </summary>
    private HorizontalScrollBar _scrollBar;

    /// <summary>
    /// Whether the image is currently being dragged.
    /// </summary>
    private bool _isDragging;

    /// <summary>
    /// Whether the image has been dragged outside the scroll area.
    /// </summary>
    private bool _isInWorld;

    /// <summary>
    /// The index where the image will be inserted.
    /// </summary>
    private int _insertIndex = -1;

    /// <summary>
    /// The last X position used for insertion calculations.
    /// </summary>
    private float _lastInsertX;

    /// <summary>
    /// Transparency of the insertion preview.
    /// </summary>
    private const float InsertPreviewAlpha = 0.3f;

    /// <summary>
    /// Minimum distance to update insertion index.
    /// </summary>
    private const float InsertionThreshold = 20f;

    /// <summary>
    /// Called when the script is initialized. Caches references and sets up the insertion preview.
    /// </summary>
    private void Awake()
    {
        // Cache references to components
        // Cache the RectTransform component of the draggable image
        // This component is used to manipulate the position and size of the image within the UI
        _rectTransform = GetComponent<RectTransform>();

        // Cache the Canvas component from the parent hierarchy
        // The canvas is required to calculate drag positions and scaling
        _canvas = GetComponentInParent<Canvas>();

        // Store the original anchored position of the draggable image
        // This is used to reset the image's position if needed
        _originalPosition = _rectTransform.anchoredPosition;

        // Cache the original parent Transform of the draggable image
        // This is used to reattach the image to its original hierarchy after dragging
        _originalParent = transform.parent;

        // Cache the HorizontalScrollBar component from the parent hierarchy
        // This component is used to manage spacing and alignment within the scroll area
        _scrollBar = _originalParent.GetComponentInParent<HorizontalScrollBar>();

        // Create the insertion preview object
        CreateInsertionPreview();
    }

    /// <summary>
    /// Creates the visual insertion preview object used to indicate where the image will be inserted.
    /// </summary>
    private void CreateInsertionPreview()
    {
        // Destroy any existing preview to avoid duplicates
        if (_insertionPreview != null)
        {
            // Destroy the existing preview object if it exists
            Destroy(_insertionPreview);
        }

        // Create a new GameObject for the preview
        _insertionPreview = new GameObject("InsertionPreview");
        // Set the preview's parent to the canvas
        _insertionPreview.transform.SetParent(_canvas.transform);

        // Add an Image component to the preview and set its color
        var previewImage = _insertionPreview.AddComponent<Image>();
        // Set the preview image to be a semi-transparent blue color
        previewImage.color = new Color(0.8f, 0.8f, 1f, InsertPreviewAlpha);

        // Set the size of the preview to match the draggable image
        var previewRect = _insertionPreview.GetComponent<RectTransform>();
        // Set the anchor and pivot points of the preview to be the same as the draggable image
        previewRect.sizeDelta = _rectTransform.sizeDelta;

        // Initially hide the preview
        _insertionPreview.SetActive(false);
    }

    /// <summary>
    /// Called when the user begins dragging the image. Prepares the image for dragging.
    /// </summary>
    /// <param name="eventData">Pointer event data containing information about the drag.</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Mark the image as being dragged
        _isDragging = true;

        // Move the image to the canvas so it can be dragged freely
        transform.SetParent(_canvas.transform);

        // Ensure the image renders on top of other UI elements
        transform.SetAsLastSibling();

        // Recreate the insertion preview to ensure it's fresh
        CreateInsertionPreview();
    }

    /// <summary>
    /// Called while the user is dragging the image. Updates the image's position and handles insertion preview.
    /// </summary>
    /// <param name="eventData">Pointer event data containing information about the drag.</param>
    public void OnDrag(PointerEventData eventData)
    {
        // Do nothing if the image is not being dragged
        if (!_isDragging) return;

        // Move the image based on the drag delta, adjusted for canvas scale
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;

        // Check if the image is near the scroll area
        if (IsNearScrollArea())
        {
            // Update the insertion preview if near the scroll area
            UpdateInsertionPreview();
        }
        else
        {
            // Hide the preview if not near the scroll area
            _insertionPreview.SetActive(false);
        }
    }

    /// <summary>
    /// Determines if the mouse is near the scroll area.
    /// </summary>
    /// <returns>True if the mouse is near the scroll area, false otherwise.</returns>
    private bool IsNearScrollArea()
    {
        // Get the bounds of the scroll area
        var scrollRect = _originalParent.GetComponent<RectTransform>();

        // Check if the mouse position is within the bounds of the scroll area
        return RectTransformUtility.RectangleContainsScreenPoint(scrollRect, Input.mousePosition, _canvas.worldCamera);
    }

    /// <summary>
    /// Updates the position and visibility of the insertion preview based on the drag position.
    /// </summary>
    private void UpdateInsertionPreview()
    {
        // Do nothing if no preview exists
        if (_insertionPreview == null) return;

        // Get the RectTransform of the scroll area
        var scrollRect = _originalParent.GetComponent<RectTransform>();

        // Convert the mouse position to local coordinates within the scroll area
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollRect, Input.mousePosition, _canvas.worldCamera, out _);

        // Find the nearest valid insertion index
        _insertIndex = FindNearestInsertionIndex();

        if (_insertIndex >= 0)
        {
            // Show the preview and attach it to the scroll area
            _insertionPreview.SetActive(true);
            _insertionPreview.transform.SetParent(_originalParent);

            // Handle the case where the image is inserted at the end
            var validItemCount = 0; // Initialize a counter to track the number of valid items (excluding the dragged image and preview)

            // Iterate through all children of the original parent
            for (var i = 0; i < _originalParent.childCount; i++)
            {
                // Get the child at the current index 'i'
                var child = _originalParent.GetChild(i);

                // Check if the current child is not the dragged image itself and not the insertion preview
                if (child.gameObject != gameObject && child.gameObject != _insertionPreview)
                {
                    // Increment the count of valid items (items that are not the dragged image or the preview)
                    validItemCount++;
                }
            }

            // Set the preview's sibling index based on the insertion index
            // Check if the insertion index is at or beyond the count of valid items
            if (_insertIndex >= validItemCount)
            {
                // If the insertion index is at or beyond the valid item count:
                // - This means the dragged image is being inserted at the end of the list.
                // - Set the insertion preview to be the last sibling in the hierarchy.
                _insertionPreview.transform.SetAsLastSibling();
            }
            else
            {
                // If the insertion index is within the range of valid items:
                // - This means the dragged image is being inserted somewhere in the middle of the list.
                // - Set the sibling index of the insertion preview to match the calculated insertion index.
                _insertionPreview.transform.SetSiblingIndex(_insertIndex);
            }

            // Update the preview's size and alignment
            // Get the RectTransform component of the insertion preview
            var previewRect = _insertionPreview.GetComponent<RectTransform>();

            // Set the size of the insertion preview to match the draggable image's size
            // - sizeDelta determines the width and height of the RectTransform relative to its anchors.
            previewRect.sizeDelta = _rectTransform.sizeDelta;

            // Set the minimum anchor point of the preview to the left-center of the parent
            // - anchorMin defines the normalized position (0 to 1) of the lower-left corner of the RectTransform relative to its parent.
            // - (0, 0.5f) means the left edge of the preview is aligned with the left edge of the parent, and vertically centered.
            previewRect.anchorMin = new Vector2(0, 0.5f);

            // Set the maximum anchor point of the preview to the left-center of the parent
            // - anchorMax defines the normalized position (0 to 1) of the upper-right corner of the RectTransform relative to its parent.
            // - (0, 0.5f) ensures the right edge of the preview is also aligned with the left edge of the parent, and vertically centered.
            previewRect.anchorMax = new Vector2(0, 0.5f);

            // Set the pivot point of the preview to the left-center
            // - pivot determines the point around which the RectTransform rotates and scales.
            // - (0, 0.5f) means the pivot is at the left edge of the preview, vertically centered.
            previewRect.pivot = new Vector2(0, 0.5f);

            // Shift images to the right of the insertion point
            for (var i = _insertIndex; i < _originalParent.childCount; i++) // Loop through all children starting from the insertion index
            {
                if (_originalParent.GetChild(i).gameObject != _insertionPreview) // Skip the insertion preview object
                {
                    // Get the RectTransform of the current child
                    var childRect = _originalParent.GetChild(i).GetComponent<RectTransform>();

                    // Update the anchored position of the child:
                    // - Calculate the new X position by shifting the child one position to the right of its current index.
                    // - Multiply the index (i + 1) by the width of the draggable image (_rectTransform.rect.width) plus the spacing between items (_scrollBar.spacing).
                    // - Set the Y position to 0 to keep the child aligned vertically.
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
            // Hide the preview if no valid index
            _insertionPreview.SetActive(false);

            // Reset the positions of all items
            // Initialize a counter to track the current index for valid items
            var currentIndex = 0;

            // Iterate through all children of the original parent
            for (var i = 0; i < _originalParent.childCount; i++)
            {
                // Get the current child at index 'i'
                var child = _originalParent.GetChild(i);

                // Skip the dragged image itself and the insertion preview object
                if (child.gameObject != gameObject && child.gameObject != _insertionPreview)
                {
                    // Get the RectTransform component of the current child
                    var childRect = child.GetComponent<RectTransform>();

                    // Calculate the new anchored position for the child:
                    // - The X position is determined by multiplying the current index by the sum of the draggable image's width and the spacing between items.
                    // - The Y position is set to 0 to keep the child vertically aligned.
                    childRect.anchoredPosition = new Vector2(
                        currentIndex * (_rectTransform.rect.width + _scrollBar.spacing), // X position
                        0 // Y position
                    );

                    // Increment the current index to prepare for the next valid child
                    currentIndex++;
                }
            }
        }
    }

    /// <summary>
    /// Finds the nearest valid insertion index based on the mouse position.
    /// </summary>
    /// <returns>The nearest valid insertion index.</returns>
    private int FindNearestInsertionIndex()
    {
        var scrollRect = _originalParent.GetComponent<RectTransform>();

        // Convert the mouse position to local coordinates
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollRect, Input.mousePosition, _canvas.worldCamera, out var localMousePos);

        // Only update the index if the mouse has moved beyond a threshold
        if (Mathf.Abs(localMousePos.x - _lastInsertX) < InsertionThreshold)
        {
            return _insertIndex;
        }

        _lastInsertX = localMousePos.x; // Update the last X position

        // Count valid items (excluding the dragged item and preview)
        int validItemCount = 0;
        float rightmostPosition = float.MinValue;

        // Iterate through all children of the original parent
        for (var i = 0; i < _originalParent.childCount; i++)
        {
            // Get the child at the current index
            var child = _originalParent.GetChild(i);

            // Check if the current child is not the dragged image itself and not the insertion preview
            if (child.gameObject != gameObject && child.gameObject != _insertionPreview)
            {
            // Increment the count of valid items (items that are not the dragged image or the preview)
            validItemCount++;

            // Get the RectTransform component of the current child
            var childRect = child.GetComponent<RectTransform>();

            // Update the rightmost position:
            // - Compare the current child's anchored X position with the current rightmost position.
            // - Use Mathf.Max to store the larger value, ensuring we track the furthest X position among valid items.
            rightmostPosition = Mathf.Max(rightmostPosition, childRect.anchoredPosition.x);
            }
        }

        // If the mouse is to the right of the rightmost item, return the last index
        float rightEdgeThreshold = rightmostPosition + (_rectTransform.rect.width / 2);
        if (localMousePos.x > rightEdgeThreshold)
        {
            return validItemCount;
        }
        // Iterate through all children of the original parent to find the nearest valid insertion point
        for (var i = 0; i < _originalParent.childCount; i++) // Loop through each child in the original parent
        {
            var child = _originalParent.GetChild(i); // Get the child at the current index 'i'

            // Check if the current child is not the dragged image itself and not the insertion preview
            if (child.gameObject != gameObject && child.gameObject != _insertionPreview){
                var childRect = child.GetComponent<RectTransform>(); // Get the RectTransform of the current child

                // Calculate the potential insertion point for this child:
                // - Take the child's anchored X position (its current horizontal position in the parent).
                // - Add half the width of the draggable image (_rectTransform.rect.width / 2) to determine the midpoint of the child.
                var insertPoint = childRect.anchoredPosition.x + (_rectTransform.rect.width / 2);

                // Check if the mouse's local X position is less than the calculated insertion point:
                // - If true, this means the mouse is positioned to the left of this child's midpoint,
                //   making this a valid insertion index.
                if (localMousePos.x < insertPoint){
                    return i; // Return the current index 'i' as the nearest valid insertion point
                }
            }
        }

        return validItemCount; // Default to the last index
    }

    /// <summary>
    /// Called when the user ends dragging the image. Handles snapping or placement logic.
    /// </summary>
    /// <param name="eventData">Pointer event data containing information about the drag.</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        // Mark as no longer dragging
        // This flag indicates that the dragging operation has ended
        _isDragging = false;

        // Check if the dragged image is still within the scroll area
        if (!IsOutsideScrollArea())
        {
            // If the image is inside the scroll area, reset the "in-world" flag
            // This flag tracks whether the image has been placed outside the scrollable UI
            _isInWorld = false;

            // Reattach the image to its original parent (the scroll area)
            transform.SetParent(_originalParent);

            // Check if a valid insertion index was determined during the drag
            if (_insertIndex >= 0)
            {
                // If a valid insertion index exists, set the sibling index of the dragged image
                // This determines the position of the image within the scroll area's hierarchy
                transform.SetSiblingIndex(_insertIndex);
            }
            else
            {
                // If no valid insertion index was found, reset the image to its original position
                // This ensures the image snaps back to where it started
                _rectTransform.anchoredPosition = _originalPosition;
            }

            // Force a layout rebuild for the scroll area
            // This ensures that the UI updates to reflect the new arrangement of elements
            LayoutRebuilder.ForceRebuildLayoutImmediate(_originalParent as RectTransform);
        }
        else if (!_isInWorld)
        {
            // If the image was dragged outside the scroll area and is not already marked as "in-world"
            // This block handles the transition of the image to a "world" state
            _isInWorld = true; // Mark the image as placed in the world
        }

        // Hide the insertion preview
        // The preview is no longer needed since the drag operation has ended
        if (_insertionPreview != null)
        {
            _insertionPreview.SetActive(false); // Deactivate the preview object
        }
    }

    /// <summary>
    /// Determines if the mouse is outside the scroll area.
    /// </summary>
    /// <returns>True if the mouse is outside the scroll area, false otherwise.</returns>
    private bool IsOutsideScrollArea()
    {
        // Get the outer panel (parent of the scroll content)
        if (!_originalParent.parent.TryGetComponent<RectTransform>(out var outerPanel)) return false;

        // Get the panel bounds in screen space
        var corners = new Vector3[4];
        outerPanel.GetWorldCorners(corners);

        // Check if the mouse position is outside the panel bounds
        var left = corners[0].x;
        var right = corners[2].x;
        var top = corners[1].y;
        var bottom = corners[0].y;

        // Check if the mouse position is outside the bounds of the outer panel
        // This condition checks if the mouse position is outside the left, right, top, or bottom edges of the panel
        return Input.mousePosition.x < left ||
               Input.mousePosition.x > right ||
               Input.mousePosition.y < bottom ||
               Input.mousePosition.y > top;
    }

    /// <summary>
    /// Called when the object is destroyed. Cleans up resources.
    /// </summary>
    private void OnDestroy()
    {
        // Destroy the insertion preview if it exists
        if (_insertionPreview != null)
        {
            // Destroy the insertion preview object to free up resources
            Destroy(_insertionPreview);
        }
    }
}
