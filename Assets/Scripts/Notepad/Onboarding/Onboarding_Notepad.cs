using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Manages the functionality of the onboarding notepad including user input, feedback, and challenge progression.
/// </summary>
public class Onboarding_Notepad : MonoBehaviour
{
    /// <summary>
    /// The input field where users type their CSS solutions
    /// </summary>
    [Tooltip("The notepad input field for CSS code")]
    [Header("Notepad")]
    public GameObject inputField;

    /// <summary>
    /// The feedback text
    /// </summary>
    [Tooltip("The feedback text area for user messages")]
    [Header("Feedback")]
    public GameObject feedbackText;

    /// <summary>
    /// The submit button
    /// </summary>
    [Tooltip("The submit button for checking CSS code")]
    [Header("Buttons")]
    public GameObject submitBtn;

    /// <summary>
    /// The reset button
    /// </summary>
    [Tooltip("The reset button for restarting the challenge")]
    public GameObject resetBtn;

    /// <summary>
    /// The reset challenge popup
    /// </summary>
    [Tooltip("The text that appears when the reset button is clicked")]
    [Header("Reset Text")]
    public GameObject resetPopup;

    /// <summary>
    /// The current challenge index
    /// </summary>
    [Tooltip("The index of the current challenge")]
    [Header("Challenge Index")]
    [HideInInspector]
    public int currentChallengeIndex;

    /// <summary>
    /// The button index
    /// </summary>
    [HideInInspector]
    public int buttonIndex;

    /// <summary>
    /// The popup displayed when all challenges are completed
    /// </summary>
    [Header("Lvl End Popup")]
    public GameObject challengeComplete;

    /// <summary>
    /// The text for the hint section
    /// </summary>
    [Header("Hint Section")]
    public GameObject hintText;

    /// <summary>
    /// The background image displayed during the room transition
    /// </summary>
    [Header("Room Transition")] 
    public Image backgroundImage;

    /// <summary>
    /// The sprite used to display the furnished version of the room in the onboarding process
    /// </summary>
    public Sprite furnishedRoomSprite;

    /// <summary>
    /// The audio clip that plays when a challenge is successfully completed
    /// </summary>
    public AudioClip successJingle;

    /// <summary>
    /// The journal component used for tracking user progress and displaying relevant information
    /// during the onboarding process.
    /// </summary>
    [Header("Journal")] 
    public Onboarding_Journal journal;

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
    /// the success sound
    /// </summary>
    public AudioClip successSound;
    
    /// <summary>
    /// the error sound
    /// </summary>
    public AudioClip errorSound;

    /// <summary>
    /// whether you can click the reset button
    /// </summary>
    [HideInInspector]
    [UsedImplicitly]
    public bool canReset;

    /// <summary>
    /// Whether you can click the submit button
    /// </summary>
    [HideInInspector]
    public bool canSubmit;

    /// <summary>
    /// Keeps track of the number of levels completed.
    /// </summary>
    public int levelsCompleted;

    /// <summary>
    /// The reference to the gameObject with the HorizontalScrollBar
    /// </summary>
    [Tooltip("The reference to the gameObject with the HorizontalScrollBar")]
    [SerializeField]
    [Header("Notepad")]
    private Onboarding_HorizontalScrollBar scrollBar;

    /// <summary>
    /// The path of the file that saves the user's progress
    /// </summary>
    private string saveFilePath;

    /// <summary>
    /// The global manager of the cursor
    /// </summary>
    private GlobalCursorManager _cursorManager;

    /// <summary>
    /// The button (Image) of the challenge that was selected
    /// </summary>
    private Onboarding_ChallengeImage selectedImage;

    /// <summary>
    /// The saved values of the updated CSS from the user
    /// </summary>
    [UsedImplicitly]
    private Dictionary<int, string> savedTexts = new();

    /// <summary>
    /// The index of the previous cursor
    /// </summary>
    private int _previousCursorIndex;

    /// <summary>
    /// The hints related to the CSS challenges
    /// </summary>
    private readonly List<string> _cssHints = new()
    {
        "Use a colon (:) between property and value. Properties like background-color and width define how elements look.",
        "Text styling: font-size and text-align both need colons and semicolons. Always hyphenate compound property names.",
        "Borders and margins are common layout tools. Remember: margin-top and border use hyphens.",
        "Use color and font-weight to style text. Both need colons, and the values go after them.",
        "Lists use 'list-style-type' to control bullets and 'padding' for spacing. Double-check spelling and colons.",
        "Links are styled with 'text-decoration' and 'color'. Use hyphens for compound properties.",
        "Width and height often go together. Each needs a colon and unit like px or %.",
        "Practice combining multiple properties in one rule block. Don't forget a semicolon at the end of each line.",
        "CSS selectors like .class or #id target specific elements. Check your dots and hashes!",
        "This final one is a recap — remember colons, semicolons, and consistent spacing. You’ve got this!"
    };

