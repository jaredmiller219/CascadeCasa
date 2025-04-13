using UnityEngine;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{

    public GameObject completePopup;
    public GameObject restartBtn;
    private Notepad _notepadManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _notepadManager = GameObject.Find("NotepadManager").GetComponent<Notepad>();
        restartBtn.GetComponent<Button>().onClick.AddListener(RestartGame);
    }


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

}
