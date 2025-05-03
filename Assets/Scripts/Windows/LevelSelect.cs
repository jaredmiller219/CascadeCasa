using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour // Defining a public class named SelectTemp that inherits from MonoBehaviour.
{

    public GameObject livingroombtn;

    public void Start()
    {
        livingroombtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
    }

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    public void LivingRoom()
    {
        // Using the SceneManager from UnityEngine.SceneManagement to load a scene named "Living Room".
        // This will switch the current scene to the one named "Living Room".
        UnityEngine.SceneManagement.SceneManager.LoadScene("Living Room");
    }
}
