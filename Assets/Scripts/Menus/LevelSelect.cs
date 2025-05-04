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

    [InspectorName("Pation Button")]
    [Tooltip("The button to go to the patio scene \n lvl 7")]
    public GameObject patioBtn;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    public void Start()
    {
        // Only detect the image and not the bounding box for click
        livingRoomBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;

        // Only detect the image and not the bounding box for click
        patioBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
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

    public void Patio()
    {
        PlayClickSound();
        StartCoroutine(LoadPatio());
    }

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    public IEnumerator LoadLivingRoom()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Living Room");
    }

    public IEnumerator LoadPatio()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Patio");
    }
}
