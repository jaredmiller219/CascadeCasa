using UnityEngine;

public class Menu : MonoBehaviour // Define a class named Menu that inherits from MonoBehaviour, allowing it to be attached to GameObjects in Unity.
{
    // This method is called to quit the game.
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

    // This method is called to load the level select scene.
    public void LevelSelect()
    {
        // Load the scene named "CSS-Dustin". This is typically the level select screen.
        UnityEngine.SceneManagement.SceneManager.LoadScene("CSS-Dustin");
    }

    // This method is called to load the settings scene.
    public void Settings()
    {
        // Load the scene named "Settings". This is typically where game settings can be adjusted.
        UnityEngine.SceneManagement.SceneManager.LoadScene("Settings");
    }
}
