using UnityEngine;
// using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class handles the behavior for the end of a level in the game.
/// </summary>
public class LevelEnd : MonoBehaviour
{
    /// <summary>
    /// Reference to the popup GameObject that appears when the level is completed.
    /// </summary>
    public GameObject completePopup;

    /// <summary>
    /// Reference to the restart button GameObject.
    /// </summary>
    public GameObject restartBtn;

    /// <summary>
    /// Reference to the menu button GameObject.
    /// </summary>
    public GameObject menuBtn;

    // /// <summary>
    // /// Reference to the Notepad manager script, which manages the game's challenges.
    // /// </summary>
    // private Notepad _notepadManager;

    private void Start()
    {
        // if (GameObject.Find("NotepadManager") != null)
        // {
        //     _notepadManager = GameObject.Find("NotepadManager").GetComponent<Notepad>();
        // }

        // Add event listeners to buttons
        restartBtn.GetComponent<Button>().onClick.AddListener(RestartGame);
        menuBtn.GetComponent<Button>().onClick.AddListener(GoToMenu);
    }

    /// <summary>
    /// This method is called to display the level completion popup.
    /// </summary>
    public void RestartGame()
    {
        Debug.Log("Restarting Game...");
        completePopup.SetActive(false);

        // _notepadManager.currentChallengeIndex = 0;

        // The following line would reload the current scene, effectively restarting the game.
        // string activeSceneName = SceneManager.GetActiveScene().name;
        // SceneManager.LoadScene(activeSceneName);
    }

    /// <summary>
    /// This method is called to navigate to the main menu.
    /// </summary>
    public void GoToMenu()
    {
        // Debug.Log("Going to Menu...");
        completePopup.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
