using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages the onboarding tutorial flow, including the activation and deactivation
/// of tutorial steps, enabling interactable elements, and handling step navigation.
/// </summary>
public class OnBoardingManager : MonoBehaviour
{
    /// <summary>
    /// Holds a reference to the dropdown GameObject used within the onboarding tutorial.
    /// Used to display or manage dropdown elements during the tutorial process.
    /// </summary>
    [Tooltip("Dropdown GameObject reference")]
    [SerializeField]
    private GameObject dropdown;

    /// <summary>
    /// An array of GameObjects representing the sequence of tutorial steps
    /// to be displayed and managed during the onboarding process.
    /// </summary>
    [Tooltip("List of tutorial step GameObjects")]
    [SerializeField]
    private GameObject[] tutorialSteps;

    /// <summary>
    /// Stores a list of interactable steps, where each step contains its associated
    /// main interactable GameObjects used within the onboarding tutorial process.
    /// </summary>
    [Tooltip("List of interactable data per tutorial step")]
    [SerializeField]
    private List<InteractableStep> interactableItems = new();

    /// <summary>
    /// Stores the list of interactable GameObjects specific to the current tutorial step.
    /// </summary>
    private List<GameObject> currentStepInteractable = new();

    /// <summary>
    /// Tracks all interactable GameObjects accumulated across the tutorial steps.
    /// These GameObjects remain enabled and interactable as the user progresses through the steps.
    /// </summary>
    private HashSet<GameObject> cumulativeInteractable = new();

    /// <summary>
    /// Stores the index of the currently active step in the onboarding tutorial flow.
    /// Used to track progress and determine which tutorial step is displayed or interacted with.
    /// </summary>
    private int currentStep;

    private void Start()
    {
        if (dropdown) dropdown.SetActive(true);
        else Debug.LogWarning("Dropdown reference not set!");

        if (tutorialSteps.Length > 0) ShowStep(0);
        else Debug.LogWarning("No tutorial steps configured!");
    }

    /// <summary>
    /// Displays the specified tutorial step by updating its visibility and preparing it
    /// for interaction, if applicable.
    /// </summary>
    /// <param name="index">The index of the tutorial step to display and prepare.</param>
    private void ShowStep(int index)
    {
        if (!IsValidStepIndex(index)) return;
        if (!HasInteractableForStep(index)) return;

        UpdateStepVisibility(index);
        PrepareStep(index);
    }

    /// <summary>
    /// Validates whether the given index corresponds to a valid tutorial step
    /// within the range of the tutorialSteps array.
    /// </summary>
    /// <param name="index">The index of the tutorial step to validate.</param>
    /// <returns>True if the index is within the valid range of tutorialSteps; otherwise, false.</returns>
    private bool IsValidStepIndex(int index)
    {
        if (index >= 0 && index < tutorialSteps.Length) return true;

        Debug.LogError($"Tutorial step index {index} is out of range!");
        return false;
    }

    /// <summary>
    /// Updates the visibility of tutorial steps, ensuring that only the specified
    /// step is active while all others are deactivated.
    /// </summary>
    /// <param name="StepIndex">The index of the tutorial step to activate and display.</param>
    private void UpdateStepVisibility(int StepIndex)
    {
        bool IsStepActive(int indexToCheck) => indexToCheck == StepIndex;

        for (var localIndex = 0; localIndex < tutorialSteps.Length; localIndex++)
        {
            var currentStep = tutorialSteps[localIndex];
            var activeStatus = IsStepActive(localIndex);
            currentStep.SetActive(activeStatus);
        }
    }

    /// <summary>
    /// Determines whether the specified tutorial step has associated interactable data.
    /// </summary>
    /// <param name="index">The index of the tutorial step to check.</param>
    /// <returns>True if the specified step has interactable data; otherwise, false.</returns>
    private bool HasInteractableForStep(int index)
    {
        if (index < interactableItems.Count) return true;

        Debug.LogWarning($"No interactable data configured for step {index}");
        return false;
    }

