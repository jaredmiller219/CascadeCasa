using UnityEngine;

public class Settings : MonoBehaviour
{

    /// <summary>
    /// Load the scene after 1 second
    /// </summary>
    /// <param name="sceneName">The name of the scene to load</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// This method is called when the "Back" button is clicked.
    /// <br/>
    /// It loads the main menu scene, allowing the player to return to the main menu.
    /// </summary>
    public void BackToMainMenu()
    {
        StartCoroutine(LoadSceneWithDelay(NavigationData.PreviousScene));
    }
}
