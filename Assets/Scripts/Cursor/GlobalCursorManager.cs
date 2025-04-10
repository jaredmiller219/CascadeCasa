using UnityEditor.PackageManager;
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

    [SerializeField] private Texture2D[] cursorTextures;

    public static GlobalCursorManager Instance
    {
        get {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
            LoadSavedCursor();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadSavedCursor()
    {
        int savedCursorIndex = PlayerPrefs.GetInt(CURSOR_PREF_KEY, 0);
        ApplyCursor(savedCursorIndex);
    }

    public void SetCursor(int cursorIndex)
    {
        PlayerPrefs.SetInt(CURSOR_PREF_KEY, cursorIndex);
        PlayerPrefs.Save();
        ApplyCursor(cursorIndex);
    }

    private void ApplyCursor(int index)
    {
        if (index >= 0 && index < cursorTextures.Length)
        {
            Cursor.SetCursor(cursorTextures[index], Vector2.zero, CursorMode.Auto);
        }
    }

    [Header("Cursor Textures")]
    [Tooltip("Default black cursor texture")]
    public Texture2D blackCursor;
    [Tooltip("Blank cursor texture")]
    public Texture2D blankCursor;
    [Tooltip("Yellow cursor texture")]
    public Texture2D yellowCursor;

    private void Start()
    {
        UpdateCursorFromSettings();
    }

    private void UpdateCursorFromSettings()
    {
        int cursorIndex = PlayerPrefs.GetInt(CURSOR_PREF_KEY, DEFAULT_CURSOR);
        Texture2D selectedCursor = cursorIndex switch
        {
            0 => blackCursor,
            1 => blankCursor,
            2 => yellowCursor,
            _ => blackCursor // Fallback to default
        };

        // Set the cursor with no offset
        Cursor.SetCursor(selectedCursor, Vector2.zero, CursorMode.Auto);
    }
}
