using UnityEngine;

public class Settings : MonoBehaviour
{

    /// <summary>
    /// This method is called when the "Back" button is clicked.
    /// <br/>
    /// It loads the main menu scene, allowing the player to return to the main menu.
    /// </summary>
    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
