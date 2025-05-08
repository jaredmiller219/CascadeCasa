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

    private readonly List<string> _instructions = new()
    {
        "CSS lets you style HTML elements by changing things like size and color. For example, you can use width to set how wide something is, and background-color to set its background color.",
        "Look for missing colons in the font size and text align properties.",
        "Ensure the border and margin top properties have colons.",
        "Use a colon after color and font weight properties.",
        "List style type and padding need colons and values.",
        "Colons are required after text decoration and color.",
        "Don't forget colons after width and height.",
        "askldhaskjd",
        "hello",
        "ausndychdnduydndkdk"
    };

    public void Start()
    {
        instructionText.GetComponent<TMP_Text>().text = "";

        float currentY = 0f;

        for (int i = 0; i < _instructions.Count; i++)
        {
            CreateButtonInstatiation(i, currentY);
            currentY -= buttonSize.y + 10f; // Add spacing between buttons
        }
    }

    public void BackToMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void SetText(int index)
    {
        if (index >= 0 && index < _instructions.Count)
        {
            instructionText.GetComponent<TMP_Text>().text = _instructions[index];
        }
        else
        {
            instructionText.GetComponent<TMP_Text>().text = "Instruction not found.";
        }
    }

    private void CreateButtonInstatiation(int index, float yPosition)
    {
        GameObject buttonObj = Instantiate(buttonPrefab, buttonContainer);

        // Configure RectTransform
        RectTransform rect = buttonObj.GetComponent<RectTransform>();
        rect.sizeDelta = buttonSize;
        rect.anchoredPosition = new Vector2(0f, yPosition);

        // Set button text and click behavior
        buttonObj.GetComponentInChildren<TMP_Text>().text = $"Button {index + 1}";
        buttonObj.GetComponent<Button>().onClick.AddListener(() => SetText(index));
    }
}
