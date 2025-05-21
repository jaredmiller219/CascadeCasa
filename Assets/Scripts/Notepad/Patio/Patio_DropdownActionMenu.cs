using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Patio_DropdownActionMenu : MonoBehaviour
{
    /// <summary>
    /// A reference to the dropdown button
    /// </summary>
    public TMP_Dropdown dropdown;

    /// <summary>
    /// Reference to the Notepad script
    /// </summary>
    private Patio_Notepad notepad;

    private void Start()
    {
        if (!dropdown) dropdown = GetComponentInChildren<TMP_Dropdown>();
        if (dropdown) dropdown.onValueChanged.AddListener(OnOptionSelected);
        else Debug.LogError("Dropdown reference is missing!");

        notepad = GetComponentInChildren<Patio_Notepad>();
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
                // Level Select
                notepad.SaveProgress();
                NavigationData.PreviousScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("LevelSelect");
                break;

            case 2:
                // Menu
                notepad.SaveProgress();
                NavigationData.PreviousScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("Menu");
                break;

            case 3:
                // Settings
                notepad.SaveProgress();
                NavigationData.PreviousScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("Settings");
                break;

            case 4:
                // Ouit
                notepad.SaveProgress();

                // If in editor, stop playing. If in build, quit application
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
                break;
        }
    }
}
