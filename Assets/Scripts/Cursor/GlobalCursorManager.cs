using UnityEngine;

/// <summary>
/// Manages the cursor settings across all scenes.
/// This script should be attached to a GameObject in every scene.
/// </summary>
public class GlobalCursorManager : MonoBehaviour
{
    private const string CursorPrefKey = "SelectedCursorIndex";
    private const int DefaultCursor = 0;

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

    private Texture2D[] _cursorTextures;

    private static GlobalCursorManager Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeCursorTextures();
            LoadSavedCursor(); // Add this line
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeCursorTextures()
    {
        _cursorTextures = new Texture2D[4];
        _cursorTextures[0] = blackCursor;
        _cursorTextures[1] = blankCursor;
        _cursorTextures[2] = yellowCursor;
        _cursorTextures[3] = iBeamCursor;
    }

    private void LoadSavedCursor()
    {
        // Get the saved cursor index, defaulting to DEFAULT_CURSOR if not found
        var savedCursorIndex = PlayerPrefs.GetInt(CursorPrefKey, DefaultCursor);
        ApplyCursor(savedCursorIndex);
    }

    public void SetCursor(int cursorIndex)
    {
        if (cursorIndex < 0 || cursorIndex >= _cursorTextures.Length)
        {
            return;
        }

        PlayerPrefs.SetInt(CursorPrefKey, cursorIndex);
        PlayerPrefs.Save();
        ApplyCursor(cursorIndex);
    }

    public int GetSelectedCursor(){
        return PlayerPrefs.GetInt(CursorPrefKey);
    }

    private void ApplyCursor(int index)
    {
        if (index < 0 || index >= _cursorTextures.Length || _cursorTextures[index] == null) return;
        var hotspot = index == 3 ? _cursorHotspot : Vector2.zero;
        Cursor.SetCursor(_cursorTextures[index], hotspot, CursorMode.Auto);
    }
}
