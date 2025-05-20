using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bedroom1_Notepad : MonoBehaviour
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
    /// The source of the audio
    /// </summary>
    [Header("Audio")]
    public AudioSource audioSource;

    /// <summary>
    /// The sound to play when the button is clicked
    /// </summary>
    public AudioClip clickSound;

    /// <summary>
    /// whether you can click the reset button
    /// </summary>
    [HideInInspector]
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
    /// The reference to the gameObject with the orizontalScrollBar
    /// </summary>
    [Tooltip("The reference to the gameObject with the HorizontalScrollBar")]
    [SerializeField]
    [Header("Notepad")]
    private Bedroom1_HorizontalScrollBar scrollBar;

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
    private Bedroom1_ChallengeImage selectedImage;

    /// <summary>
    /// The saved values of the updated CSS from the user
    /// </summary>
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
    "font-family defines the typeface. You can use common fonts like Arial or 'Times New Roman'.",
    "font-size sets how big the text is. Use units like px, em, or rem.",
    "font-weight changes how bold text looks. Try normal, bold, or numeric values like 700.",
    "line-height adjusts spacing between lines. Helpful for readability!",
    "text-align aligns your text left, center, or right.",
    "text-decoration adds effects like underline or none (useful for links).",
    "Use color to change text color. Use basic names or hex values like #333.",
    "background-color changes the area behind the text.",
    "You can combine multiple text styles in one rule block.",
    "Final check: Be mindful of colons, semicolons, and spelling in properties like font-family and line-height."
};


    private void Start()
    {
        submitBtn.GetComponent<Button>().onClick.AddListener(CheckCssInput);
        resetBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            ResetCurrentChallenge();
            ChangeFocusTo(null);
        });

        resetPopup.SetActive(false);

        const float scrollSensitivity = 0.01f;
        inputField.GetComponent<TMP_InputField>().scrollSensitivity = scrollSensitivity;

        _cursorManager = GlobalCursorManager.Instance;
        if (_cursorManager) _previousCursorIndex = GlobalCursorManager.GetSelectedCursor();

        scrollBar = FindFirstObjectByType<Bedroom1_HorizontalScrollBar>();
        if (!scrollBar) Debug.LogError("Bedroom1_HorizontalScrollBar not found in scene!");

        saveFilePath = Path.Combine(Application.persistentDataPath, "notepad_progress.json");

        canReset = false;
        canSubmit = false;

        currentChallengeIndex = -1;
        levelsCompleted = 0;

        LoadProgress();
    }

    private static void ChangeFocusTo(GameObject gameObj)
    {
        EventSystem.current.SetSelectedGameObject(gameObj);
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
    /// Save the currently displayed text into the savedTexts dictionary, only if a challenge is loaded.
    /// </summary>
    public void SaveCurrentInputIfNeeded()
    {
        if (currentChallengeIndex >= 0 && inputField.activeSelf)
        {
            savedTexts[currentChallengeIndex] = inputField.GetComponent<TMP_InputField>().text;
            SaveProgress();
        }
    }

    /// <summary>
    /// Set the CSS text
    /// </summary>
    /// <param name="css">The CSS to set</param>
    public void SetCssText(string css)
    {
        if (!inputField) return;

        // When an image is clicked, store a reference to it so we can update its CurrentCss later
        SetTextOfComponent(inputField, css, Color.black, true);
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
    public void OnInputFieldExit()
    {
        _cursorManager.SetCursor(_previousCursorIndex);
    }

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
        else Debug.LogWarning("No TMP_Text or TMP_InputField component found!");
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
    private static string ScrollBarStrValToLower(Bedroom1_HorizontalScrollBar scrollBar, int index)
    {
        return scrollBar.CssChallenges[index].Value.ToLower();
    }

    /// <summary>
    /// Validates user input against the current challenge's correct CSS snippet
    /// </summary>
    private void CheckCssInput()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound);

        if (inputField.GetComponent<TMP_InputField>().text == "" || !canSubmit) return;
        var normalizedUserInput = NormalizeCss(InputFieldStrToLower(inputField));
        var normalizedCorrectCss = NormalizeCss(ScrollBarStrValToLower(scrollBar, currentChallengeIndex));

        if (normalizedUserInput == normalizedCorrectCss)
        {
            const string displayedFeedback = "Correct!";
            SetTextOfComponent(feedbackText, displayedFeedback, Color.green, false);
            if (scrollBar) scrollBar.MarkChallengeCompleted(buttonIndex);
            SaveProgress();
            levelsCompleted++;
            SetTextOfComponent(inputField, "", Color.clear, false);
            ChangeFocusTo(null);
            if (!IsLevelComplete()) return;
            LevelComplete();
        }
        else
        {
            const string displayedFeedback = "Check colons, semicolons, dashes, and syntax!";
            SetTextOfComponent(feedbackText, displayedFeedback, Color.red, false);
        }
    }

    /// <summary>
    /// Checks whether the level is complete
    /// </summary>
    /// <returns>boolean stating whether the level is complete</returns>
    private bool IsLevelComplete()
    {
        return levelsCompleted == scrollBar.imageSprites.Length;
    }

    private void LevelComplete()
    {
        SetTextOfComponent(feedbackText, "All challenges completed!", Color.cyan, false);
        SetTextOfComponent(inputField, "", Color.clear, false);
        SetButtonInteractable(submitBtn, false);
        SetButtonInteractable(resetBtn, false);
        challengeComplete.SetActive(true);
    }

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
    /// Reset the current challenge's text
    /// </summary>
    public void ResetCurrentChallenge()
    {
        // the user hasn't selected an image yet
        if (currentChallengeIndex == -1)
        {
            SetTextOfComponent(inputField, "", Color.black, true);
            return;
        }
        savedTexts.Remove(currentChallengeIndex);
        SetTextOfComponent(inputField, scrollBar.CssChallenges[currentChallengeIndex].Key, Color.black, true);
        UpdateChallengeUI(currentChallengeIndex);
        LoadChallenge();
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
        SaveData data = new SaveData
        {
            currentChallengeIndex = currentChallengeIndex
        };

        foreach (var kvp in savedTexts)
        {
            data.challenges.Add(new ChallengeEntry { index = kvp.Key, entryText = kvp.Value });
        }

        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(saveFilePath, json);
    }

    /// <summary>
    /// Load the saved user's progress
    /// </summary>
    private void LoadProgress()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            currentChallengeIndex = data.currentChallengeIndex;

            savedTexts.Clear();
            foreach (var entry in data.challenges)
            {
                if (!string.IsNullOrWhiteSpace(entry.entryText)) savedTexts[entry.index] = entry.entryText;
            }
        }
        LoadChallenge();
    }
}
