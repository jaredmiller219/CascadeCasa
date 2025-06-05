using UnityEngine;
using TMPro;
// using UnityEngine.UI;

public class NotepadLineNumbers : MonoBehaviour
{
    /// <summary>
    /// The input field where users type their CSS solutions.
    /// </summary>
    [Tooltip("The input field where users type their CSS solutions")]
    public TMP_InputField inputField;

    /// <summary>
    /// The TextMeshProUGUI element that displays line numbers.
    /// </summary>
    [Tooltip("The TextMeshProUGUI element that displays line numbers")]
    public TextMeshProUGUI lineNumbersText;

    /// <summary>
    /// The RectTransform of the line numbers text.
    /// </summary>
    private RectTransform lineNumbersRect;

    /// <summary>
    /// The RectTransform of the text area in the input field.
    /// </summary>
    private RectTransform textRect;

    private void Start()
    {
        if (inputField)
        {
            inputField.onValueChanged.AddListener(UpdateLineNumbers);
            UpdateLineNumbers(inputField.text);

            // Get ScrollRect and text RectTransform
            lineNumbersRect = lineNumbersText.GetComponent<RectTransform>();
            textRect = inputField.textComponent.GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        // Sync vertical scroll with the text area
        if (textRect && lineNumbersRect)
        {
            var pos = lineNumbersRect.anchoredPosition;
            pos.y = textRect.anchoredPosition.y;
            lineNumbersRect.anchoredPosition = pos;
        }
    }

    /// <summary>
    /// Updates the line numbers based on the current text in the input field.
    /// </summary>
    /// <param name="text">the text</param>
    private void UpdateLineNumbers(string text)
    {
        int lineCount = text.Split('\n').Length;
        string numbers = "";

        for (int i = 1; i <= lineCount; i++) numbers += i + "\n";
        if (lineNumbersText) lineNumbersText.text = numbers;
    }
}
