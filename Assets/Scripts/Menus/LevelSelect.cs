using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    /// <summary>
    /// The button to go to the living room
    /// </summary>
    [Header("Rooms")]
    [Tooltip("The button to go to the living room scene \n lvl 1")]
    [InspectorName("LivingRoom Button")]
    public GameObject livingRoomBtn;

    /// <summary>
    /// The button to go to the patio
    /// </summary>
    [InspectorName("Patio Button")]
    [Tooltip("The button to go to the patio scene \n lvl 7")]
    public GameObject patioBtn;

    /// <summary>
    /// The button to go to the kitchen
    /// </summary>
    [InspectorName("Kitchen Button")]
    [Tooltip("The button to go to the kitchen scene \n lvl _")]
    public GameObject kitchenBtn;

    /// <summary>
    /// The button to go to the bathroom
    /// </summary>
    [InspectorName("Bathroom Button")]
    [Tooltip("The button to go to the bathroom \n lvl _")]
    public GameObject bathroomBtn;

    /// <summary>
    /// The button to go to the porch
    /// </summary>
    [InspectorName("Porch Button")]
    [Tooltip("The button to go to the porch \n lvl _")]
    public GameObject porchBtn;

    /// <summary>
    /// The button to go to bedroom 1
    /// </summary>
    [InspectorName("Bedroom 1 Button")]
    [Tooltip("The button to go to bedroom 1 \n lvl _")]
    public GameObject bedroom1Btn;

    /// <summary>
    /// The button to go to the bedroom 2
    /// </summary>
    [InspectorName("Bedroom 2 Button")]
    [Tooltip("The button to go to bedroom 2 \n lvl _")]
    public GameObject bedroom2Btn;

    /// <summary>
    /// The button to go to the garden
    /// </summary>
    [InspectorName("Garden Button")]
    [Tooltip("The button to go to garden _ \n lvl _")]
    public GameObject gardenBtn;

    /// <summary>
    /// The back button reference
    /// </summary>
    [Header("Navigation")]
    [InspectorName("Back/Menu Button")]
    public GameObject navButton;

    /// <summary>
    /// The text of the back/menu button
    /// </summary>
    [Tooltip("The text of the back/menu button")]
    public TMP_Text btnText;

    /// <summary>
    /// The source of the audio
    /// </summary>
    [Header("Audio")]
    public AudioSource audioSource;

    /// <summary>
    /// The sound to play when the button is clicked
    /// </summary>
    public AudioClip clickSound;


    public void Start()
    {
        SetAlphaHitTest(livingRoomBtn);
        SetAlphaHitTest(patioBtn);
        SetAlphaHitTest(kitchenBtn);
        SetAlphaHitTest(bathroomBtn);
        SetAlphaHitTest(porchBtn);
        SetAlphaHitTest(bedroom1Btn);
        SetAlphaHitTest(bedroom2Btn);
        SetAlphaHitTest(gardenBtn);

        btnText.text = NavigationData.CameFromLevelComplete ? "Menu" : "Back";
    }

    /// <summary>
    /// Set the hit test threshold
    /// <br />
    /// Only detect the image and not the bounding box for click.
    /// </summary>
    /// <param name="btn">The button that was clicked</param>
    private static void SetAlphaHitTest(GameObject btn)
    {
        if (btn.TryGetComponent<Image>(out var img))
        {
            img.alphaHitTestMinimumThreshold = 0.5f;
        }
    }

    /// <summary>
    /// Load the scene/room with the given name
    /// </summary>
    /// <param name="roomName">The name of the room/scene to load</param>
    public void LoadRoom(string roomName)
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound);

        string sceneToLoad;
        switch (roomName)
        {
            case "LivingRoom" or "Living Room":
                sceneToLoad = "Living Room";
                break;
            case "Patio":
                sceneToLoad = "Patio";
                break;
            case "Kitchen":
                sceneToLoad = "Kitchen";
                break;
            case "Bathroom":
                sceneToLoad = "Bathroom";
                break;
            case "Porch":
                sceneToLoad = "Porch";
                break;
            case "Bedroom1" or "Bedroom 1":
                sceneToLoad = "Bedroom1";
                break;
            case "Bedroom2" or "Bedroom 2":
                sceneToLoad = "Bedroom2";
                break;
            case "Garden":
                sceneToLoad = "Garden";
                break;
            default:
                Debug.LogWarning("Unknown room: " + roomName);
                return;
        }
        StartCoroutine(LoadSceneWithDelay(sceneToLoad));
    }

    /// <summary>
    /// Load the scene after 1 second
    /// </summary>
    /// <param name="sceneName">The name of the scene to load</param>
    /// <returns>IEnumerator</returns>
    private static IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Go back to the previous screen
    /// </summary>
    public void Back()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound);

        if (NavigationData.CameFromLevelComplete)
        {
            NavigationData.CameFromLevelComplete = false;
            StartCoroutine(LoadSceneWithDelay("Menu"));
        }
        else if (NavigationData.CameFromOnBoarding)
        {
            NavigationData.CameFromOnBoarding = false;
            StartCoroutine(LoadSceneWithDelay("Menu"));
        }
        else StartCoroutine(LoadSceneWithDelay(NavigationData.PreviousScene));
    }
}
