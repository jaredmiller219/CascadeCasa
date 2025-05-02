using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Notepad : MonoBehaviour
{
    private GlobalCursorManager _cursorManager;
    private DraggableImage selectedImage;

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

    [Header("Lvl End Popup")]
    public GameObject challengeComplete;

    [Header("Hint Section")]
    public GameObject hintText;

    private readonly string saveFilePath;

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

    private readonly List<string> _cssHints = new()
    {
        "CSS lets you style HTML elements by changing things like size and color." +
        "For example, you can use width to set how wide something is, " +
        "and background-color to set its background color.",

        "Look for missing colons in the font size and text align properties.",

        "Ensure the border and margin top properties have colons."
    };

    private int _previousCursorIndex;

    public AudioSource audioSource; // $$$$
    public AudioClip clickSound; // $$$$

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

        LoadChallenge();
    }

    public Notepad()
    {
        selectedImage = null;
    }

    public void SelectImage(DraggableImage image)
    {
        this.selectedImage = image;
        Console.WriteLine("Image selected for editing.");
    }

    public void SubmitCSS(string cssText)
    {
        if (selectedImage != null)
        {
            selectedImage.ApplyCSS(cssText);
        }
        else
        {
            Console.WriteLine("No image selected to apply CSS to.");
        }
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

    private void CheckCssInput()
    {
        if (audioSource && clickSound) // $$$$
        { // $$$$
            audioSource.PlayOneShot(clickSound); // $$$$
        } // $$$$

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

    private bool IsLevelComplete()
    {
        return currentChallengeIndex >= _cssChallenges.Count;
    }

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

    private void LoadChallenge()
    {
        inputField.GetComponent<TMP_InputField>().text = _cssChallenges[currentChallengeIndex].Key;
        hintText.GetComponent<TMP_Text>().text = _cssHints[currentChallengeIndex];
        feedbackText.GetComponent<TMP_Text>().text = "Fix the syntax!";
        feedbackText.GetComponent<TMP_Text>().color = Color.yellow;
    }

    private void ResetCurrentChallenge()
    {
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
