using UnityEngine;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{

    public GameObject completePopup;
    public GameObject restartBtn;
    // public GameObject menuBtn;
    private Notepad.Notepad _notepadManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _notepadManager = GameObject.Find("NotepadManager").GetComponent<Notepad.Notepad>();
        // set active as false
        // completePopup.SetActive(false);
        restartBtn.GetComponent<Button>().onClick.AddListener(RestartGame);
        // menuBtn.GetComponent<Button>().onClick.AddListener(GoToMenu);
    }

    // void Update(){
    //     // Check if all challenges are completed
    //     if (_notepadManager != null && _notepadManager.IsLevelComplete())
    //     {
    //         ShowCompletePopup();
    //     }
    // }


    // Restart the game
    private void RestartGame()
    {
        // Reload the current scene
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        // debug log for now
        Debug.Log("Restarting Game...");
        // Set the complete popup to inactive
        completePopup.SetActive(false);
        // Reset the game state
        // set current challenge index to 0
        _notepadManager.currentChallengeIndex = 0;
    }

    // Go to the main menu
    // private void GoToMenu()
    // {
    //     // Load the main menu scene
    //     // UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    // }

    // Show the complete popup
    // public void ShowCompletePopup()
    // {
    //     // Set the complete popup to active
    //     completePopup.SetActive(true);
    // }

}
