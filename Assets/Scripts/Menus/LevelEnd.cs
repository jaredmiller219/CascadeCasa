using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    /// <summary>
    /// Reference to the popup GameObject that appears when the level is completed.
    /// </summary>
    public GameObject completePopup;

    /// <summary>
    /// Reference to the restart button GameObject.
    /// </summary>
    public GameObject levelSelectBtn;

    /// <summary>
    /// Reference to the menu button GameObject.
    /// </summary>
    public GameObject menuBtn;

    private void Start()
    {
        levelSelectBtn.GetComponent<Button>().onClick.AddListener(GoToLevelSelect);
        menuBtn.GetComponent<Button>().onClick.AddListener(GoToMenu);
    }

    /// <summary>
    /// This method is called to display the level completion popup.
    /// </summary>
    public void GoToLevelSelect()
    {
        NavigationData.CameFromLevelComplete = true;
        completePopup.SetActive(false);
        SceneManager.LoadScene("LevelSelect");
    }

    /// <summary>
    /// This method is called to navigate to the main menu.
    /// </summary>
    public void GoToMenu()
    {
        NavigationData.CameFromLevelComplete = false;
        NavigationData.CameFromOnBoarding = false;
        completePopup.SetActive(false);
        SceneManager.LoadScene("Menu");
    }
}
