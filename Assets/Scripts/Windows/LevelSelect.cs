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
    [Tooltip("The button to go to the living room scene")]
    [InspectorName("Living Room")]
    public GameObject livingRoomBtn;
    
    public AudioSource audioSource;
    public AudioClip clickSound;

    public void Start()
    {
        // dont detect that we are clicking the image
        // unless we are over the image itself
        // rather than the bounding box
        livingRoomBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
    }
    
    private void PlayClickSound()
    {
        if (audioSource && clickSound)
            audioSource.PlayOneShot(clickSound);
    }
    
    public void LivingRoom()
    {
        PlayClickSound();
        StartCoroutine(LoadLivingRoom());
    }

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    public void LoadLivingRoom()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Living Room");
    }
}
