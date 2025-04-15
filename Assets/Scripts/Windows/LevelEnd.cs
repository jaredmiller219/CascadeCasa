using UnityEngine;
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
    /// Reference to the Notepad manager script, which manages the game's challenges.
    /// </summary>
    private Notepad _notepadManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    private void Start()
    {
        // Find the GameObject named "NotepadManager" in the scene.
        // This assumes there is a GameObject in the scene with the exact name "NotepadManager".
        // Get the Notepad component attached to that GameObject and assign it to _notepadManager.
        _notepadManager = GameObject.Find("NotepadManager").GetComponent<Notepad>();

        // Access the Button component of the restartBtn GameObject.
        // Add a listener to the button's onClick event.
        // When the button is clicked, the RestartGame method will be executed.
        restartBtn.GetComponent<Button>().onClick.AddListener(RestartGame);
    }

    // Method to restart the game.
    private void RestartGame()
    {
        // Log a message to the console indicating that the game is restarting.
        // This is useful for debugging purposes.
        Debug.Log("Restarting Game...");

        // Deactivate the completePopup GameObject.
        // This hides the popup from the screen by setting its active state to false.
        completePopup.SetActive(false);

        // Reset the current challenge index in the Notepad manager to 0.
        // This effectively resets the game's challenge progress to the beginning.
        _notepadManager.currentChallengeIndex = 0;

        // The following line would reload the current scene, effectively restarting the game.
        // Uncomment this line if you want the scene to reload when restarting:
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
