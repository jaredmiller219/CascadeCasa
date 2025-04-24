using UnityEngine;

public class SelectTemp : MonoBehaviour // Defining a public class named SelectTemp that inherits from MonoBehaviour.
{
    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    public void Css()
    {
        // Using the SceneManager from UnityEngine.SceneManagement to load a scene named "Living Room".
        // This will switch the current scene to the one named "Living Room".
        UnityEngine.SceneManagement.SceneManager.LoadScene("Living Room");
    }
}
