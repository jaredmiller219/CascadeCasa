using UnityEngine;

public class OnboardingManager : MonoBehaviour
{
    public GameObject[] tutorialSteps;
    private int currentStep = 0;

    private void Start()
    {
        ShowStep(0);
    }

    public void ShowStep(int index)
    {
        for (var i = 0; i < tutorialSteps.Length; i++)
            tutorialSteps[i].SetActive(i == index);
    }

    public void GoToNextStep()
    {
        currentStep++;
        if (currentStep < tutorialSteps.Length) ShowStep(currentStep);
        else EndTutorial();
    }

    private void EndTutorial()
    {
        foreach (var step in tutorialSteps) step.SetActive(false);
    }
}