    /// <summary>
    /// Prepares the specified tutorial step by enabling relevant interactable elements,
    /// adding highlights, and hooking up step advancement logic.
    /// </summary>
    /// <param name="index">The index of the tutorial step to prepare.</param>
    private void PrepareStep(int index)
    {
        DisableAllInteractionsAndHighlights();
        currentStepInteractable.Clear();
        currentStep = index;
        var step = interactableItems[index];

        // Add current step's interactables to the cumulative list
        foreach (var mainObject in step.mainInteractable)
        {
            if (!cumulativeInteractable.Contains(mainObject))
                cumulativeInteractable.Add(mainObject);

            currentStepInteractable.Add(mainObject);
            AddHighlight(mainObject);
            HookStepAdvance(mainObject);
        }

        // Re-enable all cumulative interactables
        foreach (var gameObject in cumulativeInteractable)
            SetInteractable(gameObject, true);
    }

    /// <summary>
    /// Disables all interactions and removes highlights for all elements within the root canvas hierarchy.
    /// </summary>
    private static void DisableAllInteractionsAndHighlights()
    {
        var rootCanvas = FindFirstObjectByType<Canvas>();
        if (!rootCanvas) return;

        static IEnumerable<Transform> GetAllChildTransforms(Canvas root) =>
            root.GetComponentsInChildren<Transform>(true);

        foreach (var child in GetAllChildTransforms(rootCanvas))
        {
            SetInteractable(child.gameObject, false);
            RemoveHighlight(child.gameObject);
        }
    }

    /// <summary>
    /// Sets the interactable state of the specified GameObject and applies the interaction state
    /// to applicable UI components attached to the GameObject, such as buttons, dropdowns,
    /// input fields, or custom components.
    /// </summary>
    /// <param name="obj">The GameObject whose interactable state needs to be updated.
    /// If null, no action is performed.</param>
    /// <param name="state">The desired interactable state to be applied.
    /// A value of true enables interaction, and a value of false disables it.</param>
    private static void SetInteractable(GameObject obj, bool state)
    {
        if (!obj) return;
        if (obj.TryGetComponent(out Button button)) button.interactable = state;
        if (obj.TryGetComponent(out TMP_Dropdown dropdown)) dropdown.interactable = state;
        if (obj.TryGetComponent(out TMP_InputField input)) input.interactable = state;
        if (obj.TryGetComponent(out Onboarding_ChallengeImage image)) image.SetInteractable(state);
    }

    /// <summary>
    /// Adds a visual highlight effect to the specified GameObject by applying or configuring an outline component.
    /// </summary>
    /// <param name="obj">The GameObject to which the highlight effect will be added.</param>
    private static void AddHighlight(GameObject obj)
    {
        if (!obj) return;
        var outline = GetOutline(obj);
        if (!outline) outline = AddOutline(obj);
        ConfigureOutline(outline, Color.yellow, new Vector2(5, 5), true);
    }

    /// <summary>
    /// Removes the highlight effect from the specified GameObject by disabling the associated Outline component,
    /// if present.
    /// </summary>
    /// <param name="obj">The GameObject from which the highlight effect will be removed.
    /// If null, no action is performed.</param>
    private static void RemoveHighlight(GameObject obj)
    {
        if (!obj) return;
        if (obj.TryGetComponent<Outline>(out var outline))
            outline.enabled = false;
    }

    /// <summary>
    /// Retrieves the Outline component from the specified GameObject, if it exists.
    /// </summary>
    /// <param name="obj">The GameObject from which to retrieve the Outline component.</param>
    /// <returns>The Outline component attached to the GameObject, or null if no Outline component is found or obj is null.</returns>
    private static Outline GetOutline(GameObject obj)
    {
        if (!obj) return null;
        return obj.GetComponent<Outline>();
    }

    /// <summary>
    /// Adds an Outline component to the specified GameObject to create a visual effect.
    /// </summary>
    /// <param name="obj">The GameObject to which the Outline component will be added.</param>
    /// <returns>The newly created Outline component attached to the GameObject, or null if obj is null.</returns>
    private static Outline AddOutline(GameObject obj)
    {
        if (!obj) return null;
        return obj.AddComponent<Outline>();
    }

