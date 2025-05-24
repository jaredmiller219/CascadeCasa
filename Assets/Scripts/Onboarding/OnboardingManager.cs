using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class InteractableStep
{
    [Tooltip("Main interactable GameObjects for this step")]
    public List<GameObject> mainInteractables = new();
}

public class TutorialManager : MonoBehaviour
{
    [Tooltip("Dropdown GameObject reference")]
    [SerializeField] private GameObject dropdown;

    [Tooltip("List of tutorial step GameObjects")]
    [SerializeField] private GameObject[] tutorialSteps;

    [Tooltip("List of interactable data per tutorial step")]
    [SerializeField] private List<InteractableStep> interactableItems = new();

    private List<GameObject> currentStepInteractables = new();

    private HashSet<GameObject> cumulativeInteractables = new();
    private int currentStep;

    private void Start()
    {
        if (dropdown) dropdown.SetActive(true);
        else Debug.LogWarning("Dropdown reference not set!");

        if (tutorialSteps.Length > 0) ShowStep(0);
        else Debug.LogWarning("No tutorial steps configured!");
    }

    private void ShowStep(int index)
    {
        if (index < 0 || index >= tutorialSteps.Length)
        {
            Debug.LogError($"Tutorial step index {index} is out of range!");
            return;
        }

        for (int i = 0; i < tutorialSteps.Length; i++)
            tutorialSteps[i].SetActive(i == index);

        if (index >= interactableItems.Count)
        {
            Debug.LogWarning($"No interactable data configured for step {index}");
            return;
        }

        DisableAllInteractionsAndHighlights();

        // Reset list for current step's main interactables
        currentStepInteractables.Clear();

        // Update current step
        int previousStep = currentStep;
        currentStep = index;

        var step = interactableItems[index];

        // Add current step's interactables to the cumulative list
        foreach (var mainGO in step.mainInteractables)
        {
            if (!cumulativeInteractables.Contains(mainGO))
                cumulativeInteractables.Add(mainGO);

            currentStepInteractables.Add(mainGO); // Used to detect valid triggers
        }

        // Re-enable all cumulative interactables
        foreach (var go in cumulativeInteractables)
        {
            SetInteractable(go, true);
        }

        // Highlight and hook only current step’s interactables
        foreach (var mainGO in step.mainInteractables)
        {
            AddHighlight(mainGO);
            HookStepAdvance(mainGO);
        }
    }

    private void DisableAllInteractionsAndHighlights()
    {
        Canvas rootCanvas = FindFirstObjectByType<Canvas>();
        if (!rootCanvas) return;

        foreach (Transform child in rootCanvas.GetComponentsInChildren<Transform>(true))
        {
            SetInteractable(child.gameObject, false);
            RemoveHighlight(child.gameObject);
        }
    }

    private void SetInteractable(GameObject obj, bool state)
    {
        if (!obj) return;

        var button = obj.GetComponent<Button>();
        if (button) button.interactable = state;

        var tmpDropdown = obj.GetComponent<TMPro.TMP_Dropdown>();
        if (tmpDropdown) tmpDropdown.interactable = state;

        var tmpInputField = obj.GetComponent<TMPro.TMP_InputField>();
        if (tmpInputField) tmpInputField.interactable = state;

        var challengeImage = obj.GetComponent<Onboarding_ChallengeImage>();
        if (challengeImage) challengeImage.SetInteractable(state);
    }

    private void AddHighlight(GameObject obj)
    {
        if (!obj) return;
        var outline = obj.GetComponent<Outline>();
        if (!outline) outline = obj.AddComponent<Outline>();
        outline.effectColor = Color.yellow;
        outline.effectDistance = new Vector2(5, 5);
        outline.enabled = true;
    }

    private void RemoveHighlight(GameObject obj)
    {
        if (!obj) return;
        var outline = obj.GetComponent<Outline>();
        if (outline) outline.enabled = false;
    }

    // private void HookStepAdvance(GameObject obj)
    // {
    //     if (!obj) return;

    //     var button = obj.GetComponent<Button>();
    //     if (button)
    //     {
    //         button.onClick.RemoveListener(GoToNextStep);
    //         button.onClick.AddListener(GoToNextStep);
    //     }

    //     var challengeImage = obj.GetComponent<Onboarding_ChallengeImage>();
    //     if (challengeImage)
    //     {
    //         challengeImage.OnInteracted -= GoToNextStep;
    //         challengeImage.OnInteracted += GoToNextStep;
    //     }
    // }

    private void HookStepAdvance(GameObject obj)
    {
        if (!obj) return;

        var button = obj.GetComponent<Button>();
        if (button)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                if (currentStepInteractables.Contains(obj))
                    GoToNextStep();
            });
        }

        var challengeImage = obj.GetComponent<Onboarding_ChallengeImage>();
        if (challengeImage)
        {
            challengeImage.OnInteracted -= OnChallengeImageInteracted;
            challengeImage.OnInteracted += OnChallengeImageInteracted;
        }
    }

    private void OnChallengeImageInteracted()
    {
        var sender = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (currentStepInteractables.Contains(sender)) GoToNextStep();
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

        PlayerPrefs.SetInt("TutorialFinished", 1);
        PlayerPrefs.Save();
        NavigationData.CameFromOnBoarding = true;

        StartCoroutine(DelayedLoadMenu());
    }

    private IEnumerator DelayedLoadMenu()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Menu");
    }
}
