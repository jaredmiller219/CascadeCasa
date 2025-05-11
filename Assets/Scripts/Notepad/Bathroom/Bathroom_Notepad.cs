using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bathroom_Notepad : MonoBehaviour
{
    /// <summary>
    /// Reference to the global cursor manager for handling cursor changes
    /// </summary>
    private GlobalCursorManager _cursorManager;

    private Bathroom_ChallengeImage selectedImage; // Reference to the selected image

    /// <summary>
    /// The input field where users type their CSS solutions
    /// </summary>
    [Tooltip("The notepad input field for CSS code")]
    [Header("Notepad")]
    public GameObject inputField;

    /// <summary>
    /// The text area used to display feedback messages to the user
    /// </summary>
    [Tooltip("The feedback text area for user messages")]
    [Header("Feedback")]
    public GameObject feedbackText;

    /// <summary>
    /// The button that users click to submit their CSS solution for validation
    /// </summary>
    [Tooltip("The submit button for checking CSS code")]
    [Header("Buttons")]
    public GameObject submitBtn;

    /// <summary>
    /// The button that resets the current challenge to its initial state
    /// </summary>
    [Tooltip("The reset button for restarting the challenge")]
    public GameObject resetBtn;

    /// <summary>
    /// The popup text displayed when the reset button is clicked
    /// </summary>
    [Tooltip("The text that appears when the reset button is clicked")]
    [Header("Reset Text")]
    public GameObject resetPopup;

    /// <summary>
    /// The index of the current challenge being solved by the user
    /// </summary>
    [Tooltip("The index of the current challenge")]
    [Header("Challenge Index")]
    public int currentChallengeIndex;

    [HideInInspector] public int buttonindex;

    /// <summary>
    /// The popup displayed when all challenges are completed
    /// </summary>
    [Header("Lvl End Popup")]
    public GameObject challengeComplete;


    /// <summary>
    /// The text area used to display hints for the current challenge
    /// </summary>
    [Header("Hint Section")]
    public GameObject hintText;

    /// <summary>
    /// The path where the progress of the game is saved
    /// </summary>
    private readonly string saveFilePath;

    /// <summary>
    /// List of CSS challenges with incorrect and correct snippets.
    /// <br />
    /// - Key: The incorrect CSS with syntax errors to fix
    /// <br />
    /// - Value: The correct CSS with proper syntax
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
    /// List of hints to assist the user in fixing CSS syntax errors
    /// </summary>
    private readonly List<string> _cssHints = new()
    {
        "CSS lets you style HTML elements by changing things like size and color." +
        "For example, you can use width to set how wide something is, " +
        "and background-color to set its background color.",

        "Look for missing colons in the font size and text align properties.",

        "Ensure the border and margin top properties have colons."
    };

    private int _previousCursorIndex;

    private void Start()
    {
        submitBtn.GetComponent<Button>().onClick.AddListener(CheckCssInput);
        resetBtn.GetComponent<Button>().onClick.AddListener(ResetCurrentChallenge);

        // saveFilePath = Path.Combine(Application.persistentDataPath, "notepad_progress.txt");

        resetPopup.SetActive(false);
        const float scrollSensitivity = 0.01f;
        inputField.GetComponent<TMP_InputField>().scrollSensitivity = scrollSensitivity;

        _cursorManager = GlobalCursorManager.Instance;
        if (_cursorManager != null) _previousCursorIndex = _cursorManager.GetSelectedCursor();

        // dont load anything at the start, but load the first challenge when the user clicks on an image
        // LoadChallenge();
    }

    public void SetCssText(string css)
    {
        SetTextOfComponent(inputField, css, Color.black, true);
    }


    public void OnInputFieldEnter()
    {
        _previousCursorIndex = _cursorManager.GetSelectedCursor();
        _cursorManager.SetCursor(3);
    }

    public void OnInputFieldExit()
    {
        _cursorManager.SetCursor(_previousCursorIndex);
    }

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
        else Debug.LogWarning("No TMP_Text or TMP_InputField component found!");
    }

    /// <summary>
    /// Validates user input against the current challenge's correct CSS snippet
    /// </summary>
    private void CheckCssInput()
    {
        var userInput = inputField.GetComponent<TMP_InputField>().text.Trim().ToLower();
        var correctCss = _cssChallenges[currentChallengeIndex].Value.ToLower();

        var normalizedUserInput = NormalizeCss(userInput);
        var normalizedCorrectCss = NormalizeCss(correctCss);

        if (normalizedUserInput == normalizedCorrectCss)
        {
            SetTextOfComponent(feedbackText, "Correct!", Color.green, false);

            // Load the next challenge after a delay
            // Invoke(nameof(NextChallenge), 1.5f);
        }
        else SetTextOfComponent(feedbackText, "Check colons, semicolons, dashes, and syntax!", Color.red, false);
    }

    /// <summary>
    /// Checks if all challenges have been completed
    /// </summary>
    /// <returns>True if all challenges are completed, otherwise false</returns>
    private bool IsLevelComplete()
    {
        return currentChallengeIndex >= _cssChallenges.Count;
    }

    /// <summary>
    /// Advances to the next challenge or completes the game
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

    private void SetChallengeIndexFromButtonIndex(int index)
    {
        currentChallengeIndex = index;
    }

    /// <summary>
    /// Loads the current challenge with an incorrect CSS snippet
    /// </summary>
    private void LoadChallenge()
    {
        if (selectedImage != null)
        {
            SetTextOfComponent(inputField, _cssChallenges[currentChallengeIndex].Key, Color.black, true);
            SetTextOfComponent(hintText, _cssHints[currentChallengeIndex], Color.black, false);
            SetTextOfComponent(feedbackText, "Fix the syntax!", Color.yellow, false);
            SetChallengeIndexFromButtonIndex(selectedImage.GetComponent<Bathroom_ChallengeImage>()._buttonIndex);
        }

        if (inputField.GetComponent<TMP_InputField>().text != "")
        {
            currentChallengeIndex = buttonindex;
            SetTextOfComponent(inputField, _cssChallenges[currentChallengeIndex].Key, Color.black, true);
        }

        // if an image wasnt selected before, aka its the start of the game, don't have anything to reset to
        return;
    }

    /// <summary>
    /// Resets the current challenge back to its original incorrect CSS snippet
    /// </summary>
    private void ResetCurrentChallenge()
    {
        LoadChallenge();
    }

    /// <summary>
    /// Normalizes CSS input by removing excess spaces and line breaks
    /// </summary>
    /// <param name="input">The CSS string to normalize</param>
    /// <returns>A normalized CSS string</returns>
    private static string NormalizeCss(string input)
    {
        // Replace newlines with empty strings, remove double spaces, and trim the result
        return input.Replace("\n", "").Replace("  ", " ").Trim();
    }

    /// <summary>
    /// Saves the current challenge index to a file
    /// </summary>
    public void SaveProgress()
    {
        File.WriteAllText(saveFilePath, currentChallengeIndex.ToString());
        Debug.Log("Progress saved!");
    }

    /// <summary>
    /// Loads the saved challenge index from file and resumes progress
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
