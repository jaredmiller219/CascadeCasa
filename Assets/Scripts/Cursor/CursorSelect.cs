using UnityEngine;
using TMPro;

/// <summary>
/// This script manages the cursor for a dropdown menu in Unity.
/// It allows the user to select different cursor textures from a dropdown list.
/// </summary>
public class CursorType : MonoBehaviour
{
    /// <summary>
    /// The blank cursor reference
    /// </summary>
    [Tooltip("The cursor texture for the blank cursor")]
    [Header("Cursor Styles")]
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
    /// The pink cursor reference
    /// </summary>
    [Tooltip("The cursor texture for the pink cursor")]
    public Texture2D pinkCursor;

    /// <summary>
    /// The gradient cursor reference
    /// </summary>
    [Tooltip("The cursor texture for the gradient cursor")]
    public Texture2D gradientCursor;

    /// <summary>
    /// The key used to save the selected cursor index in PlayerPrefs.
    /// </summary>
    /// <remarks>
    /// This allows the selected cursor to persist between game sessions.
    /// </remarks>
    private const string CursorPrefKey = "SelectedCursorIndex";

    /// <summary>
    /// The default cursor index used when no cursor is selected or saved.
    /// </summary>
    /// <remarks>
    /// This is set to 0, which corresponds to the black cursor.
    /// </remarks>
    private const int DefaultCursor = 0;

    /// <summary>
    /// Reference to the TMP_Dropdown component for selecting cursor styles
    /// </summary>
    private TMP_Dropdown _dropdown;

    /// <summary>
    /// The currently selected cursor texture
    /// </summary>
    private Texture2D _selectedCursor;

    private void Start()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        var savedCursorIndex = PlayerPrefs.GetInt(CursorPrefKey, DefaultCursor);
        _dropdown.value = savedCursorIndex;
        _dropdown.onValueChanged.AddListener(SetCursor);
        SetCursor(savedCursorIndex);
    }

    /// <summary>
    /// This method retrieves the currently selected cursor index from PlayerPrefs.
    /// <br />
    /// If the index is not found, it returns the default cursor index.
    /// </summary>
    /// <returns>The index of the currently selected cursor</returns>
    public int GetSelectedCursor()
    {
        if (_selectedCursor || PlayerPrefs.HasKey(CursorPrefKey))
        {
            return PlayerPrefs.GetInt(CursorPrefKey, DefaultCursor);
        }

        // Retrieve the saved index from PlayerPrefs or default if not found
        return PlayerPrefs.GetInt(CursorPrefKey, DefaultCursor);
    }

    /// <summary>
    /// This method is called when the dropdown value changes.
    /// <br />
    /// It updates the cursor texture and saves the selected index to the PlayerPrefs.
    /// </summary>
    /// <param name="index">The index of the selected dropdown option</param>
    private void SetCursor(int index)
    {
        PlayerPrefs.SetInt(CursorPrefKey, index);
        PlayerPrefs.Save();

        _selectedCursor = index switch
        {
            0 => blackCursor,
            1 => blankCursor,
            2 => yellowCursor,
            3 => pinkCursor,
            4 => gradientCursor,
            _ => _selectedCursor
        };

        // Apply the selected cursor texture to the system cursor
        Cursor.SetCursor(_selectedCursor, Vector2.zero, CursorMode.Auto);
    }
}
