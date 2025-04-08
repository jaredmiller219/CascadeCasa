using UnityEngine;

public class Menu : MonoBehaviour
{

    public void Quit()
    {
        // quit the game
        Application.Quit();

        // if we are in the editor, stop playing
        // TODO: JUST FOR TESTING
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void LevelSelect()
    {
        // load the level select scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("CSS-Dustin");
    }

    public void Settings()
    {
        // load the settings scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Settings");
    }

}
