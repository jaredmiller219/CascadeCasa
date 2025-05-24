using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Onboarding_DropdownActionMenu : MonoBehaviour
{
    /// <summary>
    /// A reference to the dropdown button
    /// </summary>
    public TMP_Dropdown dropdown;

    /// <summary>
    /// Reference to the Notepad script
    /// </summary>
    private Onboarding_Notepad notepad;

    private void Start()
    {
        if (!dropdown) dropdown = GetComponentInChildren<TMP_Dropdown>();
        if (dropdown) dropdown.onValueChanged.AddListener(OnOptionSelected);
        else Debug.LogError("Dropdown reference is missing!");

        notepad = GetComponentInChildren<Onboarding_Notepad>();
        if (!notepad)
            Debug.LogError("Onboarding_Notepad reference is missing!");
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
    /// <param name="index">The index of the option selected</param>
    private void OnOptionSelected(int index)
    {
        // Immediately reset so no option is visually "selected"
        dropdown.RefreshShownValue();

        switch (index)
        {
            case 0:
                // Save
                notepad.SaveProgress();
                break;
            case 1:
                // Menu
                notepad.SaveProgress();
                NavigationData.PreviousScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("Menu");
                break;

            case 2:
                // Settings
                notepad.SaveProgress();
                NavigationData.PreviousScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("Settings");
                break;

            case 3:
                // Quit the game
                notepad.SaveProgress();

                // If in the editor, stop playing. If in build, quit the application
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
                Application.Quit();
                break;
        }
    }
}
