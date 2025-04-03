// CursorSelect.cs

using UnityEngine;
using TMPro;

/// <summary>
/// This script manages the cursor for a dropdown menu in Unity.
/// It allows the user to select different cursor textures from a dropdown list.
/// The selected cursor is applied to the mouse cursor when the dropdown value changes.
/// The script also provides a method to get the currently selected cursor texture.
/// </summary>
public class CursorDropdown : MonoBehaviour
{

    // The cursor styles header
    [Header("Cursor Styles")]

    /// <summary>
    /// The blank cursor reference
    /// </summary>
    [Tooltip("The cursor texture for the blank cursor")]
    public Texture2D BlankCursor;

    /// <summary>
    /// The black cursor reference
    /// </summary>
    [Tooltip("The cursor texture for the black cursor")]
    public Texture2D BlackCursor;

    /// <summary>
    /// The yellow cursor reference
    /// </summary>
    [Tooltip("The cursor texture for the yellow cursor")]
    public Texture2D YellowCursor;

    /// <summary>
    /// Reference to the TMP_Dropdown component
    /// </summary>
    private TMP_Dropdown dropdown;

    /// <summary>
    /// The currently selected cursor texture
    /// </summary>
    private Texture2D selectedCursor;

    /// <summary>
    /// Initializes the dropdown and sets the initial cursor based on the selected index.
    /// </summary>
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>(); // Get the Dropdown component
        dropdown.onValueChanged.AddListener(SetCursor);
        SetCursor(0);
    }

    /// <summary>
    /// This method is called when the dropdown value changes.
    /// It sets the cursor based on the selected index from the dropdown.
    /// </summary>
    void SetCursor(int index)
    {
        switch (index)
        {
            case 0:
                selectedCursor = BlackCursor;
                break;
            case 1:
                selectedCursor = BlankCursor;
                break;
            case 2:
                selectedCursor = YellowCursor;
                break;
        }

        Cursor.SetCursor(selectedCursor, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// This method allows CursorManager to get the currently selected cursor
    /// </summary>
    public Texture2D GetSelectedCursor()
    {
        return selectedCursor;
    }
}
