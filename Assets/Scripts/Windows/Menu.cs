using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("Buttons (Parents with Button Component)")]
    public GameObject levelSelectButton;
    public GameObject playButton;
    public GameObject instructionsButton;

    private TMP_Text levelSelectText;
    private TMP_Text playText;
    private TMP_Text instructionsText;

    private void Start()
    {
        // Get TMP_Text from each button’s children
        levelSelectText = levelSelectButton.GetComponentInChildren<TMP_Text>();
        playText = playButton.GetComponentInChildren<TMP_Text>();
        instructionsText = instructionsButton.GetComponentInChildren<TMP_Text>();
    }

    public void OnLevelSelectPress()
    {
        SetPressedColor(levelSelectText);
    }

    public void OnLevelSelectRelease()
    {
        SetDefaultColor(levelSelectText);
        LevelSelect();
    }

    public void OnPlayPress()
    {
        SetPressedColor(playText);
    }

    public void OnPlayRelease()
    {
        SetDefaultColor(playText);
    }

    public void OnInstructionsPress()
    {
        SetPressedColor(instructionsText);
    }

    public void OnInstructionsRelease()
    {
        SetDefaultColor(instructionsText);
    }

    private void SetPressedColor(TMP_Text text)
    {
        if (text != null)
            text.color = new Color32(200, 200, 200, 255); // Light gray
    }

    private void SetDefaultColor(TMP_Text text)
    {
        if (text != null)
            text.color = new Color32(255, 255, 255, 255); // White
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
}
