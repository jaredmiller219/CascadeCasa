using UnityEngine;

/// <summary>
/// Manages the cursor settings across all scenes.
/// </summary>
public class GlobalCursorManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the GlobalCursorManager.
    /// </summary>
    public static GlobalCursorManager Instance { get; set; }

    /// <summary>
    /// Texture for the black cursor.
    /// </summary>
    [Header("Cursor Textures")]
    [Tooltip("Black cursor texture")]
    [SerializeField]
    private Texture2D blackCursor;

    /// <summary>
    /// Texture for the blank cursor.
    /// </summary>
    [Tooltip("Blank cursor texture")]
    [SerializeField]
    private Texture2D blankCursor;

    /// <summary>
    /// Texture for the yellow cursor.
    /// </summary>
    [Tooltip("Yellow cursor texture")]
    [SerializeField]
    private Texture2D yellowCursor;

    /// <summary>
    /// Texture for the yellow cursor.
    /// </summary>
    [Tooltip("Pink cursor texture")]
    [SerializeField]
    private Texture2D pinkCursor;

    /// <summary>
    /// Texture for the yellow cursor.
    /// </summary>
    [Tooltip("Gradient cursor texture")]
    [SerializeField]
    private Texture2D gradientCursor;

    /// <summary>
    ///  Texture for the heart cursor
    /// </summary>
    [Tooltip("heart cursor texture")]
    [SerializeField]
    private Texture2D heartCursor;

    /// <summary>
    /// Texture for the I-beam cursor.
    /// </summary>
    [Tooltip("I-beam cursor texture")]
    [SerializeField]
    private Texture2D IBeamCursor;

    /// <summary>
    /// Index for the I-beam cursor in the cursor textures array.
    /// </summary>
    private int IBeamCursorIndex;

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
    /// Array to hold all cursor textures.
    /// </summary>
    private Texture2D[] _cursorTextures;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Ensures only one instance of this manager exists and initializes the cursor settings.
    /// </summary>
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            InitializeCursorTextures();
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


    private void Start()
    {
        LoadSavedCursor();
    }

    /// <summary>
    /// Initializes the array of cursor textures with the serialized textures.
    /// </summary>
    private void InitializeCursorTextures()
    {
        _cursorTextures = new Texture2D[7];

        _cursorTextures[0] = blackCursor;
        _cursorTextures[1] = blankCursor;
        _cursorTextures[2] = yellowCursor;
        _cursorTextures[3] = pinkCursor;
        _cursorTextures[4] = gradientCursor;
        _cursorTextures[5] = heartCursor;
        _cursorTextures[6] = IBeamCursor;

        IBeamCursorIndex = 6;
    }

    /// <summary>
    /// Loads the saved cursor index from PlayerPrefs and applies the corresponding cursor.
    /// </summary>
    private void LoadSavedCursor()
    {
        ApplyCursor(PlayerPrefs.GetInt(CursorPrefKey, DefaultCursor));
    }

    /// <summary>
    /// Sets the cursor to the specified index and saves the preference.
    /// </summary>
    /// <param name="cursorIndex">The index of the cursor to set.</param>
    public void SetCursor(int cursorIndex)
    {
        if (cursorIndex < 0 || cursorIndex >= _cursorTextures.Length) return;
        PlayerPrefs.SetInt(CursorPrefKey, cursorIndex);
        PlayerPrefs.Save();
        ApplyCursor(cursorIndex);
    }

    /// <summary>
    /// Retrieves the currently selected cursor index from PlayerPrefs.
    /// </summary>
    /// <returns>The index of the currently selected cursor.</returns>
    public static int GetSelectedCursor()
    {
        return PlayerPrefs.GetInt(CursorPrefKey);
    }

    /// <summary>
    /// Applies the cursor texture based on the specified index.
    /// </summary>
    /// <param name="index">The index of the cursor to apply.</param>
    private void ApplyCursor(int index)
    {
        if (index < 0 || index >= _cursorTextures.Length || !_cursorTextures[index]) return;
        var hotspot = index == IBeamCursorIndex ? _cursorHotspot : Vector2.zero;
        Cursor.SetCursor(_cursorTextures[index], hotspot, CursorMode.Auto);
    }
}
