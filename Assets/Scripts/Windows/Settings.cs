using UnityEngine; // Import the UnityEngine namespace, which provides access to Unity-specific classes and functions

// This class represents the settings functionality in the game.
// It inherits from MonoBehaviour, which is the base class for all Unity scripts.
public class Settings : MonoBehaviour
{
    // This method is called to navigate back to the main menu.
    // It loads the scene named "Menu" using Unity's SceneManager.
    public void BackToMainMenu()
    {
        // Use the SceneManager to load the scene with the name "Menu".
        // This effectively transitions the game to the main menu scene.
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
