using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

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
    public Vector2 buttonSize = new();

    [System.Serializable]
    private struct InstructionEntry
    {
        public string buttonName;
        public string instructionText;

        public InstructionEntry(string name, string text)
        {
            buttonName = name;
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
        new InstructionEntry("Debug Entry 1", "askldhaskjd"),
        new InstructionEntry("Greeting Tip", "hello"),
        new InstructionEntry("Miscellaneous", "ausndychdnduydndkdk")
    };

    public void Start()
    {
        instructionText.GetComponent<TMP_Text>().text = "";
        float currentY = 0f;

        for (int i = 0; i < _instructions.Count; i++)
        {
            CreateButtonInstatiation(i, currentY);
            currentY -= buttonSize.y + 10f;
        }
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void SetText(int index)
    {
        if (index >= 0 && index < _instructions.Count)
        {
            instructionText.GetComponent<TMP_Text>().text = _instructions[index].instructionText;
        }

        else instructionText.GetComponent<TMP_Text>().text = "Instruction not found.";
    }

    private void CreateButtonInstatiation(int index, float yPosition)
    {
        GameObject buttonObj = Instantiate(buttonPrefab, buttonContainer);

        RectTransform rect = buttonObj.GetComponent<RectTransform>();
        rect.sizeDelta = buttonSize;
        rect.anchoredPosition = new Vector2(0f, yPosition);

        buttonObj.GetComponentInChildren<TMP_Text>().text = _instructions[index].buttonName;
        buttonObj.GetComponent<Button>().onClick.AddListener(() => SetText(index));
    }

}
