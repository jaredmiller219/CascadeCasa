using UnityEngine;
using UnityEngine.UI;

// This class handles the behavior for the end of a level in the game.
public class LevelEnd : MonoBehaviour
{
    // Reference to the popup GameObject that appears when the level is completed.
    public GameObject completePopup;

    // Reference to the restart button GameObject.
    public GameObject restartBtn;

    // Reference to the Notepad manager script, which manages the game's challenges.
    private Notepad _notepadManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    private void Start()
    {
        // Find the GameObject named "NotepadManager" in the scene and get its Notepad component.
        _notepadManager = GameObject.Find("NotepadManager").GetComponent<Notepad>();

        // Add a listener to the restart button's onClick event to call the RestartGame method when clicked.
        restartBtn.GetComponent<Button>().onClick.AddListener(RestartGame);
    }

    // Method to restart the game.
    private void RestartGame()
    {
        // Debug log to indicate that the game is restarting (for now, instead of reloading the scene).
        Debug.Log("Restarting Game...");

        // Set the complete popup GameObject to inactive, hiding it from the screen.
        completePopup.SetActive(false);

        // Reset the game state by setting the current challenge index in the Notepad manager to 0.
        _notepadManager.currentChallengeIndex = 0;

        // Note: The actual scene reload logic is commented out for now.
        // Uncomment the following line to reload the current scene:
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
