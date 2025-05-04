using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectPatio : MonoBehaviour
{
    /// <summary>
    /// The button to go to the living room scene
    /// </summary>
    [Header("Rooms")]
    [Tooltip("The button to go to the patio scene")]
    [InspectorName("Patio \n Lvl 7")]
    
    public GameObject patioBtn;

    public AudioSource audioSource;
    public AudioClip clickSound;

    public void Start()
    {
        // dont detect that we are clicking the image
        // unless we are over the image itself
        // rather than the bounding box
        
        patioBtn.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        

    }

    private void PlayClickSound()
    {
        if (audioSource && clickSound)
            audioSource.PlayOneShot(clickSound);
    }


    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>


    public void Patio()
    {
        PlayClickSound();
        StartCoroutine(LoadPatio());
    }
    public IEnumerator LoadPatio()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Patio");
    }
}
