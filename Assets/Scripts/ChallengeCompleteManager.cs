using UnityEngine;
using UnityEngine.UI;

public class ChallengeCompleteManager : MonoBehaviour
{

    public GameObject completePopup;
    public GameObject restartBtn;
    public GameObject menuBtn;
    private readonly NotepadManager notepadManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // set active as false
        completePopup.SetActive(false);
        restartBtn.GetComponent<Button>().onClick.AddListener(RestartGame);
        menuBtn.GetComponent<Button>().onClick.AddListener(GoToMenu);
    }

    // Update is called once per frame
    // void Update()
    // {

    // }

    // Restart the game
    public void RestartGame()
    {
        // Reload the current scene
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        // debug log for now
        Debug.Log("Restarting Game...");
        // Set the complete popup to inactive
        completePopup.SetActive(false);
        // Reset the game state
        // set current challenge index to 0
        notepadManager.currentChallengeIndex = 0;
    }

    // Go to the main menu
    public void GoToMenu()
    {
        // Load the main menu scene
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    // Show the complete popup
    public void ShowCompletePopup()
    {
        // Set the complete popup to active
        completePopup.SetActive(true);
    }

}
