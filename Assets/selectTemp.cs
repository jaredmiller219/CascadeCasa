using UnityEngine; // Importing the UnityEngine namespace for Unity-specific functionality.

public class SelectTemp : MonoBehaviour // Defining a public class named SelectTemp that inherits from MonoBehaviour.
{
    // This method is called Css (likely short for something, e.g., "Change Scene").
    // It is public, so it can be accessed from other scripts or UI elements like buttons.
    public void Css()
    {
        // Using the SceneManager from UnityEngine.SceneManagement to load a scene named "Living Room".
        // This will switch the current scene to the one named "Living Room".
        UnityEngine.SceneManagement.SceneManager.LoadScene("Living Room");
    }
}
