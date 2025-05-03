using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    /// <summary>
    /// The button to go to the living room scene
    /// </summary>
    [Header("Rooms")]
    [Tooltip("The button to go to the living room scene")]
    [InspectorName("Living Room")]
    public GameObject livingRoomBtn;

    public void Start()
    {
        // dont detect that we are clicking the image
        // unless we are over the image itself
        // rather than the bounding box
        livingRoomBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
    }

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    public void LivingRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Living Room");
    }
}
