using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LivingRoom_Notepad : MonoBehaviour
{
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
    [HideInInspector]
    public int currentChallengeIndex;

    [HideInInspector]
    public int buttonIndex;

    [Header("Lvl End Popup")]
    public GameObject challengeComplete;

    [Header("Hint Section")]
    public GameObject hintText;

    [Header("Room Transition")]
    public Image backgroundImage;
    public Sprite furnishedRoomSprite;
    public AudioClip successJingle;

    [Header("Journal")]
    public LivingRoom_Journal journal;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip successSound;
    public AudioClip errorSound;

    [HideInInspector]
    public bool canReset;

    [HideInInspector]
    public bool canSubmit;

    public int levelsCompleted;

    [Tooltip("The reference to the gameObject with the HorizontalScrollBar")]
    [SerializeField]
    [Header("Notepad")]
    private LivingRoom_HorizontalScrollBar scrollBar;

    private string saveFilePath;
    private GlobalCursorManager _cursorManager;
    private LivingRoom_ChallengeImage selectedImage;
    private Dictionary<int, string> savedTexts = new();
    private int _previousCursorIndex;

    private readonly List<string> _cssHints = new()
    {
        "Use a colon (:) between property and value. Properties like background-color and width define how elements look.",
        "Text styling: font-size and text-align both need colons and semicolons. Always hyphenate compound property names.",
        "Borders and margins are common layout tools. Remember: margin-top and border use hyphens.",
        "Use color and font-weight to style text. Both need colons, and the values go after them.",
        "Lists use 'list-style-type' to control bullets and 'padding' for spacing. Double-check spelling and colons.",
        "Links are styled with 'text-decoration' and 'color'. Use hyphens for compound properties.",
        "Width and height often go together. Each needs a colon and unit like px or %.",
        "Practice combining multiple properties in one rule block. Don't forget a semicolon at the end of each line.",
        "CSS selectors like .class or #id target specific elements. Check your dots and hashes!",
        "This final one is a recap — remember colons, semicolons, and consistent spacing. You’ve got this!"
    };

    private void Start()
    {
        submitBtn.GetComponent<Button>().onClick.AddListener(CheckCssInput);
        resetBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (audioSource && clickSound) audioSource.PlayOneShot(clickSound);
            ResetCurrentChallenge();
            ChangeFocusTo(null);
        });

        resetPopup.SetActive(false);
        inputField.GetComponent<TMP_InputField>().scrollSensitivity = 0.01f;

        _cursorManager = GlobalCursorManager.Instance;
        if (_cursorManager) _previousCursorIndex = GlobalCursorManager.GetSelectedCursor();

        scrollBar = FindFirstObjectByType<LivingRoom_HorizontalScrollBar>();
        if (!scrollBar) Debug.LogError("LivingRoom_HorizontalScrollBar not found in scene!");

        saveFilePath = Path.Combine(Application.persistentDataPath, "notepad_progress.json");

        canReset = false;
        canSubmit = false;
        currentChallengeIndex = -1;
        levelsCompleted = 0;

        LoadProgress();
    }

    private static void ChangeFocusTo(GameObject gameObj) => EventSystem.current.SetSelectedGameObject(gameObj);

    public void SaveTextForIndex(int index)
    {
        savedTexts[index] = inputField.GetComponent<TMP_InputField>().text;
        SaveProgress();
    }

    public void SaveCurrentInputIfNeeded()
    {
        if (currentChallengeIndex < 0 || !inputField.activeSelf) return;
        savedTexts[currentChallengeIndex] = inputField.GetComponent<TMP_InputField>().text;
        SaveProgress();
    }

    public void SetCssText(string css)
    {
        if (!inputField) return;
        SetTextOfComponent(inputField, css, Color.black, true);
    }

    public void OnInputFieldEnter()
    {
        _previousCursorIndex = GlobalCursorManager.GetSelectedCursor();
        _cursorManager.SetCursor(6);
    }

    public void OnInputFieldExit() => _cursorManager.SetCursor(_previousCursorIndex);

    private static void SetButtonInteractable(GameObject button, bool isInteractable)
    {
        button.GetComponent<Button>().interactable = isInteractable;
    }

    private static void SetTextOfComponent(GameObject textObject, string text, Color color, bool isInteractable)
    {
        if (textObject.TryGetComponent(out TMP_InputField inputField))
        {
            inputField.text = text;
            inputField.textComponent.color = color;
            inputField.interactable = isInteractable;
        }
        else if (textObject.TryGetComponent(out TMP_Text tmpText))
        {
            tmpText.text = text;
            tmpText.color = color;
        }
    }

    private static string InputFieldStrToLower(GameObject inputField)
    {
        return inputField.TryGetComponent<TMP_InputField>(out var input) ? input.text.Trim().ToLower() : null;
    }

    private static string ScrollBarStrValToLower(LivingRoom_HorizontalScrollBar scrollBar, int index)
    {
        return scrollBar.CssChallenges[index].Value.ToLower();
    }

    private void CheckCssInput()
    {
        if (audioSource && clickSound) audioSource.PlayOneShot(clickSound);

        if (inputField.GetComponent<TMP_InputField>().text == "" || !canSubmit) return;
        var normalizedUserInput = NormalizeCss(InputFieldStrToLower(inputField));
        var normalizedCorrectCss = NormalizeCss(ScrollBarStrValToLower(scrollBar, currentChallengeIndex));

        if (normalizedUserInput == normalizedCorrectCss)
        {
            if (audioSource && successSound) audioSource.PlayOneShot(successSound);
            SetTextOfComponent(feedbackText, "Correct!", Color.green, false);
            if (scrollBar) scrollBar.MarkChallengeCompleted(buttonIndex);
            SaveProgress();
            levelsCompleted++;
            SetTextOfComponent(inputField, "", Color.clear, false);
            ChangeFocusTo(null);
            if (!IsLevelComplete()) return;
            LevelComplete();
        }
        else
        {
            if (audioSource && errorSound) audioSource.PlayOneShot(errorSound);
            SetTextOfComponent(feedbackText, "Check colons, semicolons, dashes, and syntax!", Color.red, false);
        }
    }

    private bool IsLevelComplete() => levelsCompleted == scrollBar.imageSprites.Length;

    private void LevelComplete()
    {
        if (journal) journal.CloseJournal();
        if (backgroundImage && furnishedRoomSprite) backgroundImage.sprite = furnishedRoomSprite;
        if (audioSource && successJingle) audioSource.PlayOneShot(successJingle);
        StartCoroutine(ShowPopupAfterDelay(1.2f));
    }

    private IEnumerator ShowPopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetTextOfComponent(feedbackText, "All challenges completed!", Color.cyan, false);
        SetTextOfComponent(inputField, "", Color.clear, false);
        SetButtonInteractable(submitBtn, false);
        SetButtonInteractable(resetBtn, false);
        challengeComplete.SetActive(true);
    }

    public void LoadChallenge()
    {
        currentChallengeIndex = selectedImage ? selectedImage.buttonIndex : buttonIndex;
        LoadInputForChallenge(currentChallengeIndex);
        UpdateChallengeUI(currentChallengeIndex);
    }

    private void LoadInputForChallenge(int challengeIndex)
    {
        if (savedTexts.TryGetValue(challengeIndex, out var savedInput) && !string.IsNullOrWhiteSpace(savedInput))
        {
            SetTextOfComponent(inputField, savedInput, Color.black, true);
        }
        else SetTextOfComponent(inputField, scrollBar.CssChallenges[challengeIndex].Key, Color.black, true);
    }

    private void UpdateChallengeUI(int challengeIndex)
    {
        SetTextOfComponent(hintText, _cssHints[challengeIndex], Color.black, false);
        SetTextOfComponent(feedbackText, "Fix the syntax!", Color.yellow, false);
    }

    public void ResetCurrentChallenge()
    {
        if (currentChallengeIndex == -1)
        {
            SetTextOfComponent(inputField, "", Color.black, true);
            return;
        }
        savedTexts.Remove(currentChallengeIndex);
        SetTextOfComponent(inputField, scrollBar.CssChallenges[currentChallengeIndex].Key, Color.black, true);
        UpdateChallengeUI(currentChallengeIndex);
        LoadChallenge();
    }

    private static string NormalizeCss(string input)
    {
        return input.Replace("\n", "").Replace("  ", " ").Trim();
    }

    [System.Serializable]
    private class SaveData
    {
        public int currentChallengeIndex;
        public List<ChallengeEntry> challenges = new();
    }

    [System.Serializable]
    private class ChallengeEntry
    {
        public int index;
        public string entryText;
    }

    public void SaveProgress()
    {
        var data = new SaveData { currentChallengeIndex = currentChallengeIndex };
        foreach (var kvp in savedTexts)
        {
            data.challenges.Add(new ChallengeEntry { index = kvp.Key, entryText = kvp.Value });
        }
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
    }

    private void LoadProgress()
    {
        if (File.Exists(saveFilePath))
        {
            var json = File.ReadAllText(saveFilePath);
            var data = JsonUtility.FromJson<SaveData>(json);
            currentChallengeIndex = data.currentChallengeIndex;
            savedTexts.Clear();
            foreach (var entry in data.challenges.Where(entry => !string.IsNullOrWhiteSpace(entry.entryText)))
            {
                savedTexts[entry.index] = entry.entryText;
            }
        }
        LoadChallenge();
    }
}