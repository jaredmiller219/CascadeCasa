// NotepadManager.cs

using System.Collections.Generic;
// using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Manages a CSS learning game where players fix full CSS snippets.
/// </summary>
public class Notepad : MonoBehaviour
{
    private CursorManager cursorManager;

    /// <summary>
    /// Text area where users input their CSS solutions
    /// </summary>
    [Tooltip("The notepad input field for CSS code")]
    [Header("Notepad")]

    // the notepad input field
    public GameObject inputField;

    public TMP_InputField notepad;

    /// <summary>
    /// Displays feedback messages to the user
    /// </summary>
    [Tooltip("The feedback text area for user messages")]
    [Header("Feedback")]
    // TODO: THIS IS ONLY FOR TESTING, REMOVE LATER
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

    // /// <summary>
    // /// Path where progress is saved
    // /// </summary>
    // private string _saveFilePath;

    /// <summary>
    /// Tracks current challenge number
    /// </summary>
    // [HideInInspector]
    [Tooltip("The index of the current challenge")]
    [Header("Challenge Index")]
    public int currentChallengeIndex;

    [Header("Lvl End Popup")]
    public GameObject challengeComplete;

    // private readonly CursorManager mainCursor;

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
        // _saveFilePath = Path.Combine(Application.persistentDataPath, "notepad_progress.txt");
        submitBtn.GetComponent<Button>().onClick.AddListener(CheckCssInput);
        resetBtn.GetComponent<Button>().onClick.AddListener(ResetCurrentChallenge);
        resetPopup.SetActive(false);

        // Scroll sensitivity value for smooth scrolling
        const float scrollSensitivity = 0.01f;
        // set the scroll sensitivity of the notepadInput
        inputField.GetComponent<TMP_InputField>().scrollSensitivity = scrollSensitivity;

        // Set the cursor to the text cursor when the pointer enters the object
        // mainCursor.GetComponent<CursorManager>();

        // Note: Progress loading is disabled for testing
        // LoadProgress();

        // Find or get the CursorManager reference
        if (cursorManager == null)
        {
            cursorManager = GetComponent<CursorManager>();
        }

        LoadChallenge();
    }

    /// <summary>
    /// Validates user input against the current challenge's correct CSS snippet
    /// </summary>
    private void CheckCssInput()
    {
        var userInput = inputField.GetComponent<TMP_InputField>().text.Trim().ToLower();
        var correctCss = _cssChallenges[currentChallengeIndex].Value.ToLower();

        // Normalize input (remove extra spaces, new lines)
        var normalizedUserInput = NormalizeCss(userInput);
        var normalizedCorrectCss = NormalizeCss(correctCss);

        if (normalizedUserInput == normalizedCorrectCss)
        {
            feedbackText.GetComponent<TMP_Text>().text = "Correct!\nLoading next challenge...";
            feedbackText.GetComponent<TMP_Text>().color = Color.green;
            Invoke(nameof(NextChallenge), 1.5f);
        }
        else
        {
            feedbackText.GetComponent<TMP_Text>().text = "Check colons, semicolons, and syntax!";
            feedbackText.GetComponent<TMP_Text>().color = Color.red;
        }
    }

    public bool IsLevelComplete()
    {
        return currentChallengeIndex >= _cssChallenges.Count;
    }

    /// <summary>
    /// Advances to the next challenge or completes the game
    /// </summary>
    private void NextChallenge()
    {
        currentChallengeIndex++;

        if (IsLevelComplete()) {
            feedbackText.GetComponent<TMP_Text>().text = "All challenges completed!";
            // instead of setting the text to "You're a CSS master!", show a popup with the text
            inputField.GetComponent<TMP_InputField>().text = "";
            inputField.GetComponent<TMP_InputField>().interactable = false;
            feedbackText.GetComponent<TMP_Text>().color = Color.cyan;

            // Disable buttons
            submitBtn.GetComponent<Button>().interactable = false;
            resetBtn.GetComponent<Button>().interactable = false;

            // Show the complete popup
            challengeComplete.SetActive(true);

            // Use the cursor manager if available
            if (cursorManager != null)
            {
                // cursorManager.ResetToDefaultCursor();
            }
            else
            {
                Debug.LogWarning("CursorManager reference is missing!");
            }
        }

        else {
            LoadChallenge();
            // SaveProgress();
        }
    }

    /// <summary>
    /// Loads the current challenge with an incorrect CSS snippet
    /// </summary>
    private void LoadChallenge()
    {
        inputField.GetComponent<TMP_InputField>().text = _cssChallenges[currentChallengeIndex].Key;
        feedbackText.GetComponent<TMP_Text>().text = "Fix the syntax!";
        feedbackText.GetComponent<TMP_Text>().color = Color.yellow;
    }

    /// <summary>
    /// Resets the current challenge back to its original incorrect CSS snippet
    /// </summary>
    private void ResetCurrentChallenge(){
        LoadChallenge();
    }

    // /// <summary>
    // /// Saves the current challenge index to a file
    // /// </summary>
    // private void SaveProgress()
    // {
    //     File.WriteAllText(_saveFilePath, currentChallengeIndex.ToString());
    //     Debug.Log("Progress saved!");
    // }


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
        // return input;
        return input.Replace("\n", "").Replace("  ", " ").Trim();
    }

}
