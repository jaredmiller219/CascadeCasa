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
    private TMP_Dropdown _dropdown;

    /// <summary>
    /// The currently selected cursor texture
    /// </summary>
    private Texture2D _selectedCursor;

    /// <summary>
    /// Initializes the dropdown and sets the initial cursor based on the selected index.
    /// </summary>
    private void Start()
    {
        _dropdown = GetComponent<TMP_Dropdown>(); // Get the Dropdown component
        _dropdown.onValueChanged.AddListener(SetCursor);
        SetCursor(0);
    }

    /// <summary>
    /// This method is called when the dropdown value changes.
    /// It sets the cursor based on the selected index from the dropdown.
    /// </summary>
    private void SetCursor(int index)
    {
        _selectedCursor = index switch
        {
            0 => BlackCursor,
            1 => BlankCursor,
            2 => YellowCursor,
            _ => _selectedCursor
        };

        Cursor.SetCursor(_selectedCursor, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// This method allows CursorManager to get the currently selected cursor
    /// </summary>
    public Texture2D GetSelectedCursor()
    {
        return _selectedCursor;
    }
}
