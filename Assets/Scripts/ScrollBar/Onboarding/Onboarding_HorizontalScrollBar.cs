using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a horizontal scroll bar component designed for the onboarding system.
/// Provides functionality to display and interact with a dynamically generated list of image challenges.
/// </summary>
public class Onboarding_HorizontalScrollBar : MonoBehaviour
{
    /// <summary>
    /// the content area
    /// </summary>
    /// 
    [Header("Unlock Display")]
    public RectTransform unlockedImagePanel;

    /// <summary>
    /// The container for dynamically added or modified UI elements,
    /// mainly used to manage the layout of child components in the horizontal scroll bar.
    /// </summary>
    [Header("References")] public RectTransform content;

    /// <summary>
    /// The image prefabs to be added
    /// </summary>
    [Header("References")]
    public GameObject imagePrefab;

    /// <summary>
    /// the spacing between each image (uniform)
    /// </summary>
    [Header("Layout")]
    public float spacing;

    /// <summary>
    /// The size of the images
    /// </summary>
    [Header("Layout")]
    public Vector2 imageSize;

    /// <summary>
    /// An array of sprites to add
    /// </summary>
    [Header("Images")]
    public Sprite[] imageSprites;

    /// <summary>
    /// A reference to the journal
    /// </summary>
    [HideInInspector]
    public Onboarding_Journal journal;

    /// <summary>
    /// A list of challenges for each image index
    /// </summary>
    // Also Hidden in inspector
    public readonly List<KeyValuePair<string, string>> CssChallenges = new()
    {
        new KeyValuePair<string, string>("div {\n    background color blue;\n    width: 100px;\n}", "div {\n    background-color: blue;\n    width: 100px;\n}"),
        new KeyValuePair<string, string>("p {\n    font size 20px;\n    text align center;\n}", "p {\n    font-size: 20px;\n    text-align: center;\n}"),
        new KeyValuePair<string, string>(".box {\n    border 2px solid black;\n    margin top 10px;\n}", ".box {\n    border: 2px solid black;\n    margin-top: 10px;\n}"),
        new KeyValuePair<string, string>("#header {\n    color red;\n    font weight bold;\n}", "#header {\n    color: red;\n    font-weight: bold;\n}"),
        new KeyValuePair<string, string>("ul {\n    list style type none;\n    padding 0;\n}", "ul {\n    list-style-type: none;\n    padding: 0;\n}"),
        new KeyValuePair<string, string>("a {\n    text decoration none;\n    color green;\n}", "a {\n    text-decoration: none;\n    color: green;\n}"),
        new KeyValuePair<string, string>("img {\n    width 100px;\n    height 100px;\n}", "img {\n    width: 100px;\n    height: 100px;\n}")
    };

    /// <summary>
    /// a reference to the notepad script
    /// </summary>
    [SerializeField]
    private Onboarding_Notepad notepad;

    /// <summary>
    /// Tracks the previously clicked image index in the onboarding process.
    /// Used to determine if the same image button has been clicked consecutively.
    /// </summary>
    private int previousIndex = -1;

    /// <summary>
    /// The list of images in the scroll bar
    /// </summary>
    private readonly List<Image> _scrollImages = new();

    private void Start()
    {
        notepad = FindFirstObjectByType<Onboarding_Notepad>();
        if (!notepad) Debug.LogError("Notepad not found in scene!");

        journal = FindFirstObjectByType<Onboarding_Journal>();
        if (!journal) Debug.Log("journal not initialized");

        Onboarding_ChallengeImage.OnAnyImageClicked -= notepad.SetCssText;
        Onboarding_ChallengeImage.OnAnyImageClicked += notepad.SetCssText;

        SetupLayout();
        StartCoroutine(DelayedLoad());
    }

    /// <summary>
    /// Handles the click event for an image, performing related updates and actions.
    /// </summary>
    /// <param name="clickedIndex">The index of the clicked image.</param>
    /// <param name="css">The CSS class or identifier associated with the image.</param>
    public void HandleImageClick(int clickedIndex, string css)
    {
        var clickedImage = GetImageAtIndex(clickedIndex);
        if (clickedImage) Onboarding_ChallengeImage.NotifyImageClicked(css);
        SetupNotepad(notepad, clickedIndex, true, true);
        if (IsSameButton(clickedIndex, previousIndex) || !IsJournalOpen(journal)) journal.ToggleJournal();
        previousIndex = clickedIndex;
    }

    /// <summary>
    /// Get the image at the given index and return it.
    /// </summary>
    /// <param name="index">The index of the image to find.</param>
    /// <returns>
    /// A <see cref="Onboarding_ChallengeImage"/> if the index is valid; otherwise, <c>null</c>
    /// if the image doesn't exist or index is out of range.
    /// </returns>
    private Onboarding_ChallengeImage GetImageAtIndex(int index)
    {
        if (index >= 0 && index < _scrollImages.Count) return _scrollImages[index].GetComponent<Onboarding_ChallengeImage>();
        Debug.LogError($"Index {index} out of range.");
        return null;

    }

