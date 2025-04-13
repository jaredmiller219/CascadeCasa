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
    // Constants for PlayerPrefs keys
    private const string CursorPrefKey = "SelectedCursorIndex"; // Key used to save and retrieve the selected cursor index from PlayerPrefs
    private const int DefaultCursor = 0; // Default cursor index (black cursor)

    // The cursor styles header
    [Header("Cursor Styles")]

    [Tooltip("The cursor texture for the blank cursor")]
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
