using UnityEngine;

public class Settings : MonoBehaviour
{
    public void BackToMainMenu(){
        // load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
