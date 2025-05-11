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

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    /// <summary>
    /// The text on the level select button
    /// </summary>
    private TMP_Text levelSelectText;

    /// <summary>
    /// The text on the play button
    /// </summary>
    private TMP_Text playText;

    /// <summary>
    /// the text on the instructions button
    /// </summary>
    private TMP_Text instructionsText;

    private void Start()
    {
        levelSelectText = levelSelectButton.GetComponentInChildren<TMP_Text>();
        playText = playButton.GetComponentInChildren<TMP_Text>();
        instructionsText = instructionsButton.GetComponentInChildren<TMP_Text>();
    }

    /// <summary>
    ///
    /// </summary>
    public void OnLevelSelectPress()
    {
        SetPressedColor(levelSelectText);
    }

    /// <summary>
    ///
    /// </summary>
    public void OnLevelSelectRelease()
    {
        SetDefaultColor(levelSelectText);
        PlayClickSound();
        StartCoroutine(LoadSceneDelayed("LevelSelect"));
    }

    /// <summary>
    ///
    /// </summary>
    public void OnPlayPress()
    {
        SetPressedColor(playText);
    }

    /// <summary>
    ///
    /// </summary>
    public void OnPlayRelease()
    {
        SetDefaultColor(playText);
        PlayClickSound();
    }

    /// <summary>
    ///
    /// </summary>
    public void OnInstructionsPress()
    {
        SetPressedColor(instructionsText);
    }

    /// <summary>
    ///
    /// </summary>
    public void OnInstructionsRelease()
    {
        SetDefaultColor(instructionsText);
        PlayClickSound();
        StartCoroutine(LoadSceneDelayed("Instructions"));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text">the text to change</param>
    private void SetPressedColor(TMP_Text text)
    {
        // Light gray
        if (text != null) text.color = new Color32(200, 200, 200, 255);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text">the text to change</param>
    private void SetDefaultColor(TMP_Text text)
    {
        PlayClickSound();

        // White
        if (text != null) text.color = new Color32(255, 255, 255, 255);
    }

    /// <summary>
    ///
    /// </summary>
    public void Quit()
    {
        PlayClickSound();
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void PlayClickSound()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound);
    }

    private IEnumerator LoadSceneDelayed(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    ///
    /// </summary>
    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
}
