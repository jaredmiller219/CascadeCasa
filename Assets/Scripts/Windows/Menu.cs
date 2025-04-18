using UnityEngine;

public class Menu : MonoBehaviour
{

    /// <summary>
    /// This method is called to quit the game.
    /// It closes the application when running in a standalone build.
    /// </summary>
    /// <remarks>
    /// This method uses Application.Quit() to close the application.
    /// In the Unity Editor, it stops the play mode instead of quitting.
    /// This is useful for testing purposes since Application.Quit() does not work in the editor.
    /// </remarks>
    public void Quit()
    {
        // Quit the application. This works when the game is built and running as a standalone application.
        Application.Quit();

        // If the game is running in the Unity Editor, stop playing the game.
        // This is useful for testing purposes since Application.Quit() does not work in the editor.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop the play mode in the Unity Editor.
#endif
    }

    /// <summary>
    /// This method is called to start the level select screen.
    /// It loads the level select screen where players can choose a level to play.
    /// </summary>
    /// <remarks>
    /// This method uses the SceneManager to load the scene named "LevelSelect".
    /// It assumes that the scene is added to the build settings.
    /// </remarks>
    public void LevelSelect()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
    }


    /// <summary>
    /// This method is called to start the settings screen.
    /// It loads the settings screen where players can adjust game settings.
    /// </summary>
    /// <remarks>
    /// This method uses the SceneManager to load the scene named "Settings".
    /// It assumes that the scene is added to the build settings.
    /// </remarks>
    public void Settings()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Settings");
    }
}
