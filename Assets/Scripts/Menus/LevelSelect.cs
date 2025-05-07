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

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    public void Start()
    {
        // Only detect the image and not the bounding box for click
        livingRoomBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;

        // Only detect the image and not the bounding box for click
        patioBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;

        // Only detect the image and not the bounding box for click
        kitchenBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;

        // Only detect the image and not the bounding box for click
        bathroomBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;

        // Only detect the image and not the bounding box for click
        porchBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;

        // Only detect the image and not the bounding box for click
        bedroomxBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;

        // Only detect the image and not the bounding box for click
        bedroomx1Btn.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;

        // Only detect the image and not the bounding box for click
        gardenBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
    }

    private void PlayClickSound()
    {
        if (audioSource && clickSound)
            audioSource.PlayOneShot(clickSound);
    }

    public void LoadRoom(string roomName)
    {
        PlayClickSound();

        string sceneToLoad;

        switch (roomName)
        {
            case "LivingRoom":
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
            case "Bedroomx":
            case "Bedroomx1":
                sceneToLoad = "Bedroom _"; // Same scene for both
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

    // public void LivingRoom()
    // {
    //     PlayClickSound();
    //     StartCoroutine(LoadLivingRoom());
    // }

    // public void Patio()
    // {
    //     PlayClickSound();
    //     StartCoroutine(LoadPatio());
    // }


    // public void Bathroom()
    // {
    //     PlayClickSound();
    //     StartCoroutine(LoadBathroom());
    // }

    // public void Kitchen()
    // {
    //     PlayClickSound();
    //     StartCoroutine(LoadKitchen());
    // }

    // public void Porch()
    // {
    //     PlayClickSound();
    //     StartCoroutine(LoadPorch());
    // }

    // public void Bedroomx()
    // {
    //     PlayClickSound();
    //     StartCoroutine(LoadBedroomX());
    // }

    // public void Bedroomx1()
    // {
    //     PlayClickSound();
    //     StartCoroutine(LoadBedroomX1());
    // }

    // public void Garden()
    // {
    //     PlayClickSound();
    //     StartCoroutine(LoadGarden());
    // }


    // public IEnumerator LoadLivingRoom()
    // {
    //     yield return new WaitForSeconds(1f);
    //     SceneManager.LoadScene("Living Room");
    // }

    // public IEnumerator LoadPatio()
    // {
    //     yield return new WaitForSeconds(1f);
    //     SceneManager.LoadScene("Patio");
    // }

    // public IEnumerator LoadKitchen()
    // {
    //     yield return new WaitForSeconds(1f);
    //     SceneManager.LoadScene("Kitchen");
    // }

    // public IEnumerator LoadBathroom()
    // {
    //     yield return new WaitForSeconds(1f);
    //     SceneManager.LoadScene("Bathroom");
    // }

    // public IEnumerator LoadPorch()
    // {
    //     yield return new WaitForSeconds(1f);
    //     SceneManager.LoadScene("Porch");
    // }

    // public IEnumerator LoadBedroomX()
    // {
    //     yield return new WaitForSeconds(1f);
    //     SceneManager.LoadScene("Bedroom _");
    // }

    // public IEnumerator LoadBedroomX1()
    // {
    //     yield return new WaitForSeconds(1f);
    //     SceneManager.LoadScene("Bedroom _");
    // }

    // public IEnumerator LoadGarden()
    // {
    //     yield return new WaitForSeconds(1f);
    //     SceneManager.LoadScene("Garden");
    // }
}
