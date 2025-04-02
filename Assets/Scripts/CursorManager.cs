using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D textCursor; // Assign your I-beam cursor in the Inspector
    private CursorDropdown cursorDropdown; // Reference to the CursorDropdown script

    /// <summary>
    /// Initializes the CursorManager by finding the CursorDropdown script in the scene
    /// and setting the initial cursor to the currently selected cursor in the dropdown.
    /// </summary>
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(textCursor, Vector2.zero, CursorMode.Auto);
    }


    /// <summary>
    /// Sets the cursor back to the currently selected cursor in the CursorDropdown script
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        // Get the currently selected cursor from CursorDropdown and set it
        if (cursorDropdown != null)
        {
            Cursor.SetCursor(cursorDropdown.GetSelectedCursor(), Vector2.zero, CursorMode.Auto);
        }
    }
}
