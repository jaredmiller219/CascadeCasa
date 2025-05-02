using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections; // $$$$

public class Menu : MonoBehaviour
{
    /// <summary>
    /// Button to load the level select scene
    /// </summary>
    [Tooltip("Button to load the level select scene")]
    [Header("Menu Buttons")]
    public GameObject levelSelectButton;

    /// <summary>
    /// Button to load the play scene
    /// </summary>
    [Tooltip("Button to load the play scene")]
    public GameObject playButton;

    /// <summary>
    /// Button to load the instructions scene
    /// </summary>
    [Tooltip("Button to load the instructions scene")]
    public GameObject instructionsButton;

    [Header("Audio")] // $$$$
    public AudioSource audioSource; // $$$$
    public AudioClip clickSound; // $$$$

    private TMP_Text levelSelectText;
    private TMP_Text playText;
    private TMP_Text instructionsText;

    private void Start()
    {
        // Get TMP_Text from each button’s children
        levelSelectText = levelSelectButton.GetComponentInChildren<TMP_Text>();
        playText = playButton.GetComponentInChildren<TMP_Text>();
        instructionsText = instructionsButton.GetComponentInChildren<TMP_Text>();
    }

    public void OnLevelSelectPress()
    {
        SetPressedColor(levelSelectText);
    }

    public void OnLevelSelectRelease()
    {
        SetDefaultColor(levelSelectText);
        PlayClickSound(); // $$$$
        StartCoroutine(LoadSceneDelayed("LevelSelect")); // $$$$
    }

    public void OnPlayPress()
    {
        SetPressedColor(playText);
    }

    public void OnPlayRelease()
    {
        SetDefaultColor(playText);
        PlayClickSound(); // $$$$
        // Add your Play() call here if needed
    }

    public void OnInstructionsPress()
    {
        SetPressedColor(instructionsText);
    }

    public void OnInstructionsRelease()
    {
        SetDefaultColor(instructionsText);
        PlayClickSound(); // $$$$
        StartCoroutine(LoadSceneDelayed("Instructions")); // $$$$
    }

    public void Settings()
    {
        PlayClickSound(); // $$$$
        StartCoroutine(LoadSceneDelayed("Settings")); // $$$$
    }

    public void Quit()
    {
        PlayClickSound(); // $$$$
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void SetPressedColor(TMP_Text text)
    {
        if (text != null)
            text.color = new Color32(200, 200, 200, 255); // Light gray
    }

    private void SetDefaultColor(TMP_Text text)
    {
        if (text != null)
            text.color = new Color32(255, 255, 255, 255); // White
    }

    private void PlayClickSound() // $$$$
    { // $$$$
        if (audioSource && clickSound) // $$$$
            audioSource.PlayOneShot(clickSound); // $$$$
    } // $$$$

    private IEnumerator LoadSceneDelayed(string sceneName) // $$$$
    { // $$$$
        yield return new WaitForSeconds(1f); // $$$$ adjust if needed
        SceneManager.LoadScene(sceneName); // $$$$
    } // $$$$
}
