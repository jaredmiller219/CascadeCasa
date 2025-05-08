using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{

    // public GameObject instructionText; // Text display area
    public GameObject buttonPrefab;    // Assign your button prefab in the Inspector
    public Transform buttonContainer;  // A UI parent object (e.g., Vertical Layout Group)

    public GameObject instructionText;

    public Vector2 buttonSize = new(); // Width x Height

    // public GameObject action1;

    // public GameObject action2;

    // public GameObject action3;

    // public GameObject action4;

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

        float startY = 0f;

        for (int i = 0; i < _instructions.Count; i++)
        {
            int index = i;

            GameObject buttonObj = Instantiate(buttonPrefab, buttonContainer);

            // Set size manually
            RectTransform rect = buttonObj.GetComponent<RectTransform>();
            rect.sizeDelta = buttonSize;

            // Set position manually
            rect.anchoredPosition = new Vector2(0f, startY);
            startY -= buttonSize.y + 10f;

            buttonObj.GetComponentInChildren<TMP_Text>().text = $"Button {i + 1}";
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SetText(index));
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

    // public void SetText(int index)
    // {
    //     string newText;

    //     switch (index)
    //     {
    //         case 0:
    //             newText = _instructions[0];
    //             break;
    //         case 1:
    //             newText = _instructions[1];
    //             break;
    //         case 2:
    //             newText = _instructions[2];
    //             break;
    //         case 3:
    //             newText = _instructions[3];
    //             break;
    //         case 4:
    //             newText = _instructions[4];
    //             break;
    //         case 5:
    //             newText = _instructions[5];
    //             break;
    //         case 6:
    //             newText = _instructions[6];
    //             break;
    //         default:
    //             newText = "Instruction not found.";
    //             break;
    //     }

    //     instructionText.GetComponent<TMP_Text>().text = newText;
    // }
}
