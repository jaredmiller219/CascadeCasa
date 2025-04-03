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

    /// <summary>
    /// Reference to the i-beam cursor texture
    /// </summary>
    /// <remarks>
    /// This texture should be set in the Unity Inspector.
    /// </remarks>
    [Tooltip("The texture for the text cursor (i-beam)")]
    public Texture2D textCursor;

    /// <summary>
    /// Reference to the CursorDropdown script
    /// </summary>
    /// <remarks>
    /// This script is used to get the currently selected cursor from the dropdown.
    /// </remarks>
    private CursorDropdown cursorDropdown;

    /// <summary>
    /// Initializes the CursorManager by finding the CursorDropdown script in the scene
    /// and setting the initial cursor to the currently selected cursor in the dropdown.
    /// </summary>
    /// <remarks>
    /// This method is called when the script instance is being loaded.
    /// It sets the cursor to the currently selected cursor in the CursorDropdown script.
    /// </remarks>
    void Start()
    {
        // Find the CursorDropdown script in the scene
        cursorDropdown = FindFirstObjectByType<CursorDropdown>();

        // Set the initial cursor to whatever is currently selected in the dropdown
        if (cursorDropdown != null) {
            Texture2D selectedCursor = cursorDropdown.GetSelectedCursor();
            Cursor.SetCursor(selectedCursor, Vector2.zero, CursorMode.Auto);
        } else {
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
        Cursor.SetCursor(textCursor, Vector2.zero, CursorMode.Auto);
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
        if (cursorDropdown != null)
        {
            Cursor.SetCursor(cursorDropdown.GetSelectedCursor(), Vector2.zero, CursorMode.Auto);
        }
    }
}
