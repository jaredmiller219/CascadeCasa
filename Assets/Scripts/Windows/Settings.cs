using UnityEngine;

public class Settings : MonoBehaviour
{

    /// <summary>
    /// This method is called when the "Back to Main Menu" button is clicked.
    /// It loads the main menu scene, allowing the player to return to the main menu.
    /// </summary>
    /// <remarks>
    /// This method uses the SceneManager to load the scene with the name "Menu".
    /// It assumes that the scene is properly set up in the Unity project.
    /// </remarks>
    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
