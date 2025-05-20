using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    /// <summary>
    /// Button to load the level select scene
    /// </summary>
    [Tooltip("Button to load the level select scene")]
    [Header("Menu Buttons")]
    public GameObject levelSelectButton;

    /// <summary>
    ///
    /// </summary>
    [Tooltip("Lock icon for level select")]
    public GameObject levelSelectLock;

    /// <summary>
    /// Button to load the tutorial scene
    /// </summary>
    [Tooltip("Button to load the play scene")]
    public GameObject tutorialButton;

    /// <summary>
    /// Button to load the instructions scene
    /// </summary>
    [Tooltip("Button to load the instructions scene")]
    public GameObject instructionsButton;

    /// <summary>
    /// The source of the audio
    /// </summary>
    [Header("Audio")]
    public AudioSource audioSource;

    /// <summary>
    /// The sound to play when the button is clicked
    /// </summary>
    public AudioClip clickSound;

    /// <summary>
    /// The text on the level select button
    /// </summary>
    private TMP_Text levelSelectText;

    /// <summary>
    /// The text on the play button
    /// </summary>
    private TMP_Text tutorialText;

    /// <summary>
    /// the text on the instructions button
    /// </summary>
    private TMP_Text instructionsText;


    // TEMPORARY WHILE TESTING
#if UNITY_EDITOR
    private static bool prefsResetThisSession = false;
    private static bool ResetPlayerPrefsInEditor = true;
#endif

    private void Start()
    {
        // TEMPORARY WHILE TESTING
#if UNITY_EDITOR
        if (ResetPlayerPrefsInEditor && !prefsResetThisSession)
        {
            PlayerPrefs.DeleteKey("TutorialFinished");
            prefsResetThisSession = true;
            Debug.Log("PlayerPrefs reset at session start (Editor only)");
        }
#endif

        NavigationData.CameFromOnBoarding = false;
        NavigationData.CameFromLevelComplete = false;

        levelSelectText = levelSelectButton.GetComponentInChildren<TMP_Text>();
        tutorialText = tutorialButton.GetComponentInChildren<TMP_Text>();
        instructionsText = instructionsButton.GetComponentInChildren<TMP_Text>();
        if (!levelSelectLock) levelSelectLock = GameObject.Find("LvlSelectLock");
        bool unlocked = CheckUnlockCondition();
        SetLevelSelectInteractable(unlocked);
    }

    private bool CheckUnlockCondition()
    {
        return PlayerPrefs.GetInt("TutorialFinished", 0) == 1;
    }

    private void SetLevelSelectInteractable(bool interactable)
    {
        if (levelSelectButton) levelSelectButton.GetComponent<EventTrigger>().enabled = interactable;
        if (levelSelectButton) levelSelectButton.SetActive(interactable);
        if (levelSelectLock) levelSelectLock.SetActive(!interactable);
    }

    /// <summary>
    /// Sets the text color when the level select button is clicked
    /// </summary>
    public void OnLevelSelectPress()
    {
        SetPressedColor(levelSelectText);
    }

    /// <summary>
    /// Sets the text color when the level select button is released
    /// </summary>
    public void OnLevelSelectRelease()
    {
        SetDefaultColor(levelSelectText);
        PlayClickSound();
        NavigationData.CameFromOnBoarding = false;
        NavigationData.CameFromLevelComplete = false;
        NavigationData.PreviousScene = "Menu";
        StartCoroutine(LoadSceneDelayed("LevelSelect"));
    }

    /// <summary>
    /// Sets the text color when the Play button is clicked
    /// </summary>
    public void OnTutorialPress()
    {
        SetPressedColor(tutorialText);
    }

    /// <summary>
    /// Sets the text color when the Play button is released
    /// </summary>
    public void OnTutorialRelease()
    {
        SetDefaultColor(tutorialText);
        PlayClickSound();
        NavigationData.CameFromOnBoarding = true;
        NavigationData.CameFromLevelComplete = false;
        StartCoroutine(LoadSceneDelayed("OnBoarding"));
    }

    /// <summary>
    /// Sets the text color when the Instructions button is clicked
    /// </summary>
    public void OnInstructionsPress()
    {
        SetPressedColor(instructionsText);
    }

    /// <summary>
    /// Sets the text color when the Instructions button is released
    /// </summary>
    public void OnInstructionsRelease()
    {
        SetDefaultColor(instructionsText);
        PlayClickSound();
        StartCoroutine(LoadSceneDelayed("Instructions"));
    }

    /// <summary>
    /// Set the color of the text when a button is pressed
    /// </summary>
    /// <param name="text">the text to change</param>
    private void SetPressedColor(TMP_Text text)
    {
        // Light gray
        if (text) text.color = new Color32(200, 200, 200, 255);
    }

    /// <summary>
    /// Set the color of the text back to its default color when a button is released
    /// </summary>
    /// <param name="text">the text to change</param>
    private void SetDefaultColor(TMP_Text text)
    {
        PlayClickSound();

        // White
        if (text) text.color = new Color32(255, 255, 255, 255);
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    /// <remarks>
    /// If in editor, stop play (for debug/testing)
    /// <br />
    /// Otherwise, Quit the game
    /// </remarks>
    public void Quit()
    {
        PlayClickSound();
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    /// <summary>
    /// Play the click sound
    /// </summary>
    private void PlayClickSound()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound);
    }

    /// <summary>
    /// Load the scene after 1 second
    /// </summary>
    /// <param name="sceneName">The name of the scene to load</param>
    /// <returns>IEnumerator</returns>
    private static IEnumerator LoadSceneDelayed(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Open the settings scene
    /// </summary>
    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
}
