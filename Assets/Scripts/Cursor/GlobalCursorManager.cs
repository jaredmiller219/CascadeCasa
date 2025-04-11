using UnityEngine;

/// <summary>
/// Manages the cursor settings across all scenes.
/// This script should be attached to a GameObject in every scene.
/// </summary>
public class GlobalCursorManager : MonoBehaviour
{
    private const string CURSOR_PREF_KEY = "SelectedCursorIndex";
    private const int DEFAULT_CURSOR = 0;
    private static GlobalCursorManager instance;

    private readonly Vector2 _cursorHotspot = new(7.5f, 7.5f);

    [Header("Cursor Textures")]
    [Tooltip("Black cursor texture")]
    [SerializeField] private Texture2D blackCursor;
    [Tooltip("Blank cursor texture")]
    [SerializeField] private Texture2D blankCursor;
    [Tooltip("Yellow cursor texture")]
    [SerializeField] private Texture2D yellowCursor;
    [Tooltip("I-beam cursor texture")]
    [SerializeField] private Texture2D iBeamCursor;

    // private Texture2D _selectedCursor;
    private int _selectedCursor;


    private Texture2D[] cursorTextures;

    public static GlobalCursorManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeCursorTextures();
            DontDestroyOnLoad(gameObject);
            LoadSavedCursor();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeCursorTextures()
    {
        // cursorTextures = new[] { blackCursor, blankCursor, yellowCursor, iBeamCursor };
        cursorTextures = new Texture2D[4];
        cursorTextures[0] = blackCursor;
        cursorTextures[1] = blankCursor;
        cursorTextures[2] = yellowCursor;
        cursorTextures[3] = iBeamCursor;

        // Add validation
        for (int i = 0; i < cursorTextures.Length; i++)
        {
            if (cursorTextures[i] == null)
            {
                Debug.LogError($"Cursor texture at index {i} is null!");
            }
            else
            {
                Debug.Log($"Loaded cursor texture {i}: {cursorTextures[i].name}");
            }
        }
    }

    private void LoadSavedCursor()
    {
        int savedCursorIndex = PlayerPrefs.GetInt(CURSOR_PREF_KEY, DEFAULT_CURSOR);
        ApplyCursor(savedCursorIndex);
    }

    public void SetCursor(int cursorIndex)
    {
        if (cursorIndex < 0 || cursorIndex >= cursorTextures.Length)
        {
            Debug.LogWarning($"Invalid cursor index: {cursorIndex}");
            return;
        }

        PlayerPrefs.SetInt(CURSOR_PREF_KEY, cursorIndex);
        PlayerPrefs.Save();
        ApplyCursor(cursorIndex);
    }

    public int GetSelectedCursor()
    {
        // If _selectedCursor is null, load from PlayerPrefs
        if (_selectedCursor == 0 && !PlayerPrefs.HasKey(CURSOR_PREF_KEY))
        {
            int savedIndex = PlayerPrefs.GetInt(CURSOR_PREF_KEY, DEFAULT_CURSOR);
            return savedIndex;
        }
        return _selectedCursor;
    }

    private void ApplyCursor(int index)
    {
        if (index >= 0 && index < cursorTextures.Length && cursorTextures[index] != null)
        {
            Debug.Log($"Setting cursor to index {index}: {cursorTextures[index].name}");
            Vector2 hotspot = Vector2.zero;
            Debug.Log($"Applying cursor: {cursorTextures[index].name}");

            // Special handling for text cursor
            if (index == 3) // Assuming index 1 is your text cursor
            {
                hotspot = _cursorHotspot;
                Cursor.SetCursor(cursorTextures[index], hotspot, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(cursorTextures[index], hotspot, CursorMode.Auto);
            }
            // _selectedCursor = index;
        }
        else
        {
            Debug.LogError($"Failed to apply cursor: Invalid index {index} or missing texture");
        }
    }
}