    private void Start()
    {
        submitBtn.GetComponent<Button>().onClick.AddListener(CheckCssInput);
        resetBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound);
            ResetCurrentChallenge();
            ChangeFocusTo(null);
        });

        resetPopup.SetActive(false);
        inputField.GetComponent<TMP_InputField>().scrollSensitivity = 0.01f;

        _cursorManager = GlobalCursorManager.Instance;
        if (_cursorManager) _previousCursorIndex = GlobalCursorManager.GetSelectedCursor();

        scrollBar = FindFirstObjectByType<Onboarding_HorizontalScrollBar>();
        if (!scrollBar) Debug.LogError("Onboarding_HorizontalScrollBar not found in scene!");

        saveFilePath = Path.Combine(Application.persistentDataPath, "notepad_progress.json");

        canReset = false;
        canSubmit = false;
        currentChallengeIndex = -1;
        levelsCompleted = 0;

        LoadProgress();
    }
    
    /// <summary>
    /// Switch to the i-beam when inside the input field
    /// </summary>
    public void OnInputFieldEnter()
    {
        _previousCursorIndex = GlobalCursorManager.GetSelectedCursor();
        _cursorManager.SetCursor(6);
    }

    /// <summary>
    /// Switch back to the previous cursor when back outside the input field
    /// </summary>
    public void OnInputFieldExit() => _cursorManager.SetCursor(_previousCursorIndex);

    /// <summary>
    /// Load the challenge
    /// </summary>
    public void LoadChallenge()
    {
        currentChallengeIndex = selectedImage ? selectedImage.buttonIndex : buttonIndex;
        LoadInputForChallenge(currentChallengeIndex);
        UpdateChallengeUI(currentChallengeIndex);
    }
    
    /// <summary>
    /// Save the currently displayed text into the savedTexts dictionary, only if a challenge is loaded.
    /// </summary>
    public void SaveCurrentInputIfNeeded()
    {
        if (currentChallengeIndex < 0 || !inputField.activeSelf) return;
        savedTexts[currentChallengeIndex] = inputField.GetComponent<TMP_InputField>().text;
        SaveProgress();
    }
    
    /// <summary>
    /// Save the current text at the button's index
    /// </summary>
    /// <param name="index">The index of the button</param>
    public void SaveTextForIndex(int index)
    {
        savedTexts[index] = inputField.GetComponent<TMP_InputField>().text;
        SaveProgress();
    }
    
    /// <summary>
    /// Reset the current challenge's text
    /// </summary>
    public void ResetCurrentChallenge()
    {
        if (currentChallengeIndex == -1)
        {
            SetTextOfComponent(inputField, "", Color.black, true);
            return;
        }
        
        var challengeKey = scrollBar.CssChallenges[currentChallengeIndex].Key;
        savedTexts.Remove(currentChallengeIndex);
        SetTextOfComponent(inputField, challengeKey, Color.black, true);
        UpdateChallengeUI(currentChallengeIndex);
        LoadChallenge();
    }
    
    /// <summary>
    /// Load the CSS for the current challenge
    /// </summary>
    /// <param name="challengeIndex">The challenge index</param>
    private void LoadInputForChallenge(int challengeIndex)
    {
        if (savedTexts.TryGetValue(challengeIndex, out var savedInput) && !string.IsNullOrWhiteSpace(savedInput))
        {
            SetTextOfComponent(inputField, savedInput, Color.black, true);
        }
        else SetTextOfComponent(inputField, scrollBar.CssChallenges[challengeIndex].Key, Color.black, true);
    }

    /// <summary>
    /// Update the feedback and hint text
    /// </summary>
    /// <param name="challengeIndex">The challenge index</param>
    private void UpdateChallengeUI(int challengeIndex)
    {
        SetTextOfComponent(hintText, _cssHints[challengeIndex], Color.black, false);
        SetTextOfComponent(feedbackText, "Fix the syntax!", Color.yellow, false);
    }
    
    /// <summary>
    /// Set the CSS text
    /// </summary>
    /// <param name="css">The CSS to set</param>
    public void SetCssText(string css)
    {
        if (!inputField) return;
        SetTextOfComponent(inputField, css, Color.black, true);
    }
    
    /// <summary>
    /// Sets the feedback text and color for the user
    /// </summary>
    /// <param name="textObject">The GameObject containing the text</param>
    /// <param name="text">The text to display</param>
    /// <param name="color">The color of the text</param>
    /// <param name="isInteractable">Whether it is interactable</param>
    private static void SetTextOfComponent(GameObject textObject, string text, Color color, bool isInteractable)
    {
        if (textObject.TryGetComponent(out TMP_InputField inputField))
        {
            inputField.text = text;
            inputField.textComponent.color = color;
            inputField.interactable = isInteractable;
        }
        else if (textObject.TryGetComponent(out TMP_Text tmpText))
        {
            tmpText.text = text;
            tmpText.color = color;
        }
    }
    
    /// <summary>
    /// Validates user input against the current challenge's correct CSS snippet
    /// </summary>
    private void CheckCssInput()
    {
        PlaySound(clickSound);
        if (!CanAttemptSubmission()) return;
        
        if (IsUserCssCorrect()) HandleSuccess();
        else HandleFailure();
    }
    
    /// <summary>
    /// Determines whether a submission attempt can be made.
    /// </summary>
    /// <returns>True if the submit button can be clicked and the input text is not empty or whitespace;
    /// otherwise, false.</returns>
    private bool CanAttemptSubmission()
    {
        var inputText = inputField.GetComponent<TMP_InputField>().text;
        return canSubmit && !string.IsNullOrWhiteSpace(inputText);
    }
    
    /// <summary>
    /// Determines whether the CSS code entered by the user matches the required correct CSS code.
    /// </summary>
    /// <returns>True if the user's CSS code is correct; otherwise, false.</returns>
    private bool IsUserCssCorrect()
    {
        var InputFieldString = InputFieldStrToLower(inputField);
        var ScrollBarString = ScrollBarStrValToLower(scrollBar, currentChallengeIndex);
        
        var userCss =  NormalizeCss(InputFieldString);
        var correctCss = NormalizeCss(ScrollBarString);
        
        return userCss == correctCss;
    }
    
    /// <summary>
    /// Make the text easier to check against the correct value
    /// </summary>
    /// <param name="input">The string to normalize</param>
    /// <returns>A string representing the normalized input</returns>
    private static string NormalizeCss(string input)
    {
        return input.Replace("\n", "").Replace("  ", " ").Trim();
    }
    
    /// <summary>
    /// Handles the successful completion of a CSS challenge by providing positive feedback,
    /// updating the challenge status as complete, and performing level completion checks.
    /// </summary>
    private void HandleSuccess()
    {
        PlaySound(successSound);
        
        ShowFeedback("Correct!", Color.green);
        MarkChallengeAsComplete();
        
        SetTextOfComponent(inputField, "", Color.clear, false);
        ChangeFocusTo(null);
        
        if (IsLevelComplete()) LevelComplete();
    }

    /// <summary>
    /// Handles the scenario when the user's CSS input is incorrect.
    /// Provides feedback to the user about potential syntax issues and plays an error sound.
    /// </summary>
    private void HandleFailure()
    {
        PlaySound(errorSound);
        ShowFeedback("Check colons, semicolons, dashes, and syntax!", Color.red);
    }

    /// <summary>
    /// Displays feedback to the user with the specified message and color.
    /// </summary>
    /// <param name="message">The feedback message to display.</param>
    /// <param name="color">The color of the feedback text.</param>
    private void ShowFeedback(string message, Color color)
    {
        SetTextOfComponent(feedbackText, message, color, false);
    }

    /// <summary>
    /// Marks the current challenge as complete, updates the progress internally,
    /// and increments the number of levels completed.
    /// </summary>
    private void MarkChallengeAsComplete()
    {
        if (scrollBar) scrollBar.MarkChallengeCompleted(buttonIndex);
        SaveProgress();
        levelsCompleted++;
    }
    
    /// <summary>
    /// Checks whether the level is complete
    /// </summary>
    /// <returns>boolean stating whether the level is complete</returns>
    private bool IsLevelComplete() => levelsCompleted == scrollBar.imageSprites.Length;

    /// <summary>
    /// The level is complete
    /// </summary>
    private void LevelComplete()
    {
        if (journal) journal.CloseJournal();
        SetSprite(furnishedRoomSprite);
        PlaySound(successJingle);
        StartCoroutine(ShowPopupAfterDelay(1.2f));
    }
    
    /// <summary>
    /// Displays a popup after a specified delay.
    /// </summary>
    /// <param name="delay">The amount of time in seconds to wait before showing the popup.</param>
    /// <returns>An enumerator that handles the delay.</returns>
    private IEnumerator ShowPopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        SetTextOfComponent(feedbackText, "All challenges completed!", Color.cyan, false);
        SetTextOfComponent(inputField, "", Color.clear, false);
        SetButtonInteractable(submitBtn, false);
        SetButtonInteractable(resetBtn, false);
        challengeComplete.SetActive(true);
    }
    
    /// <summary>
    /// Plays the specified audio clip using the configured audio source.
    /// </summary>
    /// <param name="clip">The audio clip to be played.</param>
    private void PlaySound(AudioClip clip)
    {
        if (audioSource && clip) audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Sets the provided sprite as the background image.
    /// </summary>
    /// <param name="newSprite">The new sprite to apply to the background image.</param>
    private void SetSprite(Sprite newSprite)
    {
        if (backgroundImage && newSprite) backgroundImage.sprite = newSprite;
    }

    /// <summary>
    /// Changes the currently focused UI element to the specified GameObject.
    /// </summary>
    /// <param name="gameObj">The GameObject to set focus to. If null, clears the current focus.</param>
    private static void ChangeFocusTo(GameObject gameObj) => EventSystem.current.SetSelectedGameObject(gameObj);
    
    /// <summary>
    /// Set the button's interactability status
    /// </summary>
    /// <param name="button">The button to set</param>
    /// <param name="isInteractable">The interactability status</param>
    private static void SetButtonInteractable(GameObject button, bool isInteractable)
    {
        button.GetComponent<Button>().interactable = isInteractable;
    }

    /// <summary>
    /// For GameObject inputField (gets text, trims, lowers)
    /// </summary>
    /// <param name="inputField">The input field GameObject</param>
    /// <returns>The text (string) trimmed and lowered</returns>
    private static string InputFieldStrToLower(GameObject inputField)
    {
        return inputField.TryGetComponent<TMP_InputField>(out var input) ? input.text.Trim().ToLower() : null;
    }
    
    /// <summary>
    /// For scrollBar + index (gets challenge value, lowers)
    /// </summary>
    /// <param name="scrollBar">The horizontal scrollbar reference</param>
    /// <param name="index">the index of the text to lower</param>
    /// <returns>The text (string) lowered</returns>
    private static string ScrollBarStrValToLower(Onboarding_HorizontalScrollBar scrollBar, int index)
    {
        return scrollBar.CssChallenges[index].Value.ToLower();
    }
    
    /// <summary>
    /// The user data to save
    /// </summary>
    [System.Serializable]
    private class SaveData
    {
        /// <summary>
        /// The current challenge index of the save data
        /// </summary>
        public int currentChallengeIndex;
        
        /// <summary>
        /// A list of challenge entries for the save data
        /// </summary>
        public List<ChallengeEntry> challenges = new();
    }

    /// <summary>
    /// Each element of challenges to be saved
    /// </summary>
    [System.Serializable]
    private class ChallengeEntry
    {
        /// <summary>
        /// The index of the challenge to be saved
        /// </summary>
        public int index;
        
        /// <summary>
        /// The text of the challenge to be saved
        /// </summary>
        public string entryText;
    }
    
    /// <summary>
    /// Save the current user's progress
    /// </summary>
    public void SaveProgress()
    {
        var data = new SaveData { currentChallengeIndex = currentChallengeIndex };
        foreach (var kvp in savedTexts)
        {
            data.challenges.Add(new ChallengeEntry { index = kvp.Key, entryText = kvp.Value });
        }
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
    }

    /// <summary>
    /// Load the saved user's progress
    /// </summary>
    private void LoadProgress()
    {
        if (File.Exists(saveFilePath))
        {
            var json = File.ReadAllText(saveFilePath);
            var data = JsonUtility.FromJson<SaveData>(json);
            currentChallengeIndex = data.currentChallengeIndex;
            savedTexts.Clear();
            foreach (var entry in data.challenges.Where(entry => !string.IsNullOrWhiteSpace(entry.entryText)))
            {
                savedTexts[entry.index] = entry.entryText;
            }
        }
        LoadChallenge();
    }
}
