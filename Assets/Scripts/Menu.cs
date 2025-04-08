using UnityEngine;

public class Menu : MonoBehaviour
{
    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    public void Quit()
    {
        // quit the game
        Application.Quit();

        // if we are in the editor, stop playing
        // JUST FOR TESTING
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void LevelSelect()
    {
        // load the level select scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("CSS-Dustin");
    }

}
