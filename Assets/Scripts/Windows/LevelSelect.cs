using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections; // Needed for IEnumerator

public class SelectTemp : MonoBehaviour
{
    public AudioSource audioSource; // $$$$
    public AudioClip clickSound; // $$$$

    public void Css()
    {
        PlayClickSound(); // $$$$
        StartCoroutine(LoadLivingRoom()); // $$$$
    }

    private void PlayClickSound() // $$$$
    {
        if (audioSource && clickSound)
            audioSource.PlayOneShot(clickSound);
    }

    private IEnumerator LoadLivingRoom() // $$$$
    {
        yield return new WaitForSeconds(1f); // $$$$
        SceneManager.LoadScene("Living Room"); // $$$$
    }
}
