using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Defines types of interactable UI elements.
/// </summary>
public enum InteractableType
{
    /// <summary>Button UI element.</summary>
    Button,
    /// <summary>Dropdown UI element.</summary>
    Dropdown,
    /// <summary>Clickable image UI element.</summary>
    ClickableImage,
    /// <summary>Input field UI element.</summary>
    InputField,
    /// <summary>Generic interactable element.</summary>
    Generic
}

/// <summary>
/// Represents data for a tutorial step's interactable elements.
/// </summary>
[System.Serializable]
public class InteractableStep
{
    [Tooltip("Main interactable GameObjects for this step")]
    public List<GameObject> mainInteractables = new();

    [Tooltip("Allowed interactable types for this step")]
    public List<InteractableType> allowedTypes = new();
}

/// <summary>
/// Manages allowed interactable types during tutorial steps.
/// </summary>
public static class InteractionManager
{
    /// <summary>
    /// Set of allowed interactable types for the current step.
    /// </summary>
    private static HashSet<InteractableType> allowedTypesThisStep = new();

    /// <summary>
    /// Sets the allowed interactable types for the current tutorial step.
    /// </summary>
    /// <param name="allowed">Allowed interactable types.</param>
    public static void SetAllowedTypes(params InteractableType[] allowed)
    {
        allowedTypesThisStep = new HashSet<InteractableType>(allowed);
    }

    /// <summary>
    /// Checks if a given interactable type is allowed in the current step.
    /// </summary>
    /// <param name="type">Interactable type to check.</param>
    /// <returns>True if allowed, false otherwise.</returns>
    public static bool IsTypeAllowed(InteractableType type)
    {
        return allowedTypesThisStep.Contains(type);
    }
}

/// <summary>
/// Manages tutorial steps, enabling and highlighting interactable UI elements.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the Dropdown GameObject.
    /// </summary>
    [Tooltip("Dropdown GameObject reference")]
    [SerializeField]
    private GameObject dropdown;

    /// <summary>
    /// Array of tutorial step GameObjects.
    /// </summary>
    [Tooltip("List of tutorial step GameObjects")]
    [SerializeField]
    private GameObject[] tutorialSteps;

    /// <summary>
    /// List of interactable data for each tutorial step.
    /// </summary>
    [Tooltip("List of interactable data per tutorial step")]
    [SerializeField]
    private List<InteractableStep> interactableItems = new();


    // Keeps track of all interactables made available in previous steps
    private HashSet<GameObject> cumulativeInteractables = new();

    /// <summary>
    /// Index of the current tutorial step.
    /// </summary>
    private int currentStep;

    /// <summary>
    /// Called on start; initializes dropdown and shows the first tutorial step.
    /// </summary>
    private void Start()
    {
        if (dropdown) dropdown.SetActive(true);
        else Debug.LogWarning("Dropdown reference not set!");
        if (tutorialSteps.Length > 0) ShowStep(0);
        else Debug.LogWarning("No tutorial steps configured!");
    }

    /// <summary>
    /// Shows the tutorial step at the specified index.
    /// </summary>
    /// <param name="index">Index of the tutorial step to show.</param>
    private void ShowStep(int index)
    {
        if (index < 0 || index >= tutorialSteps.Length)
        {
            Debug.LogError($"Tutorial step index {index} is out of range!");
            return;
        }

        // Optional: keep all steps active, or toggle visuals only if needed
        for (int i = 0; i < tutorialSteps.Length; i++)
            tutorialSteps[i].SetActive(i == index);

        if (index >= interactableItems.Count)
        {
            Debug.LogWarning($"No interactable data configured for step {index}");
            return;
        }

        var step = interactableItems[index];

        // Update allowed types
        InteractionManager.SetAllowedTypes(step.allowedTypes.ToArray());

        // Disable all first
        DisableAllInteractionsAndHighlights();

        // Add new main interactables to cumulative list
        foreach (var mainGO in step.mainInteractables)
        {
            if (!cumulativeInteractables.Contains(mainGO))
                cumulativeInteractables.Add(mainGO);
        }

        // Enable interaction on all cumulative interactables
        foreach (var go in cumulativeInteractables)
        {
            SetInteractable(go, true);
        }

        // Highlight current step's main interactables
        foreach (var mainGO in step.mainInteractables)
        {
            AddHighlight(mainGO);
        }
    }


    /// <summary>
    /// Disables interaction and removes highlights from all UI elements under the root Canvas.
    /// </summary>
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

    /// <summary>
    /// Enables or disables interaction on a GameObject depending on the allowed types.
    /// </summary>
    /// <param name="obj">GameObject to modify.</param>
    /// <param name="state">True to enable interaction, false to disable.</param>
    private void SetInteractable(GameObject obj, bool state)
    {
        if (!obj) return;

        var button = obj.GetComponent<Button>();
        if (button && InteractionManager.IsTypeAllowed(InteractableType.Button))
            button.interactable = state;

        var tmpDropdown = obj.GetComponent<TMPro.TMP_Dropdown>();
        if (tmpDropdown && InteractionManager.IsTypeAllowed(InteractableType.Dropdown))
            tmpDropdown.interactable = state;

        var tmpInputField = obj.GetComponent<TMPro.TMP_InputField>();
        if (tmpInputField && InteractionManager.IsTypeAllowed(InteractableType.InputField))
            tmpInputField.interactable = state;

        var challengeImage = obj.GetComponent<Onboarding_ChallengeImage>();
        if (challengeImage && InteractionManager.IsTypeAllowed(InteractableType.ClickableImage))
        {
            challengeImage.SetInteractable(state);
        }
    }

    /// <summary>
    /// Adds a yellow outline highlight to the specified GameObject.
    /// </summary>
    /// <param name="obj">GameObject to highlight.</param>
    private void AddHighlight(GameObject obj)
    {
        if (!obj) return;
        var outline = obj.GetComponent<Outline>();
        if (!outline) outline = obj.AddComponent<Outline>();
        outline.effectColor = Color.yellow;
        outline.effectDistance = new Vector2(5, 5);
        outline.enabled = true;
    }

    /// <summary>
    /// Removes any outline highlight from the specified GameObject.
    /// </summary>
    /// <param name="obj">GameObject to remove highlight from.</param>
    private void RemoveHighlight(GameObject obj)
    {
        if (!obj) return;
        var outline = obj.GetComponent<Outline>();
        if (outline) outline.enabled = false;
    }

    /// <summary>
    /// Advances to the next tutorial step or ends the tutorial if all steps are completed.
    /// </summary>
    public void GoToNextStep()
    {
        currentStep++;
        if (currentStep < tutorialSteps.Length) ShowStep(currentStep);
        else EndTutorial();
    }

    /// <summary>
    /// Ends the tutorial by deactivating all steps, saving progress, and loading the main menu.
    /// </summary>
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

    /// <summary>
    /// Coroutine to delay loading the main menu scene by one second.
    /// </summary>
    /// <returns>IEnumerator for coroutine.</returns>
    private IEnumerator DelayedLoadMenu()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainMenu");
    }
}
