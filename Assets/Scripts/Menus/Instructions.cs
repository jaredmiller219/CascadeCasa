using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Instructions : MonoBehaviour
{
    public GameObject instructionText;

    public GameObject action1;

    public GameObject action2;

    public GameObject action3;

    // public GameObject action4;

    // public GameObject action5;

    // public GameObject action6;

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
    }

    public void BackToMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void SetText(int index)
    {
        string newText;

        switch (index)
        {
            case 0:
                newText = _instructions[0];
                break;
            case 1:
                newText = _instructions[1];
                break;
            case 2:
                newText = _instructions[2];
                break;
            case 3:
                newText = _instructions[3];
                break;
            case 4:
                newText = _instructions[4];
                break;
            case 5:
                newText = _instructions[5];
                break;
            case 6:
                newText = _instructions[6];
                break;
            default:
                newText = "Instruction not found.";
                break;
        }

        instructionText.GetComponent<TMP_Text>().text = newText;
    }
}
