using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    /// <summary>
    /// The button to go to the living room scene
    /// </summary>
    [Header("Rooms")]
    [Tooltip("The button to go to the living room scene \n lvl 1")]
    [InspectorName("Living Button")]
    public GameObject livingRoomBtn;

    [InspectorName("Patio Button")]
    [Tooltip("The button to go to the patio scene \n lvl 7")]
    public GameObject patioBtn;

    [InspectorName("Kitchen Button")]
    [Tooltip("The button to go to the kitchen scene \n lvl _")]
    public GameObject kitchenBtn;

    [InspectorName("Bathroom Button")]
    [Tooltip("The button to go to the bathroom \n lvl _")]
    public GameObject bathroomBtn;

    [InspectorName("Porch Button")]
    [Tooltip("The button to go to the porch \n lvl _")]
    public GameObject porchBtn;

    [InspectorName("Bedroom _ Button")]
    [Tooltip("The button to go to bedroom _ \n lvl _")]
    public GameObject bedroomxBtn;

    [InspectorName("Bedroom _ Button")]
    [Tooltip("The button to go to bedroom _ \n lvl _")]
    public GameObject bedroomx1Btn;

    [InspectorName("Garden Button")]
    [Tooltip("The button to go to garden _ \n lvl _")]
    public GameObject gardenBtn;

    public GameObject outlineOverlay;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    public void Start()
    {
        SetAlphaHitTest(livingRoomBtn);
        SetAlphaHitTest(patioBtn);
        SetAlphaHitTest(kitchenBtn);
        SetAlphaHitTest(bathroomBtn);
        SetAlphaHitTest(porchBtn);
        SetAlphaHitTest(bedroomxBtn);
        SetAlphaHitTest(bedroomx1Btn);
        SetAlphaHitTest(gardenBtn);
    }

    public void SetAlphaHitTest(GameObject btn)
    {
        if (btn.TryGetComponent<Image>(out var img))
        {
            // Only detect the image and not the bounding box for click
            img.alphaHitTestMinimumThreshold = 0.5f;
        }
    }

    public void LoadRoom(string roomName)
    {
        if (audioSource && clickSound)
        {
            audioSource.PlayOneShot(clickSound);
        }

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
                sceneToLoad = "Bedroom 1";
                break;
            case "Bedroom2" or "Bedroom 2":
                sceneToLoad = "Bedroom 2";
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

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }

    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }
}
