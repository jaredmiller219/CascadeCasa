using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Manages a CSS learning game where players fix full CSS snippets.
/// </summary>
public class KitchenNotepad : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    // $$$$ Click sound fields
    [Header("Audio")] // $$$$
    public AudioSource audioSource; // $$$$
    public AudioClip clickSound; // $$$$

    /// <summary>
    /// List of CSS challenges with incorrect and correct snippets.
    /// </summary>
    private readonly List<KeyValuePair<string, string>> _cssChallenges = new()
    {
        new KeyValuePair<string, string>(
            "div {\n    background color blue;\n    width: 100px;\n}",
            "div {\n    background-color: blue;\n    width: 100px;\n}"
        ),
        new KeyValuePair<string, string>(
            "p {\n    font size 20px;\n    text align center;\n}",
            "p {\n    font-size: 20px;\n    text-align: center;\n}"
        ),
        new KeyValuePair<string, string>(
            ".box {\n    border 2px solid black;\n    margin top 10px;\n}",
            ".box {\n    border: 2px solid black;\n    margin-top: 10px;\n}"
        )
    };

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
        if (_cursorManager != null && eventData.pointerCurrentRaycast.gameObject == inputField)
        {
            _cursorManager.SetCursor(3);
        }
    }

    /// <summary>
    /// Handles pointer exiting the input field area
    /// </summary>
    /// <param name="eventData">Data related to the pointer event</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_cursorManager != null && eventData.pointerCurrentRaycast.gameObject != inputField)
        {
            _cursorManager.SetCursor(_cursorManager.GetSelectedCursor());
        }
    }

    /// <summary>
    /// Validates user input against the current challenge's correct CSS snippet
    /// </summary>
    private void CheckCssInput()
    {
        PlayClickSound(); // $$$$

        var userInput = inputField.GetComponent<TMP_InputField>().text.Trim().ToLower();
        var correctCss = _cssChallenges[currentChallengeIndex].Value.ToLower();

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
            feedbackText.GetComponent<TMP_Text>().text = "All challenges completed!";
            feedbackText.GetComponent<TMP_Text>().color = Color.cyan;

            inputField.GetComponent<TMP_InputField>().text = "";
            inputField.GetComponent<TMP_InputField>().interactable = false;

            submitBtn.GetComponent<Button>().interactable = false;
            resetBtn.GetComponent<Button>().interactable = false;

            challengeComplete.SetActive(true);
        }
        else
        {
            LoadChallenge();
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
    private void ResetCurrentChallenge()
    {
        PlayClickSound(); // $$$$
        LoadChallenge();
    }

    /// <summary>
    /// Normalizes CSS input by removing excess spaces and line breaks
    /// </summary>
    /// <param name="input">The CSS string to normalize</param>
    /// <returns>A normalized CSS string</returns>
    private static string NormalizeCss(string input)
    {
        return input.Replace("\n", "").Replace("  ", " ").Trim();
    }

    /// <summary>
    /// Plays the UI click sound if available
    /// </summary>
    private void PlayClickSound() // $$$$
    { // $$$$
        if (audioSource && clickSound) // $$$$
            audioSource.PlayOneShot(clickSound); // $$$$
    } // $$$$
}
