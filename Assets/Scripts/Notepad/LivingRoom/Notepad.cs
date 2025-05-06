using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notepad : MonoBehaviour
{
    /// <summary>
    /// The input field where users type their CSS solutions
    /// </summary>
    [Tooltip("The notepad input field for CSS code")]
    [Header("Notepad")]
    public GameObject inputField;

    [Tooltip("The feedback text area for user messages")]
    [Header("Feedback")]
    public GameObject feedbackText;

    [Tooltip("The submit button for checking CSS code")]
    [Header("Buttons")]
    public GameObject submitBtn;

    [Tooltip("The reset button for restarting the challenge")]
    public GameObject resetBtn;

    [Tooltip("The text that appears when the reset button is clicked")]
    [Header("Reset Text")]
    public GameObject resetPopup;

    [Tooltip("The index of the current challenge")]
    [Header("Challenge Index")]
    public int currentChallengeIndex;

    [HideInInspector]
    public int buttonindex;

    /// <summary>
    /// The popup displayed when all challenges are completed
    /// </summary>
    [Header("Lvl End Popup")]
    public GameObject challengeComplete;

    [Header("Hint Section")]
    public GameObject hintText;

    [Header("Audio")]
    public AudioSource audioSource;
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

    private readonly string saveFilePath;

    private GlobalCursorManager _cursorManager;

    private LivingRoom_ChallengeImage selectedImage;

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

    private readonly List<string> _cssHints = new()
    {
        "CSS lets you style HTML elements by changing things like size and color." +
        "For example, you can use width to set how wide something is, " +
        "and background-color to set its background color.",

        "Look for missing colons in the font size and text align properties.",

        "Ensure the border and margin top properties have colons."
    };

    private int _previousCursorIndex;

    // Dictionary to store each challenge's user input by its index
    private Dictionary<int, string> challengeInputs = new();


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

        // dont load anything at the start, but load the first challenge when the user clicks on an image
        // LoadChallenge();
    }

    public void SetCssText(string css)
    {
        // inputField.GetComponent<TMP_InputField>().text = css;
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
        // Set the button to be interactable or not
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

    private IEnumerator HandleCorrectInput()
    {
        yield return new WaitForSeconds(1.5f);
        SetTextOfComponent(feedbackText, "Select a new furniture", Color.green, false);
    }

    /// <summary>
    /// Validates user input against the current challenge's correct CSS snippet
    /// </summary>
    private void CheckCssInput()
    {
        if (audioSource && clickSound)
        {
            audioSource.PlayOneShot(clickSound);
        }

        if (inputField.GetComponent<TMP_InputField>().text != "" && canSubmit)
        {
            var userInput = inputField.GetComponent<TMP_InputField>().text.Trim().ToLower();
            var correctCss = _cssChallenges[currentChallengeIndex].Value.ToLower();

            var normalizedUserInput = NormalizeCss(userInput);
            var normalizedCorrectCss = NormalizeCss(correctCss);

            if (normalizedUserInput == normalizedCorrectCss)
            {
                // SubmitCSS(userInput);

                SetTextOfComponent(feedbackText, "Correct!", Color.green, false);

                var scrollBar = FindFirstObjectByType<LivingRoom_HorizontalScrollBar>();
                if (scrollBar != null)
                {
                    scrollBar.MarkChallengeCompleted(buttonindex);
                }
                SetTextOfComponent(inputField, "", Color.clear, false);

                StartCoroutine(HandleCorrectInput());

                // Load the next challenge after a delay
                // Invoke(nameof(NextChallenge), 1.5f);
            }
            else
            {
                // If the input is incorrect, display error feedback
                SetTextOfComponent(feedbackText, "Check colons, semicolons, dashes, and syntax!", Color.red, false);
            }
        }
    }

    private bool IsLevelComplete()
    {
        return currentChallengeIndex >= _cssChallenges.Count;
    }

    private void NextChallenge()
    {
        currentChallengeIndex++;

        if (IsLevelComplete())
        {
            // Display a completion message to the user
            SetTextOfComponent(feedbackText, "All challenges completed!", Color.cyan, false);

            // Clear the input field and make it non-interactable
            SetTextOfComponent(inputField, "", Color.clear, false);

            // Disable the submit and reset buttons
            SetButtonInteractable(submitBtn, false);
            SetButtonInteractable(resetBtn, false);

            challengeComplete.SetActive(true);
        }
        else
        {
            LoadChallenge();
        }
    }

    public void LoadChallenge()
    {
        // if the image exists, then we can set the text in the notepad
        if (selectedImage != null)
        {
            // update the current challenge index to the selected image's button index
            currentChallengeIndex = selectedImage.GetComponent<LivingRoom_ChallengeImage>()._buttonIndex;
            // Check if there is user input for the current challenge
            if (challengeInputs.ContainsKey(currentChallengeIndex))
            {
                // Load the user's previous input for this specific challenge
                SetTextOfComponent(inputField, challengeInputs[currentChallengeIndex], Color.black, true);
            }
            else
            {
                // No user input saved, so load the default incorrect CSS for the current challenge
                SetTextOfComponent(inputField, _cssChallenges[currentChallengeIndex].Key, Color.black, true);
            }
            // Set the input field text to the incorrect CSS snippet for the current challenge
            // SetTextOfComponent(inputField, _cssChallenges[currentChallengeIndex].Key, Color.black, true);

            // Set the hint text for the current challenge
            SetTextOfComponent(hintText, _cssHints[currentChallengeIndex], Color.black, false);

            // Display a message prompting the user to fix the syntax
            SetTextOfComponent(feedbackText, "Fix the syntax!", Color.yellow, false);
        }

        if (inputField.GetComponent<TMP_InputField>().text != "" && canReset)
        {
            // If the input field is not empty, set the current challenge index to the button index
            currentChallengeIndex = buttonindex;

            // Check if there is user input for the current challenge
            if (challengeInputs.ContainsKey(currentChallengeIndex))
            {
                // Load the user's previous input for this specific challenge
                SetTextOfComponent(inputField, challengeInputs[currentChallengeIndex], Color.black, true);
            }
            else
            {
                // No user input saved, so load the default incorrect CSS for the current challenge
                SetTextOfComponent(inputField, _cssChallenges[currentChallengeIndex].Key, Color.black, true);
            }

            // Set the input field text to the incorrect CSS snippet for the current challenge
            // SetTextOfComponent(inputField, _cssChallenges[currentChallengeIndex].Key, Color.black, true);

            // Set the hint text for the current challenge
            SetTextOfComponent(hintText, _cssHints[currentChallengeIndex], Color.black, false);

            // Display a message prompting the user to fix the syntax
            SetTextOfComponent(feedbackText, "Fix the syntax!", Color.yellow, false);
        }

        // if an image wasnt selected before, aka its the start of the game, don't have anything to reset to
        return;
    }

    private void ResetCurrentChallenge()
    {
        // Reset the user's input for the current challenge to the original incorrect syntax
        challengeInputs[currentChallengeIndex] = "";
        LoadChallenge();
    }

    private static string NormalizeCss(string input)
    {
        return input.Replace("\n", "").Replace("  ", " ").Trim();
    }

    public void SaveProgress()
    {
        File.WriteAllText(saveFilePath, currentChallengeIndex.ToString());
        Debug.Log("Progress saved!");
    }

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
