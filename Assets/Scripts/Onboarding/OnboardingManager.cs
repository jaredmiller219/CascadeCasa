using UnityEngine;

public class OnboardingManager : MonoBehaviour
{
    /// <summary>
    /// A reference to the list of gameObjects of each step
    /// </summary>
    [Tooltip("A reference to the list of gameObjects of each step")]
    [SerializeField]
    private GameObject[] tutorialSteps;

    /// <summary>
    /// The current step
    /// </summary>
    [Tooltip("The current step")]
    private int currentStep;

    private void Start()
    {
        // Start of testing
        PlayerPrefs.SetInt("TutorialFinished", 1);
        PlayerPrefs.Save();
        NavigationData.CameFromOnBoarding = true;
        // End of testing

        if (tutorialSteps.Length > 0) ShowStep(0);
        else Debug.LogWarning("No tutorial steps configured!");
    }

    /// <summary>
    /// Activates the tutorial step at the specified index and deactivates all others.
    /// </summary>
    /// <param name="index">The index of the tutorial step to show.</param>
    public void ShowStep(int index)
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

        // Debug.Log($"Showing tutorial step: {index + 1}");
    }

    /// <summary>
    /// Advances to the next tutorial step.
    /// <br />
    /// Calls <see cref="ShowStep"/> if there are remaining steps.
    /// <br />
    /// otherwise ends the tutorial.
    /// </summary>
    public void GoToNextStep()
    {
        currentStep++;
        if (currentStep < tutorialSteps.Length) ShowStep(currentStep);
        else EndTutorial();
    }

    /// <summary>
    /// Ends the tutorial by deactivating all tutorial steps and
    /// transitions to main menu after a delay.
    /// </summary>
    private void EndTutorial()
    {
        foreach (var step in tutorialSteps) if (step) step.SetActive(false);

        // Tell the user they are done
        PlayerPrefs.SetInt("TutorialFinished", 1);
        PlayerPrefs.Save();
        NavigationData.CameFromOnBoarding = true;

        // Delay for 3 seconds

        // Go to main menu


        Debug.Log("Tutorial completed!");
    }

}
