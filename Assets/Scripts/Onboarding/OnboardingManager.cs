using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnboardingManager : MonoBehaviour
{
    /// <summary>
    /// A reference to the list of gameObjects of each step
    /// </summary>
    [Tooltip("A reference to the list of gameObjects of each step")]
    [SerializeField]
    private GameObject[] tutorialSteps;

    /// <summary>
    /// THe dropdown GameObject
    /// </summary>
    [Tooltip("A reference to the Dropdown GameObject")]
    [SerializeField]
    private GameObject dropdown;

    /// <summary>
    /// The current step
    /// </summary>
    [Tooltip("The current step")]
    private int currentStep;

#if UNITY_EDITOR
    private static bool testingSetupDone;
#endif

    private void Start()
    {
        // TESTING
#if UNITY_EDITOR
        if (!testingSetupDone)
        {
            PlayerPrefs.SetInt("TutorialFinished", 1);
            PlayerPrefs.Save();
            NavigationData.CameFromOnBoarding = true;
            testingSetupDone = true;
        }
#endif
        // DONE TESTING
        // var dropdownMenu = dropdown.GetComponent<TMPro.TMP_Dropdown>();
        dropdown.SetActive(true);
        if (tutorialSteps.Length > 0) ShowStep(0);
        else Debug.LogWarning("No tutorial steps configured!");
    }

    /// <summary>
    /// Activates the tutorial step at the specified index and deactivates all others.
    /// </summary>
    /// <param name="index">The index of the tutorial step to show.</param>
    private void ShowStep(int index)
    {
        if (index < 0 || index >= tutorialSteps.Length)
        {
            Debug.LogError($"Tutorial step index {index} is out of range!");
            return;
        }

        var stepNumber = 0;
        foreach (var step in tutorialSteps)
        {
            if (!step)
            {
                Debug.LogError($"Step {stepNumber + 1} has no GameObject assigned!");
                continue;
            }
            step.SetActive(stepNumber == index);
            stepNumber++;
        }
    }

    /// <summary>
    /// Advances to the next tutorial step.
    /// <br />
    /// Calls <see cref="ShowStep"/> if there are remaining steps.
    /// <br />
    /// Otherwise, ends the tutorial.
    /// </summary>
    public void GoToNextStep()
    {
        currentStep++;
        if (currentStep < tutorialSteps.Length) ShowStep(currentStep);
        else EndTutorial();
    }

    /// <summary>
    /// Ends the tutorial by deactivating all tutorial steps and
    /// transitions to the main menu after a delay.
    /// </summary>
    private void EndTutorial()
    {
        foreach (var step in tutorialSteps) if (step) step.SetActive(false);

        // Tell the user they are done
        PlayerPrefs.SetInt("TutorialFinished", 1);
        PlayerPrefs.Save();
        NavigationData.CameFromOnBoarding = true;

        // Go to the main menu
        // Debug.Log("Tutorial completed!");
        StartCoroutine(DelayedLoadMenu());
    }


    private IEnumerator DelayedLoadMenu()
    {
        dropdown.SetActive(false);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Menu");
    }

}
