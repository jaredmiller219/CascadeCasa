using UnityEngine;
// using UnityEngine.UI;
using TMPro;

public class DropdownActionMenu : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    private Notepad notepad;

    void Start()
    {
        // Get the TMP_Dropdown component
        if (dropdown != null)
        {
            dropdown = GetComponentInChildren<TMP_Dropdown>();
            dropdown.onValueChanged.AddListener(OnOptionSelected);
        }

        notepad = GetComponentInChildren<Notepad>();
    }

    void OnOptionSelected(int index)
    {
        // Immediately reset so no option is visually "selected"
        dropdown.RefreshShownValue();

        // Do something based on what was clicked
        switch (index)
        {
            case 0:
                // Option 1 is Save

                // Call save function from notepad
                // notepad.SaveProgress();

                // break
                break;
            case 1:
                // Option 2 is Menu

                // call save function from notepad
                // notepad.SaveProgress();

                // Load menu scene
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");

                // break
                break;
            case 2:
                // Option 3 is Quit

                // call save function from notepad
                // notepad.SaveProgress();

                // Quit the game
                // If in editor, stop playing. If in build, quit application
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();

                // break
                break;
            default:
                // Handle unexpected index

                // break
                break;
        }
    }
}
