using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Kitchen_Notepad : MonoBehaviour
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
    public int buttonindex;

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
    /// whether or not you can click the reset button
    /// </summary>
    [HideInInspector]
    public bool canReset;

    /// <summary>
    /// whether or not you can click the submit button
    /// </summary>
    [HideInInspector]
    public bool canSubmit;

    /// <summary>
    /// The path of the file that saves the user's progress
    /// </summary>
    private readonly string saveFilePath;

    /// <summary>
    /// The glabal manager of the cursor
    /// </summary>
    private GlobalCursorManager _cursorManager;

    /// <summary>
    /// The button (Image) of the challenge that was selected
    /// </summary>
    private Kitchen_ChallengeImage selectedImage;

    /// <summary>
    /// The saved values of the updated css from the user
    /// </summary>
    private Dictionary<int, string> savedTexts = new();

    /// <summary>
    /// The undex of the previous cursor
    /// </summary>
    private int _previousCursorIndex;

    /// <summary>
    /// The list of css challenges
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _cssChallenges = new()
    {
        new("div {\n    background color blue;\n    width: 100px;\n}", "div {\n    background-color: blue;\n    width: 100px;\n}"),
        new("p {\n    font size 20px;\n    text align center;\n}", "p {\n    font-size: 20px;\n    text-align: center;\n}"),
        new(".box {\n    border 2px solid black;\n    margin top 10px;\n}", ".box {\n    border: 2px solid black;\n    margin-top: 10px;\n}"),
        new("#header {\n    color red;\n    font weight bold;\n}", "#header {\n    color: red;\n    font-weight: bold;\n}"),
        new("ul {\n    list style type none;\n    padding 0;\n}", "ul {\n    list-style-type: none;\n    padding: 0;\n}"),
        new("a {\n    text decoration none;\n    color green;\n}", "a {\n    text-decoration: none;\n    color: green;\n}"),
        new("img {\n    width 100px;\n    height 100px;\n}", "img {\n    width: 100px;\n    height: 100px;\n}"),
    };

    /// <summary>
    /// The hints related to the css challenges
    /// </summary>
    private readonly List<string> _cssHints = new()
    {
        "CSS lets you style HTML elements by changing things like size and color. For example, you can use width to set how wide something is, and background-color to set its background color.",
        "Look for missing colons in the font size and text align properties.",
        "Ensure the border and margin top properties have colons.",
        "Use a colon after color and font weight properties.",
        "List style type and padding need colons and values.",
        "Colons are required after text decoration and color.",
        "Don't forget colons after width and height."
    };

    private void Start()
    {
        submitBtn.GetComponent<Button>().onClick.AddListener(CheckCssInput);
        resetBtn.GetComponent<Button>().onClick.AddListener(ResetCurrentChallenge);

        resetPopup.SetActive(false);

        const float scrollSensitivity = 0.01f;
        inputField.GetComponent<TMP_InputField>().scrollSensitivity = scrollSensitivity;

        _cursorManager = GlobalCursorManager.Instance;
        if (_cursorManager != null)
        {
            _previousCursorIndex = _cursorManager.GetSelectedCursor();
        }

        canReset = false;
        canSubmit = false;

        currentChallengeIndex = -1;

        // dont load anything at the start, but load the first challenge when the user clicks on an image
        // LoadChallenge();
    }

    /// <summary>
    /// Save the current text at the button's index
    /// </summary>
    /// <param name="index">The index of the button</param>
    public void SaveTextForIndex(int index)
    {
        string currentInput = inputField.GetComponent<TMP_InputField>().text;
        savedTexts[index] = currentInput;
    }

    /// <summary>
    /// Set the css text
    /// </summary>
    /// <param name="css">The css to set</param>
    public void SetCssText(string css)
    {
        // When an image is clicked, store a reference to it so we can update its CurrentCss later
        SetTextOfComponent(inputField, css, Color.black, true);
    }

    /// <summary>
    /// Switch to the i-beam when inside the input field
    /// </summary>
    public void OnInputFieldEnter()
    {
        _previousCursorIndex = _cursorManager.GetSelectedCursor();
        _cursorManager.SetCursor(3);
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
    private void SetButtonInteractable(GameObject button, bool isInteractable)
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
    private void SetTextOfComponent(GameObject textObject, string text, Color color, bool isInteractable)
    {
        if (textObject == null)
        {
            Debug.LogWarning("Text object is null!");
            return;
        }

        TMP_Text tmpText = textObject.GetComponent<TMP_Text>();

        if (textObject.TryGetComponent<TMP_InputField>(out var inputField))
        {
            // Set text and color for TMP_InputField
            inputField.text = text;
            inputField.textComponent.color = color;
            inputField.interactable = isInteractable;
        }
        else if (tmpText != null)
        {
            // Set text and color for TMP_Text
            tmpText.text = text;
            tmpText.color = color;
        }
        else
        {
            Debug.LogWarning("No TMP_Text or TMP_InputField component found!");
        }
    }

    /// <summary>
    /// Validates user input against the current challenge's correct CSS snippet
    /// </summary>
    private void CheckCssInput()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound);

        if (inputField.GetComponent<TMP_InputField>().text != "" && canSubmit)
        {
            var userInput = inputField.GetComponent<TMP_InputField>().text.Trim().ToLower();
            var correctCss = _cssChallenges[currentChallengeIndex].Value.ToLower();

            var normalizedUserInput = NormalizeCss(userInput);
            var normalizedCorrectCss = NormalizeCss(correctCss);

            if (normalizedUserInput == normalizedCorrectCss)
            {
                SetTextOfComponent(feedbackText, "Correct!", Color.green, false);

                var scrollBar = FindFirstObjectByType<Kitchen_HorizontalScrollBar>();
                if (scrollBar != null)
                {
                    scrollBar.MarkChallengeCompleted(buttonindex);
                }
                SetTextOfComponent(inputField, "", Color.clear, false);

                // Load the next challenge after a delay
                // Invoke(nameof(NextChallenge), 1.5f);
            }
            else
            {
                SetTextOfComponent(feedbackText, "Check colons, semicolons, dashes, and syntax!", Color.red, false);
            }
        }
    }

    /// <summary>
    /// Checks whether the level is complete
    /// </summary>
    /// <returns>boolean stating whether level is complete</returns>
    private bool IsLevelComplete()
    {
        return currentChallengeIndex >= _cssChallenges.Count;
    }

    /// <summary>
    /// Goes to the next challenge
    /// </summary>
    private void NextChallenge()
    {
        currentChallengeIndex++;

        if (IsLevelComplete())
        {
            SetTextOfComponent(feedbackText, "All challenges completed!", Color.cyan, false);
            SetTextOfComponent(inputField, "", Color.clear, false);
            SetButtonInteractable(submitBtn, false);
            SetButtonInteractable(resetBtn, false);
            challengeComplete.SetActive(true);
        }
        else LoadChallenge();
    }

    /// <summary>
    /// Set the
    /// </summary>
    /// <param name="index"></param>
    private void SetChallengeIndexFromButtonIndex(int index)
    {
        currentChallengeIndex = index;
    }

    /// <summary>
    /// Load the challenge
    /// </summary>
    public void LoadChallenge()
    {
        if (selectedImage != null)
        {
            SetChallengeIndexFromButtonIndex(selectedImage.GetComponent<Kitchen_ChallengeImage>()._buttonIndex);
        }
        currentChallengeIndex = buttonindex;
        LoadInputForChallenge(currentChallengeIndex);
        UpdateChallengeUI(currentChallengeIndex);
    }

    /// <summary>
    /// Load the css for the current challenge
    /// </summary>
    /// <param name="challengeIndex">The challenge index</param>
    private void LoadInputForChallenge(int challengeIndex)
    {
        if (savedTexts.TryGetValue(challengeIndex, out string savedInput) && !string.IsNullOrWhiteSpace(savedInput))
        {
            SetTextOfComponent(inputField, savedInput, Color.black, true);
        }
        else SetTextOfComponent(inputField, _cssChallenges[challengeIndex].Key, Color.black, true);
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
    private void ResetCurrentChallenge()
    {
        // user hasn't selected an image yet
        if (currentChallengeIndex == -1)
        {
            SetTextOfComponent(inputField, "", Color.black, true);
            return;
        }

        savedTexts.Remove(currentChallengeIndex);
        SetTextOfComponent(inputField, _cssChallenges[currentChallengeIndex].Key, Color.black, true);
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
    /// Save the current user's progress
    /// </summary>
    public void SaveProgress()
    {
        File.WriteAllText(saveFilePath, currentChallengeIndex.ToString());
        Debug.Log("Progress saved!");
    }

    /// <summary>
    /// Load the saved user's progress
    /// </summary>
    private void LoadProgress()
    {
        if (File.Exists(saveFilePath))
        {
            string savedIndex = File.ReadAllText(saveFilePath);
            if (int.TryParse(savedIndex, out int index) && index < _cssChallenges.Count)
            {
                currentChallengeIndex = index;
            }
        }
        LoadChallenge();
    }
}