    /// <summary>
    /// Marks the challenge at the specified index as completed by updating its visual state and properties.
    /// </summary>
    /// <param name="index">The index of the challenge to mark as completed.</param>
    public void MarkChallengeCompleted(int index)
    {
        if (index < 0 || index >= _scrollImages.Count)
        {
            Debug.LogWarning($"Invalid index {index} for marking challenge as complete.");
            return;
        }

        var button = _scrollImages[index].gameObject;
        var checkmark = button.transform.Find("Checkmark");
        var lockIcon = button.transform.Find("Lock");

        if (checkmark && lockIcon && button.TryGetComponent<Onboarding_ChallengeImage>(out var challengeImage))
        {
            checkmark.gameObject.SetActive(true);
            lockIcon.gameObject.SetActive(false);

            challengeImage.Completed = true;
            challengeImage.Locked = false;

            AddUnlockedImage(index);
        }
        else Debug.LogWarning("Checkmark object not found under button!");
    }

    /// <summary>
    /// Adds an unlocked image to the unlocked image panel based on the provided index.
    /// </summary>
    /// <param name="index">The index of the image in the imageSprites array to be added.</param>
    private void AddUnlockedImage(int index)
    {
        if (!unlockedImagePanel)
        {
            Debug.LogWarning("Unlocked image panel is not assigned!");
            return;
        }

        if (index < 0 || index >= imageSprites.Length)
        {
            Debug.LogWarning("Index out of range for imageSprites.");
            return;
        }

        var newImageObj = Instantiate(imagePrefab, unlockedImagePanel);
        if (!newImageObj.TryGetComponent<Image>(out var newImage)) return;

        newImage.sprite = imageSprites[index];
        newImage.preserveAspect = true;
        newImage.GetComponent<RectTransform>().sizeDelta = imageSize;
        
        var challengeComponent = newImageObj.GetComponent<Onboarding_ChallengeImage>();
        if (challengeComponent) Destroy(challengeComponent);
    }

    /// <summary>
    /// wait for one frame until rebuild
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator DelayedLoad()
    {
        yield return null;
        LoadImagesFromArray();
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }

    /// <summary>
    /// Set up the layout of the scroll area
    /// </summary>
    private void SetupLayout()
    {
        if (!content) return;
        var layoutGroup = GetOrAddComponent<HorizontalLayoutGroup>(content.gameObject);
        
        SetLayoutGroupOptions(layoutGroup, false, false, 
            false, false, TextAnchor.MiddleLeft, 
            new RectOffset(10, 10, 30, 10));
        
        var ContentSizeFitter = GetOrAddComponent<ContentSizeFitter>(content.gameObject);
        SetContentFitter(ContentSizeFitter, ContentSizeFitter.FitMode.PreferredSize);
    }

    /// <summary>
    /// Configures the layout group settings of a HorizontalLayoutGroup component.
    /// </summary>
    /// <param name="layout">The HorizontalLayoutGroup component to be configured.</param>
    /// <param name="controlWidth">Indicates whether the layout group should control the width of its child elements.</param>
    /// <param name="controlHeight">Indicates whether the layout group should control the height of its child elements.</param>
    /// <param name="expandWidth">Indicates whether the layout group should force its child elements
    /// to expand their width.</param>
    /// <param name="expandHeight">Indicates whether the layout group should force its child elements
    /// to expand their height.</param>
    /// <param name="childAlignment">The alignment of the child elements.</param>
    /// <param name="padding">The padding of the layout group.</param>
    private static void SetLayoutGroupOptions(HorizontalLayoutGroup layout, bool controlWidth, bool controlHeight,
        bool expandWidth, bool expandHeight, TextAnchor childAlignment, RectOffset padding)
    {
        layout.childControlWidth = controlWidth;
        layout.childControlHeight = controlHeight;
        layout.childForceExpandWidth = expandWidth;
        layout.childForceExpandHeight = expandHeight;
        layout.childAlignment = childAlignment;
        layout.padding = padding;
    }

    /// <summary>
    /// Configures the given <see cref="ContentSizeFitter"/> to use the specified fit mode
    /// for both horizontal and vertical layout fitting.
    /// </summary>
    /// <param name="fitter">The <see cref="ContentSizeFitter"/> component to be configured.</param>
    /// <param name="fitMode">The desired
    /// <see cref="ContentSizeFitter.FitMode"/> to apply to horizontal and vertical fitting.</param>
    private static void SetContentFitter(ContentSizeFitter fitter, ContentSizeFitter.FitMode fitMode)
    {
        fitter.horizontalFit = fitMode;
        fitter.verticalFit = fitMode;
    }

