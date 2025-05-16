using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour
{
    /// <summary>
    /// The button itself to instantiate
    /// </summary>
    public GameObject buttonPrefab;

    /// <summary>
    /// The content area of the buttons
    /// </summary>
    public Transform buttonContainer;

    /// <summary>
    /// The text component for each instruction
    /// </summary>
    public GameObject instructionText;

    /// <summary>
    /// The size of each button
    /// </summary>
    public Vector2 buttonSize;

    /// <summary>
    /// A structure composed of the button name and the instruction's text to set.
    /// </summary>
    [System.Serializable]
    private struct InstructionEntry
    {
        /// <summary>
        /// The button's name
        /// </summary>
        public string buttonName;

        /// <summary>
        /// The instruction text
        /// </summary>
        public string instructionText;

        /// <summary>
        /// The constructor for the instruction entry.
        /// </summary>
        /// <param name="btnName">The name of the button for the related instruction</param>
        /// <param name="text">The text of the instruction to set</param>
        public InstructionEntry(string btnName, string text)
        {
            buttonName = btnName;
            instructionText = text;
        }
    }

    /// <summary>
    ///  Contains a button name and the instruction text for it
    /// </summary>
    private readonly List<InstructionEntry> _instructions = new()
    {
        new InstructionEntry("CSS Basics", "CSS lets you style HTML elements by changing things like size and color. For example, you can use width to set how wide something is, and background-color to set its background color."),
        new InstructionEntry("Font Fixes", "Look for missing colons in the font size and text align properties."),
        new InstructionEntry("Borders & Margins", "Ensure the border and margin top properties have colons."),
        new InstructionEntry("Colors & Fonts", "Use a colon after color and font weight properties."),
        new InstructionEntry("Lists & Padding", "List style type and padding need colons and values."),
        new InstructionEntry("Text Decoration", "Colons are required after text decoration and color."),
        new InstructionEntry("Size Settings", "Don't forget colons after width and height."),
        new InstructionEntry("Debug Entry 1", "more text"),
        new InstructionEntry("Greeting Tip", "hello"),
        new InstructionEntry("Miscellaneous", "here's some text here")
    };

    public void Start()
    {
        instructionText.GetComponent<TMP_Text>().text = "";
        var currentY = 0f;

        for (var i = 0; i < _instructions.Count; i++)
        {
            CreateButtonInstantiation(i, currentY);
            currentY -= buttonSize.y + 10f;
        }
    }

    /// <summary>
    /// Load the scene after 1 second
    /// </summary>
    /// <param name="sceneName">The name of the scene to load</param>
    /// <returns>IEnumerator</returns>
    private static IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Go back to the menu screen
    /// </summary>
    public void BackToMenu()
    {
        StartCoroutine(LoadSceneWithDelay(NavigationData.PreviousScene));
    }

    /// <summary>
    /// Set the instruction text component
    /// </summary>
    /// <param name="index">The index of which to set the text to</param>
    public void SetText(int index)
    {
        if (index >= 0 && index < _instructions.Count)
        {
            instructionText.GetComponent<TMP_Text>().text = _instructions[index].instructionText;
        }

        else instructionText.GetComponent<TMP_Text>().text = "Instruction not found.";
    }

    /// <summary>
    /// Create the button for the instruction list
    /// </summary>
    /// <param name="index">The index of the button</param>
    /// <param name="yPosition">The y-position of the button</param>
    private void CreateButtonInstantiation(int index, float yPosition)
    {
        var buttonObj = Instantiate(buttonPrefab, buttonContainer);

        var rect = buttonObj.GetComponent<RectTransform>();
        rect.sizeDelta = buttonSize;
        rect.anchoredPosition = new Vector2(0f, yPosition);

        buttonObj.GetComponentInChildren<TMP_Text>().text = _instructions[index].buttonName;
        buttonObj.GetComponent<Button>().onClick.AddListener(() => SetText(index));
    }

}
