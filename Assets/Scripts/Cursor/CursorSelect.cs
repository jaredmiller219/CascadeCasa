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
    private const string CursorPrefKey = "SelectedCursorIndex";
    private const int DefaultCursor = 0; // Black cursor is default

    // The cursor styles header
    [Header("Cursor Styles")]

    [Tooltip("The cursor texture for the blank cursor")]
    public Texture2D blankCursor;

    /// <summary>
    /// The black cursor reference
    /// </summary>
    [Tooltip("The cursor texture for the black cursor")]
    public Texture2D blackCursor;

    /// <summary>
    /// The yellow cursor reference
    /// </summary>
    [Tooltip("The cursor texture for the yellow cursor")]
    public Texture2D yellowCursor;

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
        var savedCursorIndex = PlayerPrefs.GetInt(CursorPrefKey, DefaultCursor); // Set the default value

        // Set the cursor based on the selected index
        _dropdown.value = savedCursorIndex;
        _dropdown.onValueChanged.AddListener(SetCursor); // Add listener to the dropdown
        SetCursor(savedCursorIndex);
    }

    /// <summary>
    /// This method is called when the dropdown value changes.
    /// It sets the cursor based on the selected index from the dropdown.
    /// </summary>
    private void SetCursor(int index)
    {

        // Save the selected index to PlayerPrefs
        PlayerPrefs.SetInt(CursorPrefKey, index);
        PlayerPrefs.Save();

        // Set the cursor based on the selected index
        // The index corresponds to the order of the cursors in the dropdown
        _selectedCursor = index switch
        {
            0 => blackCursor,
            1 => blankCursor,
            2 => yellowCursor,
            _ => _selectedCursor
        };

        Cursor.SetCursor(_selectedCursor, Vector2.zero, CursorMode.Auto);
    }

    // public void SwitchCursor(int cursorIndex)
    // {
    //     GlobalCursorManager.Instance.SetCursor(cursorIndex);
    // }

    /// <summary>
    /// This method allows CursorManager to get the currently selected cursor
    /// </summary>
    public int GetSelectedCursor()
    {
        // If _selectedCursor is null, load from PlayerPrefs
        if (_selectedCursor != null || PlayerPrefs.HasKey(CursorPrefKey))
            return PlayerPrefs.GetInt(CursorPrefKey, DefaultCursor);
        var savedIndex = PlayerPrefs.GetInt(CursorPrefKey, DefaultCursor);
        return savedIndex;
    }
}
