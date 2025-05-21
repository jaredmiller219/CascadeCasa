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
        new InstructionEntry(
            "Living Room",
            "In this room, you learned the foundational structure of CSS: CSS rules follow the pattern selector { property: value; }. Every property (like background-color, font-size, or width) needs a colon : and ends with a semicolon ;. Hyphenated properties (like margin-top or font-weight) must include the hyphen. This level helped you practice catching common beginner mistakes in syntax."
        ),
        new InstructionEntry(
            "Kitchen",
            "In the Kitchen, you explored how to space and arrange elements using the box model. You learned how margin and padding create space inside and outside elements, and how display: flex can lay out content. You also worked with layout helpers like justify-content, align-items, box-sizing, and max-width."
        ),
        new InstructionEntry(
            "Bedroom1",
            "This room focused on typography and color. You styled text using font-family, font-size, font-weight, line-height, and text-align. You also learned how to decorate and color text with text-decoration, color, and background-color. By combining these properties, you made text readable and expressive."
        ),
        new InstructionEntry(
        "Bedroom2",
        "In Bedroom2, you explored different types of CSS selectors: element selectors (like p), class selectors (.name), ID selectors (#header), and more advanced ones like group selectors and descendant selectors. You also learned how specificity affects which styles get applied, and how to structure combined selectors properly."
        ),

        new InstructionEntry(
        "Bathroom",
        "In the Bathroom, you styled lists and interactive elements. You learned to customize bullets using list-style-type and adjusted spacing with padding and margin. You also explored pseudo-classes like :hover, :active, and :first-child to add interactive effects to buttons and links. These features make your page feel more alive and responsive to users."
        ),
        new InstructionEntry(
            "Patio",
            "In the Patio, you added visual polish using modern CSS properties like transition, box-shadow, border-radius, and text-shadow. These styles add depth, smoothness, and a professional feel to your interfaces. You also practiced combining multiple properties into clean, elegant rule sets — a perfect way to end your journey through the Cascade Casa."
        ),

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

        var image = buttonObj.GetComponent<Image>();
        if (image) image.color = new Color(0.95f, 0.95f, 0.95f, 0.75f);

        buttonObj.GetComponentInChildren<TMP_Text>().text = _instructions[index].buttonName;
        buttonObj.GetComponent<Button>().onClick.AddListener(() => SetText(index));
    }

}
