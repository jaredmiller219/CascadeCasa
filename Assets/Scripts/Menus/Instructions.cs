using UnityEngine;
using TMPro;

public class Instructions : MonoBehaviour
{
    public GameObject instructionText;

    public void Start()
    {
        instructionText.GetComponent<TMP_Text>().text = "";
    }

    public void BackToMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void SetText(string newText)
    {
        instructionText.GetComponent<TMP_Text>().text = newText;
    }
}
