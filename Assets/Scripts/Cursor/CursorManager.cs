// CursorManager.cs

using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// CursorManager is a Unity MonoBehaviour that manages the cursor appearance
/// when the mouse pointer enters or exits a UI element.
/// It changes the cursor to a text cursor when the pointer is over the object
/// and resets it to the currently selected cursor in the CursorDropdown script
/// when the pointer exits.
/// </summary>
/// <remarks>
/// This script requires the UnityEngine.EventSystems namespace for pointer events.
/// </remarks>
public class CursorManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // The header for the i-beam cursor
    [Header("Cursors")]

    public Texture2D textHoverCursor;
    [Tooltip("The texture for the text cursor (i-beam)")]

    // Reference to the CursorDropdown script
    // This script is used to get the currently selected cursor from the dropdown.
    // [SerializeField]
    private CursorType _cursorDropdown;

    // private static CursorType _sharedDropdown;

    // private void Awake()
    // {
    //     if (_cursorDropdown != null)
    //     {
    //         _sharedDropdown = _cursorDropdown;
    //     }
    // }

    // public void ResetToDefaultCursor()
    // {
    //     // Try both the instance and shared reference
    //     CursorType dropdownToUse = _cursorDropdown != null ? _cursorDropdown : _sharedDropdown;

    //     if (dropdownToUse != null)
    //     {
    //         Debug.Log("Resetting cursor using dropdown");
    //         var selectedCursor = dropdownToUse.GetSelectedCursor();
    //         Cursor.SetCursor(selectedCursor, Vector2.zero, CursorMode.Auto);
    //     }
    //     else
    //     {
    //         Debug.LogWarning("No cursor dropdown reference available!");
    //     }
    // }

    /// <summary>
    /// The hotspot for the cursor, which is the point within the cursor image
    /// that will be used to click on objects.
    /// </summary>
    /// <remarks>
    /// The hotspot is set to the center of the cursor image (7.5, 7.5) for a 15x15 pixel cursor.
    /// </remarks>
    private readonly Vector2 _cursorHotspot = new(7.5f, 7.5f);

    /// <summary>
    /// Initializes the CursorManager by finding the CursorDropdown script in the scene
    /// and setting the initial cursor to the currently selected cursor in the dropdown.
    /// </summary>
    /// <remarks>
    /// This method is called when the script instance is being loaded.
    /// It sets the cursor to the currently selected cursor in the CursorDropdown script.
    /// </remarks>
    private void Start()
    {
        // Find the CursorDropdown script in the scene
        _cursorDropdown = FindFirstObjectByType<CursorType>();

        // Set the initial cursor to whatever is currently selected in the dropdown
        if (_cursorDropdown != null) {
            var selectedCursor = _cursorDropdown.GetSelectedCursor();
            Cursor.SetCursor(selectedCursor, Vector2.zero, CursorMode.Auto);
        } else {
            // Debug.LogWarning("No cursor dropdown reference available!");
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }


    /// <summary>
    /// Sets the cursor to the text cursor when the pointer enters the object.
    /// </summary>
    /// <remarks>
    /// This method is called when the pointer enters the UI element.
    /// It sets the cursor to the i-beam texture specified in the textCursor field.
    /// </remarks>
    /// <param name="eventData">The event data associated with the pointer event.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(textHoverCursor, _cursorHotspot, CursorMode.Auto);
    }


    /// <summary>
    /// Sets the cursor back to the currently selected cursor in the CursorDropdown script
    /// </summary>
    /// <remarks>
    /// This method is called when the pointer exits the UI element.
    /// It resets the cursor to the currently selected cursor in the CursorDropdown script.
    /// </remarks>
    /// <param name="eventData">The event data associated with the pointer event.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        // Get the currently selected cursor from CursorDropdown and set it
        if (_cursorDropdown != null)
        {
            Cursor.SetCursor(_cursorDropdown.GetSelectedCursor(), Vector2.zero, CursorMode.Auto);
        }
        // ResetToDefaultCursor();
    }
}
