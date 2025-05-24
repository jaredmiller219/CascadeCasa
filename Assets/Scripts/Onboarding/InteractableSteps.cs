using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a single step in the tutorial containing the main interactable GameObjects for that step.
/// </summary>
[System.Serializable]
public class InteractableStep
{
    /// <summary>
    /// Stores the main interactable GameObjects associated with a specific step in the tutorial.
    /// </summary>
    [Tooltip("Main interactable GameObjects for this step")]
    public List<GameObject> mainInteractable = new();
}
