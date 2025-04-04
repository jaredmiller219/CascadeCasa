// NotepadManager.cs

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Manages a CSS learning game where players fix full CSS snippets.
/// </summary>
public class NotepadManager : MonoBehaviour
{
    /// <summary>
    /// Text area where users input their CSS solutions
    /// </summary>
    [Tooltip("The notepad input field for CSS code")]
    [Header("Notepad")]

    // the notepad input field
    public GameObject inputField;

    /// <summary>
    /// Displays feedback messages to the user
    /// </summary>
    [Tooltip("The feedback text area for user messages")]
    [Header("Feedback")]
    public GameObject feedbackText;

    // The header for the buttons
    [Header("Buttons")]

    // Button that triggers solution validation
    [Tooltip("The submit button for checking CSS code")]
    public GameObject submitBtn;

    /// <summary>
    /// Button to reset the current challenge
    /// </summary>
    [Tooltip("The reset button for restarting the challenge")]
    public GameObject resetBtn;

    // The header for the reset text
    [Header("Reset Text")]

    // Text to display when the reset button is clicked
    [Tooltip("The text that appears when the reset button is clicked")]
    public GameObject resetPopup;

    /// <summary>
    /// Path where progress is saved
    /// </summary>
    private string _saveFilePath;

    /// <summary>
    /// Tracks current challenge number
    /// </summary>
    private int _currentChallengeIndex;

    /// <summary>
    /// List of CSS challenges with incorrect and correct snippets.
    ///
    /// <para>
    /// - Key: The incorrect CSS with syntax errors to fix
    /// </para>
    /// <para>
    /// - Value: The correct CSS with proper syntax
    /// </para>
    ///
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _cssChallenges = new()
    {
        new KeyValuePair<string, string>(
            "div {\n    background color blue;\n    width: 100px;\n}", // Incorrect
            "div {\n    background-color: blue;\n    width: 100px;\n}" // Correct
        ),
        new KeyValuePair<string, string>(
            "p {\n    font size 20px;\n    text align center;\n}", // Incorrect
            "p {\n    font-size: 20px;\n    text-align: center;\n}" // Correct
        ),
        new KeyValuePair<string, string>(
            ".box {\n    border 2px solid black;\n    margin top 10px;\n}", // Incorrect
            ".box {\n    border: 2px solid black;\n    margin-top: 10px;\n}" // Correct
        )
    };

    /// <summary>
    /// Initializes the game state and sets up event listeners
    /// </summary>
    private void Start()
    {
        _saveFilePath = Path.Combine(Application.persistentDataPath, "notepad_progress.txt");
        submitBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CheckCssInput);
        resetBtn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ResetCurrentChallenge);
        resetPopup.SetActive(false);

        // Scroll sensitivity value for smooth scrolling
        const float scrollSensitivity = 0.01f;
        // set the scroll sensitivity of the notepadInput
        inputField.GetComponent<TMP_InputField>().scrollSensitivity = scrollSensitivity;

        // Note: Progress loading is disabled for testing
        // LoadProgress();

        LoadChallenge();
    }

    /// <summary>
    /// Validates user input against the current challenge's correct CSS snippet
    /// </summary>
    private void CheckCssInput()
    {
        var userInput = inputField.GetComponent<TMP_InputField>().text.Trim().ToLower();
        var correctCss = _cssChallenges[_currentChallengeIndex].Value.ToLower();

        // Normalize input (remove extra spaces, new lines)
        var normalizedUserInput = NormalizeCss(userInput);
        var normalizedCorrectCss = NormalizeCss(correctCss);

        if (normalizedUserInput == normalizedCorrectCss)
        {
            feedbackText.GetComponent<TMP_Text>().text = "Correct! Loading next challenge...";
            feedbackText.GetComponent<TMP_Text>().color = Color.green;
            Invoke(nameof(NextChallenge), 1.5f);
        }
        else
        {
            feedbackText.GetComponent<TMP_Text>().text = "Incorrect!\nCheck colons, semicolons, and syntax!";
            feedbackText.GetComponent<TMP_Text>().color = Color.red;
        }
    }

    /// <summary>
    /// Advances to the next challenge or completes the game
    /// </summary>
    private void NextChallenge()
    {
        _currentChallengeIndex++;

        if (_currentChallengeIndex >= _cssChallenges.Count)
        {
            feedbackText.GetComponent<TMP_Text>().text = "All challenges completed!";
            inputField.GetComponent<TMP_InputField>().text = "You're a CSS master!";
            feedbackText.GetComponent<TMP_Text>().color = Color.cyan;
            submitBtn.GetComponent<Button>().interactable = false;
            resetBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            LoadChallenge();
            SaveProgress();
        }
    }

    /// <summary>
    /// Loads the current challenge with an incorrect CSS snippet
    /// </summary>
    private void LoadChallenge()
    {
        inputField.GetComponent<TMP_InputField>().text = _cssChallenges[_currentChallengeIndex].Key;
        feedbackText.GetComponent<TMP_Text>().text = "Fix the syntax!";
        feedbackText.GetComponent<TMP_Text>().color = Color.yellow;
    }

    /// <summary>
    /// Resets the current challenge back to its original incorrect CSS snippet
    /// </summary>
    private void ResetCurrentChallenge()
    {
        LoadChallenge();
    }

    /// <summary>
    /// Saves the current challenge index to a file
    /// </summary>
    private void SaveProgress()
    {
        File.WriteAllText(_saveFilePath, _currentChallengeIndex.ToString());
        Debug.Log("Progress saved!");
    }


    // TODO: UNCOMMENT THIS LATER WHEN YOU ACTUALLY MAKE THE GAME
    // /// <summary>
    // /// Loads the saved challenge index from the file and resumes progress
    // /// </summary>
    // private void LoadProgress()
    // {
    //     if (File.Exists(_saveFilePath))
    //     {
    //         var savedIndex = File.ReadAllText(_saveFilePath);
    //         if (int.TryParse(savedIndex, out var index) && index < _cssChallenges.Count)
    //         {
    //             _currentChallengeIndex = index;
    //         }
    //     }
    //     LoadChallenge();
    // }

    /// <summary>
    /// Normalizes CSS input by removing excess spaces and line breaks
    /// </summary>
    private static string NormalizeCss(string input)
    {
        return input;
        // return input.Replace("\n", "").Replace("  ", " ").Trim();
    }

    private void Update()
    {
        // get key down is backspace
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            // get the text area
            var textArea = inputField.GetComponent<TMP_InputField>().textComponent;
            // set the text area to the top
            textArea.transform.localPosition = new Vector3(0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inputField.GetComponent<TMP_InputField>().isFocused)
            {
                inputField.GetComponent<TMP_InputField>().DeactivateInputField();
            }
        }
    }

}
