using UnityEngine;

public class OnboardingManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialSteps;
    private int currentStep;

    private void Start()
    {
        if (tutorialSteps.Length > 0) ShowStep(0);
        else Debug.LogWarning("No tutorial steps configured!");
    }

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
        
        Debug.Log($"Showing tutorial step: {index + 1}");
    }

    public void GoToNextStep()
    {
        currentStep++;
        if (currentStep < tutorialSteps.Length) ShowStep(currentStep);
        else EndTutorial();
    }

    private void EndTutorial()
    {
        foreach (var step in tutorialSteps)
        {
            if (step) step.SetActive(false);
        }
        Debug.Log("Tutorial completed!");
    }
}