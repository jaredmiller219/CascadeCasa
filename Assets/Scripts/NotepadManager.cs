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
    // UI References
    /// <summary>
    /// Text area where users input their CSS solutions
    /// </summary>
    [Tooltip("The notepad input field for CSS code")]
    [Header("Notepad")]
    public GameObject inputField;

    /// <summary>
    /// Displays feedback messages to the user
    /// </summary>
    [Tooltip("The feedback text area for user messages")]
    [Header("Feedback")]
    public GameObject feedbackText;

    // The header for the buttons
    [Header("Buttons")]

    /// <summary>
    /// Button that triggers solution validation
    /// </summary>
    [Tooltip("The submit button for checking CSS code")]
    public GameObject SubmitBtn;

    /// <summary>
    /// Button to reset the current challenge
    /// </summary>
    [Tooltip("The reset button for restarting the challenge")]
    public GameObject ResetBtn;

    // The header for the reset text
    [Header("Reset Text")]

    /// <summary>
    /// Text to display when the reset button is clicked
    /// </summary>
    [Tooltip("The text that appears when the reset button is clicked")]
    public GameObject resetPopup;

    /// <summary>
    /// Path where progress is saved
    /// </summary>
    private string saveFilePath;

    /// <summary>
    /// Tracks current challenge number
    /// </summary>
    private int currentChallengeIndex;

    /// <summary>
    /// List of CSS challenges with incorrect and correct snippets
    /// </summary>
    /// <remarks>
    /// Key = incorrect CSS snippet,
    /// Value = correct CSS snippet
    /// </remarks>
    private readonly List<KeyValuePair<string, string>> cssChallenges = new()
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

    /// <summary>
    /// Initializes the game state and sets up event listeners
    /// </summary>
    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "notepad_progress.txt");
        SubmitBtn.GetComponent<Button>().onClick.AddListener(CheckCSSInput);
        ResetBtn.GetComponent<Button>().onClick.AddListener(ResetCurrentChallenge);
        resetPopup.SetActive(false);

        // Scroll sensitivity value for smooth scrolling
        float ScrollSensitivity = 0.01f;
        // set the scroll sensitivity of the notepadInput
        inputField.GetComponent<TMP_InputField>().scrollSensitivity = ScrollSensitivity;

        // Note: Progress loading is disabled for testing
        // LoadProgress();

        LoadChallenge();
    }

    /// <summary>
    /// Validates user input against the current challenge's correct CSS snippet
    /// </summary>
    public void CheckCSSInput()
    {
        string userInput = inputField.GetComponent<TMP_InputField>().text.Trim().ToLower();
        string correctCSS = cssChallenges[currentChallengeIndex].Value.ToLower();

        // Normalize input (remove extra spaces, new lines)
        string normalizedUserInput = NormalizeCSS(userInput);
        string normalizedCorrectCSS = NormalizeCSS(correctCSS);

        if (normalizedUserInput == normalizedCorrectCSS)
        {
            feedbackText.GetComponent<TMP_Text>().text = "Correct! Loading next challenge...";
            feedbackText.GetComponent<TMP_Text>().color = Color.green;
            Invoke(nameof(NextChallenge), 1.5f);
        }
        else
        {
            feedbackText.GetComponent<TMP_Text>().text = "Incorrect. Check colons, semicolons, and syntax!";
            feedbackText.GetComponent<TMP_Text>().color = Color.red;
        }
    }

    /// <summary>
    /// Advances to the next challenge or completes the game
    /// </summary>
    private void NextChallenge()
    {
        currentChallengeIndex++;

        if (currentChallengeIndex >= cssChallenges.Count)
        {
            feedbackText.GetComponent<TMP_Text>().text = "All challenges completed!";
            inputField.GetComponent<TMP_InputField>().text = "You're a CSS master!";
            feedbackText.GetComponent<TMP_Text>().color = Color.cyan;
            SubmitBtn.GetComponent<Button>().interactable = false;
            ResetBtn.GetComponent<Button>().interactable = false;
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
        inputField.GetComponent<TMP_InputField>().text = cssChallenges[currentChallengeIndex].Key;
        feedbackText.GetComponent<TMP_Text>().text = "Fix the syntax!";
        feedbackText.GetComponent<TMP_Text>().color = Color.yellow;
    }

    /// <summary>
    /// Hides the reset text panel after a delay
    /// </summary>
    private void HidePanel()
    {
        resetPopup.SetActive(false);
    }


    /// <summary>
    /// Resets the current challenge back to its original incorrect CSS snippet
    /// </summary>
    private void ResetCurrentChallenge()
    {
        LoadChallenge();
        resetPopup.SetActive(true);
        // delay for 3 seconds and then hide the resetText panel
        CancelInvoke(nameof(HidePanel)); // Cancel any existing delayed hide
        Invoke(nameof(HidePanel), 1f);   // Start a new delay
    }

    /// <summary>
    /// Saves the current challenge index to a file
    /// </summary>
    private void SaveProgress()
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
            if (int.TryParse(savedIndex, out int index) && index < cssChallenges.Count)
            {
                currentChallengeIndex = index;
            }
        }
        LoadChallenge();
    }

    /// <summary>
    /// Normalizes CSS input by removing excess spaces and line breaks
    /// </summary>
    private string NormalizeCSS(string input)
    {
        return input.Replace("\n", "").Replace("  ", " ").Trim();
    }
}
