using UnityEngine;

/// <summary>
/// Manages the cursor settings across all scenes.
/// This script should be attached to a GameObject in every scene.
/// </summary>
public class GlobalCursorManager : MonoBehaviour
{
    /// <summary>
    /// Key used to save and retrieve the selected cursor index from PlayerPrefs.
    /// </summary>
    private const string CursorPrefKey = "SelectedCursorIndex";

    /// <summary>
    /// Default cursor index to use if no saved preference is found.
    /// </summary>
    private const int DefaultCursor = 0;

    /// <summary>
    /// Hotspot position for the I-beam cursor (used for text editing).
    /// </summary>
    private readonly Vector2 _cursorHotspot = new(7.5f, 7.5f);

    /// <summary>
    /// Texture for the black cursor.
    /// </summary>
    [Header("Cursor Textures")]
    [Tooltip("Black cursor texture")]
    [SerializeField] private Texture2D blackCursor;

    /// <summary>
    /// Texture for the blank cursor.
    /// </summary>
    [Tooltip("Blank cursor texture")]
    [SerializeField] private Texture2D blankCursor;

    /// <summary>
    /// Texture for the yellow cursor.
    /// </summary>
    [Tooltip("Yellow cursor texture")]
    [SerializeField] private Texture2D yellowCursor;

    /// <summary>
    /// Texture for the I-beam cursor.
    /// </summary>
    [Tooltip("I-beam cursor texture")]
    [SerializeField] private Texture2D iBeamCursor;

    /// <summary>
    /// Array to hold all cursor textures.
    /// </summary>
    private Texture2D[] _cursorTextures;

    /// <summary>
    /// Singleton instance of the GlobalCursorManager.
    /// </summary>
    public static GlobalCursorManager Instance { get; set; }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures only one instance of this manager exists and initializes the cursor settings.
    /// </summary>
    private void Awake()
    {
        // Check if an instance of this manager already exists
        if (Instance == null)
        {
            // Set this instance as the singleton instance
            Instance = this;

            // Initialize the array of cursor textures
            InitializeCursorTextures();

            // Load the saved cursor preference and apply it
            LoadSavedCursor();

            // Prevent this GameObject from being destroyed when loading new scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy this GameObject if another instance already exists
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Initializes the array of cursor textures with the serialized textures.
    /// </summary>
    private void InitializeCursorTextures()
    {
        // Create an array to hold the cursor textures
        _cursorTextures = new Texture2D[4];

        // Assign each texture to its corresponding index
        _cursorTextures[0] = blackCursor; // Black cursor
        _cursorTextures[1] = blankCursor; // Blank cursor
        _cursorTextures[2] = yellowCursor; // Yellow cursor
        _cursorTextures[3] = iBeamCursor; // I-beam cursor
    }

    /// <summary>
    /// Loads the saved cursor index from PlayerPrefs and applies the corresponding cursor.
    /// </summary>
    private void LoadSavedCursor()
    {
        // Retrieve the saved cursor index from PlayerPrefs, defaulting to DefaultCursor if not found
        var savedCursorIndex = PlayerPrefs.GetInt(CursorPrefKey, DefaultCursor);

        // Apply the cursor based on the saved index
        ApplyCursor(savedCursorIndex);
    }

    /// <summary>
    /// Sets the cursor to the specified index and saves the preference.
    /// </summary>
    /// <param name="cursorIndex">The index of the cursor to set.</param>
    public void SetCursor(int cursorIndex)
    {
        // Ensure the index is within the valid range
        if (cursorIndex < 0 || cursorIndex >= _cursorTextures.Length) return;

        // Save the selected cursor index to PlayerPrefs
        PlayerPrefs.SetInt(CursorPrefKey, cursorIndex);

        // Persist the changes to PlayerPrefs
        PlayerPrefs.Save();

        // Apply the cursor based on the selected index
        ApplyCursor(cursorIndex);
    }

    /// <summary>
    /// Retrieves the currently selected cursor index from PlayerPrefs.
    /// </summary>
    /// <returns>The index of the currently selected cursor.</returns>
    public int GetSelectedCursor()
    {
        // Return the saved cursor index from PlayerPrefs
        return PlayerPrefs.GetInt(CursorPrefKey);
    }

    /// <summary>
    /// Applies the cursor texture based on the specified index.
    /// </summary>
    /// <param name="index">The index of the cursor to apply.</param>
    private void ApplyCursor(int index)
    {
        // Ensure the index is within the valid range and the texture is not null
        if (index < 0 || index >= _cursorTextures.Length || _cursorTextures[index] == null) return;

        // Use a custom hotspot for the I-beam cursor, otherwise use the default (top-left corner)
        var hotspot = index == 3 ? _cursorHotspot : Vector2.zero;

        // Set the cursor texture, hotspot, and mode
        Cursor.SetCursor(_cursorTextures[index], hotspot, CursorMode.Auto);
    }
}
