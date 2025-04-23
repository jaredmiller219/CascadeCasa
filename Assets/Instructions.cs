using UnityEngine;

public class Instructions : MonoBehaviour
{
    public void BackToMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
