using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DropdownActionMenu : MonoBehaviour
{
    /// <summary>
    /// A reference to the dropdown button
    /// </summary>
    public TMP_Dropdown dropdown;

    /// <summary>
    /// Reference to the Notepad script
    /// </summary>
    private LivingRoom_Notepad notepad;

    private void Start()
    {
        // if (dropdown)
        // {
        //     dropdown = GetComponentInChildren<TMP_Dropdown>();
        //     dropdown.onValueChanged.AddListener(OnOptionSelected);
        // }
        if (!dropdown) dropdown = GetComponentInChildren<TMP_Dropdown>();
        if (dropdown) dropdown.onValueChanged.AddListener(OnOptionSelected);
        else Debug.LogError("Dropdown reference is missing!");

        notepad = GetComponentInChildren<LivingRoom_Notepad>();
        if (!notepad) Debug.LogError("LivingRoom_Notepad reference is missing!");
    }

    /// <summary>
    /// This method is called when an option is selected from the dropdown menu.
    /// It handles the action associated with the selected option.
    /// <para>The index corresponds to the following options:<br/>
    /// 0 - Save <br/>
    /// 1 - Level Select <br/>
    /// 2 - Menu <br/>
    /// 3 - Settings<br />
    /// 4 - Quit</para>
    /// </summary>
    private void OnOptionSelected(int index)
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
                notepad.SaveProgress();
                NavigationData.PreviousScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("LevelSelect");
                break;

            case 2:
                // Option 2 is Menu

                // Load menu scene
                NavigationData.PreviousScene = SceneManager.GetActiveScene().name;
                notepad.SaveProgress();
                SceneManager.LoadScene("Menu");
                break;

            case 3:
                // Option 3 is Settings

                // Load settings scene
                NavigationData.PreviousScene = SceneManager.GetActiveScene().name;
                notepad.SaveProgress();
                SceneManager.LoadScene("Settings");
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
        }
    }
}
