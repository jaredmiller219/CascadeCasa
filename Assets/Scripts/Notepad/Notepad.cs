// NotepadManager.cs

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Manages a CSS learning game where players fix full CSS snippets.
/// </summary>
public class Notepad : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Reference to the global cursor manager for handling cursor changes
    private readonly GlobalCursorManager _cursorManager;

    /// <summary>
    /// Text area where users input their CSS solutions
    /// </summary>
    [Tooltip("The notepad input field for CSS code")]
    [Header("Notepad")]
    public GameObject inputField; // The input field for user CSS code

    /// <summary>
    /// Displays feedback messages to the user
    /// </summary>
    [Tooltip("The feedback text area for user messages")]
    [Header("Feedback")]
    public GameObject feedbackText; // Text area for displaying feedback to the user

    // The header for the buttons
    [Header("Buttons")]

    // Button that triggers solution validation
    [Tooltip("The submit button for checking CSS code")]
    public GameObject submitBtn; // Button to submit the user's CSS solution

    /// <summary>
    /// Button to reset the current challenge
    /// </summary>
    [Tooltip("The reset button for restarting the challenge")]
    public GameObject resetBtn; // Button to reset the current challenge

    // The header for the reset text
    [Header("Reset Text")]

    // Text to display when the reset button is clicked
    [Tooltip("The text that appears when the reset button is clicked")]
    public GameObject resetPopup; // Popup text displayed when the reset button is clicked

    /// <summary>
    /// Tracks current challenge number
    /// </summary>
    [Tooltip("The index of the current challenge")]
    [Header("Challenge Index")]
    public int currentChallengeIndex; // Index of the current challenge being solved

    [Header("Lvl End Popup")]
    public GameObject challengeComplete; // Popup displayed when all challenges are completed

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
        // Challenge 1: Fix the missing colon in "background color"
        new KeyValuePair<string, string>(
            "div {\n    background color blue;\n    width: 100px;\n}", // Incorrect
            "div {\n    background-color: blue;\n    width: 100px;\n}" // Correct
        ),
        // Challenge 2: Fix the missing colons in "font size" and "text align"
        new KeyValuePair<string, string>(
            "p {\n    font size 20px;\n    text align center;\n}", // Incorrect
            "p {\n    font-size: 20px;\n    text-align: center;\n}" // Correct
        ),
        // Challenge 3: Fix the missing colons in "border" and "margin top"
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
        // Attach the CheckCssInput method to the submit button's click event
        submitBtn.GetComponent<Button>().onClick.AddListener(CheckCssInput);

        // Attach the ResetCurrentChallenge method to the reset button's click event
        resetBtn.GetComponent<Button>().onClick.AddListener(ResetCurrentChallenge);

        // Ensure the reset popup is hidden at the start
        resetPopup.SetActive(false);

        // Set the scroll sensitivity for the input field to allow smooth scrolling
        const float scrollSensitivity = 0.01f;
        inputField.GetComponent<TMP_InputField>().scrollSensitivity = scrollSensitivity;

        // Load the first challenge
        LoadChallenge();
    }

    /// <summary>
    /// Handles pointer entering the input field area
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // If the cursor manager is available and the pointer is over the input field
        if (_cursorManager != null && eventData.pointerCurrentRaycast.gameObject == inputField)
        {
            // Change the cursor to the I-beam cursor (index 3 assumed)
            _cursorManager.SetCursor(3);
        }
    }

    /// <summary>
    /// Handles pointer exiting the input field area
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        // If the cursor manager is available and the pointer is not over the input field
        if (_cursorManager != null && eventData.pointerCurrentRaycast.gameObject != inputField)
        {
            // Reset the cursor to the previously selected cursor
            _cursorManager.SetCursor(_cursorManager.GetSelectedCursor());
        }
    }

    /// <summary>
    /// Validates user input against the current challenge's correct CSS snippet
    /// </summary>
    private void CheckCssInput()
    {
        // Get the user's input from the input field and normalize it
        var userInput = inputField.GetComponent<TMP_InputField>().text.Trim().ToLower();
        var correctCss = _cssChallenges[currentChallengeIndex].Value.ToLower();

        // Normalize both user input and the correct CSS for comparison
        var normalizedUserInput = NormalizeCss(userInput);
        var normalizedCorrectCss = NormalizeCss(correctCss);

        // Check if the normalized user input matches the correct CSS
        if (normalizedUserInput == normalizedCorrectCss)
        {
            // Display success feedback and load the next challenge after a delay
            feedbackText.GetComponent<TMP_Text>().text = "Correct!\nLoading next challenge...";
            feedbackText.GetComponent<TMP_Text>().color = Color.green;
            Invoke(nameof(NextChallenge), 1.5f);
        }
        else
        {
            // Display error feedback if the input is incorrect
            feedbackText.GetComponent<TMP_Text>().text = "Check colons, semicolons, and syntax!";
            feedbackText.GetComponent<TMP_Text>().color = Color.red;
        }
    }

    /// <summary>
    /// Checks if all challenges have been completed
    /// </summary>
    private bool IsLevelComplete()
    {
        // Return true if the current challenge index exceeds the total number of challenges
        return currentChallengeIndex >= _cssChallenges.Count;
    }

    /// <summary>
    /// Advances to the next challenge or completes the game
    /// </summary>
    private void NextChallenge()
    {
        // Increment the current challenge index
        currentChallengeIndex++;

        // If all challenges are completed
        if (IsLevelComplete())
        {
            // Display a completion message and disable input
            feedbackText.GetComponent<TMP_Text>().text = "All challenges completed!";
            feedbackText.GetComponent<TMP_Text>().color = Color.cyan;
            inputField.GetComponent<TMP_InputField>().text = "";
            inputField.GetComponent<TMP_InputField>().interactable = false;

            // Disable buttons
            submitBtn.GetComponent<Button>().interactable = false;
            resetBtn.GetComponent<Button>().interactable = false;

            // Show the challenge completion popup
            challengeComplete.SetActive(true);
        }
        else
        {
            // Load the next challenge
            LoadChallenge();
        }
    }

    /// <summary>
    /// Loads the current challenge with an incorrect CSS snippet
    /// </summary>
    private void LoadChallenge()
    {
        // Set the input field text to the incorrect CSS snippet for the current challenge
        inputField.GetComponent<TMP_InputField>().text = _cssChallenges[currentChallengeIndex].Key;

        // Display a message prompting the user to fix the syntax
        feedbackText.GetComponent<TMP_Text>().text = "Fix the syntax!";
        feedbackText.GetComponent<TMP_Text>().color = Color.yellow;
    }

    /// <summary>
    /// Resets the current challenge back to its original incorrect CSS snippet
    /// </summary>
    private void ResetCurrentChallenge()
    {
        // Reload the current challenge
        LoadChallenge();
    }

    /// <summary>
    /// Normalizes CSS input by removing excess spaces and line breaks
    /// </summary>
    private static string NormalizeCss(string input)
    {
        // Remove newlines and extra spaces, then trim the result
        return input.Replace("\n", "").Replace("  ", " ").Trim();
    }
}
