using UnityEngine;

public class OnboardingManager : MonoBehaviour
{
    public GameObject[] tutorialSteps;  
    private int currentStep = 0;

    void Start()
    {
        ShowStep(0); 
    }

    public void ShowStep(int index)
    {
        for (int i = 0; i < tutorialSteps.Length; i++)
        {
            tutorialSteps[i].SetActive(i == index);  
        }
    }

    public void NextStep()
    {
        currentStep++;
        if (currentStep < tutorialSteps.Length)
        {
            ShowStep(currentStep);
        }
        else
        {
            EndTutorial();
        }
    }

    void EndTutorial()
    {
        Debug.Log("Tutorial Finished!");
    }
}
