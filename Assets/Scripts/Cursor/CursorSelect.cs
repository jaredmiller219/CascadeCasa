// CursorSelect.cs

using UnityEngine;
using TMPro;

/// <summary>
/// This script manages the cursor for a dropdown menu in Unity.
/// It allows the user to select different cursor textures from a dropdown list.
/// The selected cursor is applied to the mouse cursor when the dropdown value changes.
/// The script also provides a method to get the currently selected cursor texture.
/// </summary>
public class CursorType : MonoBehaviour
{
    /// <summary>
    /// The blank cursor reference
    /// </summary>
    [Tooltip("The cursor texture for the blank cursor")]
    [Header("Cursor Styles")]
    public Texture2D blankCursor; // Texture for the blank cursor

    /// <summary>
    /// The black cursor reference
    /// </summary>
    [Tooltip("The cursor texture for the black cursor")]
    public Texture2D blackCursor; // Texture for the black cursor

    /// <summary>
    /// The yellow cursor reference
    /// </summary>
    [Tooltip("The cursor texture for the yellow cursor")]
    public Texture2D yellowCursor; // Texture for the yellow cursor

    /// <summary>
    /// The key used to save the selected cursor index in PlayerPrefs.
    /// This allows the selected cursor to persist between game sessions.
    /// The index is saved as an integer, and the default value is set to 0 (black cursor).
    /// </summary>
    private const string CursorPrefKey = "SelectedCursorIndex";

    /// <summary>
    /// The default cursor index used when no cursor is selected or saved.
    /// This is set to 0, which corresponds to the black cursor.
    /// </summary>
    private const int DefaultCursor = 0;

    /// <summary>
    /// Reference to the TMP_Dropdown component
    /// </summary>
    private TMP_Dropdown _dropdown; // Dropdown UI component for selecting cursor styles

    /// <summary>
    /// The currently selected cursor texture
    /// </summary>
    private Texture2D _selectedCursor; // Holds the currently selected cursor texture

    /// <summary>
    /// Initializes the dropdown and sets the initial cursor based on the selected index.
    /// </summary>
    private void Start()
    {
        // Get the TMP_Dropdown component attached to the same GameObject
        _dropdown = GetComponent<TMP_Dropdown>();

        // Retrieve the saved cursor index from PlayerPrefs, or use the default value if not found
        var savedCursorIndex = PlayerPrefs.GetInt(CursorPrefKey, DefaultCursor);

        // Set the dropdown's value to the saved cursor index
        _dropdown.value = savedCursorIndex;

        // Add a listener to the dropdown to call SetCursor whenever the value changes
        _dropdown.onValueChanged.AddListener(SetCursor);

        // Set the cursor to the saved index on startup
        SetCursor(savedCursorIndex);
    }

    /// <summary>
    /// This method is called when the dropdown value changes.
    /// It sets the cursor based on the selected index from the dropdown.
    /// </summary>
    /// <remarks>
    /// This method is called automatically by the TMP_Dropdown component when the user selects a new option.
    /// It updates the cursor texture and saves the selected index to PlayerPrefs for persistence.
    /// </remarks>
    /// <param name="index">The index of the selected dropdown option</param>
    private void SetCursor(int index)
    {
        // Save the selected index to PlayerPrefs for persistence
        PlayerPrefs.SetInt(CursorPrefKey, index);
        PlayerPrefs.Save(); // Ensure the changes are saved immediately

        // Determine which cursor texture to use based on the selected index
        _selectedCursor = index switch
        {
            0 => blackCursor, // Index 0 corresponds to the black cursor
            1 => blankCursor, // Index 1 corresponds to the blank cursor
            2 => yellowCursor, // Index 2 corresponds to the yellow cursor
            _ => _selectedCursor // Default to the current cursor if the index is invalid
        };

        // Apply the selected cursor texture to the system cursor
        Cursor.SetCursor(_selectedCursor, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// This method allows CursorManager to get the currently selected cursor.
    /// </summary>
    /// <remarks>
    /// This method retrieves the currently selected cursor index from PlayerPrefs.
    /// If the index is not found, it returns the default cursor index.
    /// </remarks>
    /// <returns>The index of the currently selected cursor</returns>
    public int GetSelectedCursor()
    {
        // If _selectedCursor is not null or PlayerPrefs contains the cursor key, return the saved index
        if (_selectedCursor != null || PlayerPrefs.HasKey(CursorPrefKey))
            return PlayerPrefs.GetInt(CursorPrefKey, DefaultCursor);

        // Otherwise, retrieve the saved index from PlayerPrefs (fallback to default if not found)
        var savedIndex = PlayerPrefs.GetInt(CursorPrefKey, DefaultCursor);
        return savedIndex;
    }
}
