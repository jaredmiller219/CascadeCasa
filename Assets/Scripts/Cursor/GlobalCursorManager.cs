using UnityEngine;

/// <summary>
/// Manages the cursor settings across all scenes.
/// This script should be attached to a GameObject in every scene.
/// </summary>
public class GlobalCursorManager : MonoBehaviour
{
    private const string CURSOR_PREF_KEY = "SelectedCursorIndex";
    private const int DEFAULT_CURSOR = 0;
    private static readonly GlobalCursorManager instance;

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

    private Texture2D[] cursorTextures;

    public static GlobalCursorManager Instance => instance;

    // private void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //         InitializeCursorTextures();
    //         DontDestroyOnLoad(gameObject);
    //         LoadSavedCursor();
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    private void Start()
    {
        InitializeCursorTextures();
        int savedCursorIndex = -1;
        // Load the saved cursor index and apply it
        if (!PlayerPrefs.HasKey(CURSOR_PREF_KEY)){
            savedCursorIndex = PlayerPrefs.GetInt(CURSOR_PREF_KEY, DEFAULT_CURSOR);
        }
        ApplyCursor(savedCursorIndex);
    }

    private void InitializeCursorTextures()
    {
        cursorTextures = new Texture2D[4];
        cursorTextures[0] = blackCursor;
        cursorTextures[1] = blankCursor;
        cursorTextures[2] = yellowCursor;
        cursorTextures[3] = iBeamCursor;
    }

    public void SetCursor(int cursorIndex)
    {
        if (cursorIndex < 0 || cursorIndex >= cursorTextures.Length)
        {
            return;
        }

        PlayerPrefs.SetInt(CURSOR_PREF_KEY, cursorIndex);
        PlayerPrefs.Save();
        ApplyCursor(cursorIndex);
    }

    public int GetSelectedCursor(){
        return PlayerPrefs.GetInt(CURSOR_PREF_KEY);
    }

    private void ApplyCursor(int index)
    {
        if (index >= 0 && index < cursorTextures.Length && cursorTextures[index] != null)
        {
            Vector2 hotspot = index == 3 ? _cursorHotspot : Vector2.zero;
            Cursor.SetCursor(cursorTextures[index], hotspot, CursorMode.Auto);
        }
    }
}
