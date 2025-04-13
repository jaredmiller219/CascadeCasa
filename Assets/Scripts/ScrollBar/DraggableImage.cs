using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private Vector2 _originalPosition;
    private Transform _originalParent;
    private GameObject _insertionPreview;
    private HorizontalScrollBar _scrollBar;
    private bool _isDragging;
    private bool _isInWorld;
    private int _insertIndex = -1;
    private float _lastInsertX;
    private const float InsertPreviewAlpha = 0.3f;
    private const float InsertionThreshold = 20f;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _originalPosition = _rectTransform.anchoredPosition;
        _originalParent = transform.parent;
        _scrollBar = _originalParent.GetComponentInParent<HorizontalScrollBar>();
        CreateInsertionPreview();
    }

    private void CreateInsertionPreview()
    {
        if (_insertionPreview != null)
        {
            Destroy(_insertionPreview);
        }

        _insertionPreview = new GameObject("InsertionPreview");
        _insertionPreview.transform.SetParent(_canvas.transform);
        var previewImage = _insertionPreview.AddComponent<Image>();
        previewImage.color = new Color(0.8f, 0.8f, 1f, InsertPreviewAlpha);
        var previewRect = _insertionPreview.GetComponent<RectTransform>();
        previewRect.sizeDelta = _rectTransform.sizeDelta;
        _insertionPreview.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
        transform.SetParent(_canvas.transform);
        transform.SetAsLastSibling();

        // Recreate preview on each drag start
        CreateInsertionPreview();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;

        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;

        // Check if we're near the scroll area
        if (IsNearScrollArea())
        {
            UpdateInsertionPreview();
        }
        else
        {
            _insertionPreview.SetActive(false);
        }
    }

    private bool IsNearScrollArea()
    {
        // Get the scroll area bounds
        var scrollRect = _originalParent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(scrollRect, Input.mousePosition, _canvas.worldCamera);
    }

    private void UpdateInsertionPreview()
    {
        if (_insertionPreview == null) return;

        var scrollRect = _originalParent.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollRect, Input.mousePosition, _canvas.worldCamera, out _);

        _insertIndex = FindNearestInsertionIndex();

        if (_insertIndex >= 0)
        {
            _insertionPreview.SetActive(true);
            _insertionPreview.transform.SetParent(_originalParent);
            _insertionPreview.transform.SetSiblingIndex(_insertIndex);

            var previewRect = _insertionPreview.GetComponent<RectTransform>();
            previewRect.sizeDelta = _rectTransform.sizeDelta;

            // Set anchors to center
            previewRect.anchorMin = new Vector2(0, 0.5f);
            previewRect.anchorMax = new Vector2(0, 0.5f);
            previewRect.pivot = new Vector2(0, 0.5f);

            // Shift images right of insertion point by one position only
            for (var i = _insertIndex; i < _originalParent.childCount; i++)
            {
                if (_originalParent.GetChild(i).gameObject != _insertionPreview)
                {
                    var childRect = _originalParent.GetChild(i).GetComponent<RectTransform>();
                    // Calculate position based on index, not by adding offset
                    childRect.anchoredPosition = new Vector2((i + 1) * (_rectTransform.rect.width + _scrollBar.spacing), 0);
                }
            }

            // Position preview at the insertion point with vertical center alignment
            previewRect.anchoredPosition = new Vector2(
                _insertIndex * (_rectTransform.rect.width + _scrollBar.spacing),
                0
            );
        }
        else
        {
            _insertionPreview.SetActive(false);
            // Reset positions
            var currentIndex = 0;
            for (var i = 0; i < _originalParent.childCount; i++)
            {
                var child = _originalParent.GetChild(i);
                if (child.gameObject != gameObject && child.gameObject != _insertionPreview)
                {
                    var childRect = child.GetComponent<RectTransform>();
                    childRect.anchoredPosition = new Vector2(currentIndex * (_rectTransform.rect.width + _scrollBar.spacing), 0);
                    currentIndex++;
                }
            }
        }
    }

    private int FindNearestInsertionIndex()
    {
        var scrollRect = _originalParent.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollRect, Input.mousePosition, _canvas.worldCamera, out var localMousePos);

        // Only update if mouse has moved beyond a threshold
        if (Mathf.Abs(localMousePos.x - _lastInsertX) < InsertionThreshold)
        {
            return _insertIndex; // Keep the current index if movement is small
        }

        _lastInsertX = localMousePos.x;

        // Find nearest valid insertion point
        for (var i = 0; i < _originalParent.childCount; i++)
        {
            var childRect = _originalParent.GetChild(i).GetComponent<RectTransform>();
            if (childRect.gameObject != gameObject && childRect.gameObject != _insertionPreview)
            {
                var insertPoint = childRect.anchoredPosition.x;
                if (localMousePos.x < insertPoint)
                {
                    return i;
                }
            }
        }

        return _originalParent.childCount;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;

        if (!IsOutsideScrollArea())
        {
            // Always snap to insertion point when-inside-panel
            _isInWorld = false;
            transform.SetParent(_originalParent);

            if (_insertIndex >= 0)
            {
                transform.SetSiblingIndex(_insertIndex);
            }
            else
            {
                // If no valid insertion point, return to original position
                _rectTransform.anchoredPosition = _originalPosition;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_originalParent as RectTransform);
        }
        else if (!_isInWorld)
        {
            // Place in the world if dragged an outside panel
            _isInWorld = true;
        }

        if (_insertionPreview != null)
        {
            _insertionPreview.SetActive(false);
        }
    }

    private bool IsOutsideScrollArea()
    {
        // Get the outer panel (parent of the scroll content)
        if (!_originalParent.parent.TryGetComponent<RectTransform>(out var outerPanel)) return false;

        // Get the panel bounds in screen space
        var corners = new Vector3[4];
        outerPanel.GetWorldCorners(corners);

        // Check if mouse position is outside the panel bounds
        var left = corners[0].x;
        var right = corners[2].x;
        var top = corners[1].y;
        var bottom = corners[0].y;

        return Input.mousePosition.x < left ||
               Input.mousePosition.x > right ||
               Input.mousePosition.y < bottom ||
               Input.mousePosition.y > top;
    }

    private void OnDestroy()
    {
        if (_insertionPreview != null)
        {
            Destroy(_insertionPreview);
        }
    }
}
