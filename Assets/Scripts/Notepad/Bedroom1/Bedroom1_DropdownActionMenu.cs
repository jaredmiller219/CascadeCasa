using UnityEngine;
using TMPro;

public class Bedroom1_DropdownActionMenu : MonoBehaviour
{
    /// <summary>
    /// This class handles the behavior of a dropdown menu in the game.
    /// It allows the player to select different options such as saving progress,
    /// going to the level select screen, returning to the main menu, or quitting the game.
    /// </summary>
    public TMP_Dropdown dropdown;

    /// <summary>
    /// Reference to the Notepad script, which manages the game's challenges.
    /// </summary>
    private Bedroom1_Notepad notepad;

    void Start()
    {
        // Get the TMP_Dropdown component
        if (dropdown != null)
        {
            dropdown = GetComponentInChildren<TMP_Dropdown>();
            dropdown.onValueChanged.AddListener(OnOptionSelected);
        }

        notepad = GetComponentInChildren<Bedroom1_Notepad>();
    }

    /// <summary>
    /// This method is called when an option is selected from the dropdown menu.
    /// It handles the action associated with the selected option.
    /// <para>The index corresponds to the following options:<br/>
    /// 0 - Save <br/>
    /// 1 - Level Select <br/>
    /// 2 - Menu <br/>
    /// 3 - Quit</para>
    /// </summary>
    void OnOptionSelected(int index)
    {
        // Immediately reset so no option is visually "selected"
        dropdown.RefreshShownValue();

        switch (index)
        {
            case 0:
                // Option 1 is Save

                // Call save function from notepad
                notepad.SaveProgress();
                break;

            case 1:
                // Option 1 is Level Select

                // Load level select scene
                UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
                break;

            case 2:
                // Option 2 is Menu

                // Load menu scene
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
                break;

            case 3:
                // Option 3 is Settings

                // Load settings scene
                UnityEngine.SceneManagement.SceneManager.LoadScene("Settings");
                break;

            case 4:
                // Option 4 is Quit

                // Quit the game
                // If in editor, stop playing. If in build, quit application
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
                break;

            default:
                // Handle unexpected index
                break;
        }
    }
}
