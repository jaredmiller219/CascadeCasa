using UnityEngine;

public class Instructions : MonoBehaviour
{
    public GameObject instructionText;

    public void BackToMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void SetText(TMPro.TMP_Text newText)
    {
        instructionText.GetComponent<TMPro.TMP_Text>().text = newText.ToString();
    }
}
