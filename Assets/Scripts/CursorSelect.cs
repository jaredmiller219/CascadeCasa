using UnityEngine;
using TMPro;

public class CursorDropdown : MonoBehaviour
{
    public Texture2D BlankCursor;     // Assign in Inspector
    public Texture2D BlackCursor;      // Assign in Inspector
    public Texture2D YellowCursor; // Assign in Inspector

    private TMP_Dropdown dropdown;

    private Texture2D selectedCursor;

    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>(); // Get the Dropdown component
        dropdown.onValueChanged.AddListener(SetCursor);
        SetCursor(0);
    }

    void SetCursor(int index)
    {
        switch (index)
        {
            case 0:
                selectedCursor = BlackCursor;
                break;
            case 1:
                selectedCursor = BlankCursor;
                break;
            case 2:
                selectedCursor = YellowCursor;
                break;
        }

        Cursor.SetCursor(selectedCursor, Vector2.zero, CursorMode.Auto);
    }

    // This method allows CursorManager to get the currently selected cursor
    public Texture2D GetSelectedCursor()
    {
        return selectedCursor;
    }
}