    /// <summary>
    /// Retrieves the specified component of type <typeparamref name="T"/> from the given GameObject.
    /// If the component does not exist, it adds a new one and returns it.
    /// </summary>
    /// <param name="obj">The GameObject to search for the component.</param>
    /// <typeparam name="T">The type of the component to retrieve or add.</typeparam>
    /// <returns>
    /// The existing component of type <typeparamref name="T"/> if found;
    /// otherwise, a newly added component of type <typeparamref name="T"/>.
    /// </returns>
    private T GetOrAddComponent<T>(GameObject obj) where T : Component =>
        obj.TryGetComponent(out T comp) ? comp : obj.AddComponent<T>();

    /// <summary>
    /// Load the images from the image sprites array
    /// </summary>
    private void LoadImagesFromArray()
    {
        if (imageSprites == null) return;
        ClearImages();
        foreach (var sprite in imageSprites) AddImage(sprite);
    }

    /// <summary>
    /// Add the sprite to the scroll images list as an image.
    /// </summary>
    /// <param name="sprite">The sprite to add.</param>
    private void AddImage(Sprite sprite)
    {
        if (!ValidateReferences()) return;

        var imgObj = Instantiate(imagePrefab, content);
        if (!imgObj.TryGetComponent<Image>(out var img)) return;

        ApplyImageProperties(img, sprite);
        AddScriptToImage(imgObj);
    }

    /// <summary>
    /// Checks if the content and imagePrefab are valid
    /// </summary>
    /// <returns>A boolean representing if it was able to find the references</returns>
    private bool ValidateReferences()
    {
        if (content && imagePrefab) return true;
        Debug.LogError("Missing content or imagePrefab.");
        return false;
    }

    /// <summary>
    /// Applies visual properties to the image: sprite, aspect ratio, size, and tracking.
    /// </summary>
    private void ApplyImageProperties(Image img, Sprite sprite)
    {
        var rect = img.GetComponent<RectTransform>();
        img.sprite = sprite;
        img.preserveAspect = true;
        _scrollImages.Add(img);
        rect.sizeDelta = imageSize * 2;
    }

    /// <summary>
    /// Prepares the image GameObject for interaction
    /// and attaches the related CSS script to the image
    /// </summary>
    private void AddScriptToImage(GameObject imgObj)
    {
        if (imgObj.TryGetComponent<LayoutElement>(out var layout)) DestroyImmediate(layout);
        var image = imgObj.AddComponent<Onboarding_ChallengeImage>();
        var index = (_scrollImages.Count - 1) % CssChallenges.Count;
        image.AssociatedCss = CssChallenges[index].Key;
    }

    /// <summary>
    /// Clear the scrollImages array and the images in the scroll bar
    /// </summary>
    private void ClearImages()
    {
        foreach (var img in _scrollImages.Where(i => i)) Destroy(img.gameObject);
        _scrollImages.Clear();
    }

    /// <summary>
    /// Call update image sizes if the app is currently playing
    /// </summary>
    private void OnValidate()
    {
        if (Application.isPlaying) UpdateImageSizes();
    }

    /// <summary>
    /// Update the image sizes
    /// </summary>
    private void UpdateImageSizes()
    {
        foreach (var img in _scrollImages.Where(img => img))
        {
            img.GetComponent<RectTransform>().sizeDelta = imageSize * 2;
        }

        if (!content) return;
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }

    /// <summary>
    /// Check if the button clicked is the same
    /// as the previous button that was clicked
    /// </summary>
    /// <param name="clickedIndex">the current index of the button clicked</param>
    /// <param name="previousIndex">the index of the previous button clicked</param>
    /// <returns>boolean representing whether the same button was clicked or not</returns>
    private static bool IsSameButton(int clickedIndex, int previousIndex) => clickedIndex == previousIndex;

    /// <summary>
    /// Checks if the journal is open.
    /// </summary>
    /// <param name="journal"></param>
    /// <returns>boolean representing if the journal was open or not</returns>
    private static bool IsJournalOpen(Onboarding_Journal journal)
    {
        if (!journal) Debug.LogError("journal is null");
        return journal.journalPopup.activeSelf;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="notepad">The notepad reference</param>
    /// <param name="clickedIndex">The index of the button clicked</param>
    /// <param name="canReset">Whether the notepad is able to reset</param>
    /// <param name="canSubmit">Whether the notepad is able to submit</param>
    private static void SetupNotepad(Onboarding_Notepad notepad, int clickedIndex, bool canReset, bool canSubmit)
    {
        notepad.buttonIndex = clickedIndex;
        notepad.canReset = canReset;
        notepad.canSubmit = canSubmit;
        notepad.LoadChallenge();
    }

    private void OnDestroy()
    {
        if (notepad) Onboarding_ChallengeImage.OnAnyImageClicked -= notepad.SetCssText;
    }
}
