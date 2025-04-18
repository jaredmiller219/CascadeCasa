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
    /// <summary>
    /// Reference to the global cursor manager for handling cursor changes
    /// </summary>
    private readonly GlobalCursorManager _cursorManager;

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

    /// <summary>
    /// The popup displayed when all challenges are completed
    /// </summary>
    [Header("Lvl End Popup")]
    public GameObject challengeComplete;

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
    /// <param name="eventData">Data related to the pointer event</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Check if the cursor manager is available and the pointer is over the input field
        if (_cursorManager != null && eventData.pointerCurrentRaycast.gameObject == inputField)
        {
            // Change the cursor to the I-beam cursor (index 3 assumed)
            _cursorManager.SetCursor(3);
        }
    }

    /// <summary>
    /// Handles pointer exiting the input field area
    /// </summary>
    /// <param name="eventData">Data related to the pointer event</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        // Check if the cursor manager is available and the pointer is not over the input field
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
        // Retrieve the user's input from the input field and normalize it
        var userInput = inputField.GetComponent<TMP_InputField>().text.Trim().ToLower();

        // Retrieve the correct CSS snippet for the current challenge and normalize it
        var correctCss = _cssChallenges[currentChallengeIndex].Value.ToLower();

        // Normalize the user's input by removing unnecessary spaces and line breaks
        var normalizedUserInput = NormalizeCss(userInput);

        // Normalize the correct CSS snippet for comparison
        var normalizedCorrectCss = NormalizeCss(correctCss);

        // Compare the normalized user input with the normalized correct CSS
        if (normalizedUserInput == normalizedCorrectCss)
        {
            // If the input is correct, display success feedback
            feedbackText.GetComponent<TMP_Text>().text = "Correct!\nLoading next challenge...";
            feedbackText.GetComponent<TMP_Text>().color = Color.green;

            // Load the next challenge after a delay
            Invoke(nameof(NextChallenge), 1.5f);
        }
        else
        {
            // If the input is incorrect, display error feedback
            feedbackText.GetComponent<TMP_Text>().text = "Check colons, semicolons, and syntax!";
            feedbackText.GetComponent<TMP_Text>().color = Color.red;
        }
    }

    /// <summary>
    /// Checks if all challenges have been completed
    /// </summary>
    /// <returns>True if all challenges are completed, otherwise false</returns>
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
        // Increment the current challenge index to move to the next challenge
        currentChallengeIndex++;

        // Check if all challenges have been completed
        if (IsLevelComplete())
        {
            // Display a completion message to the user
            feedbackText.GetComponent<TMP_Text>().text = "All challenges completed!";
            feedbackText.GetComponent<TMP_Text>().color = Color.cyan;

            // Clear the input field and make it non-interactable
            inputField.GetComponent<TMP_InputField>().text = "";
            inputField.GetComponent<TMP_InputField>().interactable = false;

            // Disable the submit and reset buttons
            submitBtn.GetComponent<Button>().interactable = false;
            resetBtn.GetComponent<Button>().interactable = false;

            // Show the challenge completion popup
            challengeComplete.SetActive(true);
        }
        else
        {
            // Load the next challenge if there are more challenges remaining
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
        // Reload the current challenge to reset the input field
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
}