    /// <summary>
    /// Configures the properties of a given Outline component, including its color,
    /// thickness, and enable state.
    /// </summary>
    /// <param name="outline">The Outline component to be configured.</param>
    /// <param name="outlineColor">The color of the outline effect.</param>
    /// <param name="outlineThickness">The thickness of the outline effect, defined as a vector.</param>
    /// <param name="enabled">A boolean value
    /// indicating whether the outline effect should be enabled or disabled.</param>
    private static void ConfigureOutline(Outline outline, Color outlineColor, Vector2 outlineThickness, bool enabled)
    {
        outline.effectColor = outlineColor;
        outline.effectDistance = outlineThickness;
        outline.enabled = enabled;
    }

    /// <summary>
    /// Hooks additional step-specific behaviors to the specified GameObject,
    /// such as button click events or challenge image interactions, enabling
    /// the onboarding functionality tied to the object.
    /// </summary>
    /// <param name="obj">The GameObject to which the step-specific behavior will be attached.</param>
    private void HookStepAdvance(GameObject obj)
    {
        if (!obj) return;
        HookButton(obj);
        HookChallengeImage(obj);
    }

    /// <summary>
    /// Configures the provided GameObject if it contains a Button component by setting up
    /// its click listener.
    /// The listener checks if the GameObject is part of the current step's
    /// interactable elements and triggers navigation to the next onboarding tutorial step.
    /// </summary>
    /// <param name="obj">The GameObject to be checked and configured for button interactions.</param>
    private void HookButton(GameObject obj)
    {
        var button = obj.GetComponent<Button>();
        if (!button) return;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            if (currentStepInteractable.Contains(obj)) GoToNextStep();
        });
    }

    /// <summary>
    /// Hooks the Onboarding_ChallengeImage component from the specified GameObject by
    /// assigning the OnInteracted event to handle user interactions with the component.
    /// </summary>
    /// <param name="obj">The GameObject containing the Onboarding_ChallengeImage component to be hooked.</param>
    private void HookChallengeImage(GameObject obj)
    {
        var challengeImage = obj.GetComponent<Onboarding_ChallengeImage>();
        if (!challengeImage) return;
        challengeImage.OnInteracted -= OnChallengeImageInteracted;
        challengeImage.OnInteracted += OnChallengeImageInteracted;
    }

    /// <summary>
    /// Handles the interaction event triggered by an Onboarding_ChallengeImage component,
    /// verifying if the currently selected GameObject is part of the interactable elements for the current step
    /// and progresses to the next step if valid.
    /// </summary>
    private void OnChallengeImageInteracted()
    {
        var gameObj = EventSystem.current;
        var currentObject = gameObj.currentSelectedGameObject;
        if (currentStepInteractable.Contains(currentObject)) GoToNextStep();
    }

    /// <summary>
    /// Advances the onboarding tutorial to the next step in the sequence if there are remaining steps.
    /// Activates the next tutorial step and updates the current step index.
    /// If there are no more steps, the tutorial ends.
    /// </summary>
    private void GoToNextStep()
    {
        // Checks if the step index is within the valid range of tutorial steps.
        bool IsValidStep(int step) => step < tutorialSteps.Length;

        currentStep++;
        if (IsValidStep(currentStep)) ShowStep(currentStep);
        else EndTutorial();
    }

    /// <summary>
    /// Ends the onboarding tutorial by deactivating all tutorial steps, setting a completion flag,
    /// and updating navigation data to indicate the tutorial has finished.
    /// Transitions to the main menu scene after a delay.
    /// </summary>
    private void EndTutorial()
    {
        foreach (var step in tutorialSteps) if (step) step.SetActive(false);
        PlayerPrefs.SetInt("TutorialFinished", 1);
        PlayerPrefs.Save();
        NavigationData.CameFromOnBoarding = true;
        StartCoroutine(DelayedLoadMenu());
    }

    /// <summary>
    /// Loads the main menu scene after a specified delay, allowing time for any necessary transitions or animations.
    /// </summary>
    /// <returns>Returns an IEnumerator used to handle the timed delay before transitioning to the main menu.</returns>
    private static IEnumerator DelayedLoadMenu()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Menu");
    }
}
