using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Kitchen_HorizontalScrollBar : MonoBehaviour
{
    // ---------------- Public Variables --------------------------

    /// <summary>
    /// the content area
    /// </summary>
    [Header("References")]
    public RectTransform content;

    /// <summary>
    /// The image prefab to be added
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
    /// An array of sprites to _____
    /// </summary>
    [Header("Images")]
    public Sprite[] imageSprites;

    /// <summary>
    /// A reference to the journal
    /// </summary>
    [HideInInspector]
    public Kitchen_Journal journal;

    /// <summary>
    /// A list of challenges for each image index
    /// </summary>
    [HideInInspector]
    public readonly List<KeyValuePair<string, string>> _cssChallenges = new()
    {
        new("div {\n    background color blue;\n    width: 100px;\n}", "div {\n    background-color: blue;\n    width: 100px;\n}"),
        new("p {\n    font size 20px;\n    text align center;\n}", "p {\n    font-size: 20px;\n    text-align: center;\n}"),
        new(".box {\n    border 2px solid black;\n    margin top 10px;\n}", ".box {\n    border: 2px solid black;\n    margin-top: 10px;\n}"),
        new("#header {\n    color red;\n    font weight bold;\n}", "#header {\n    color: red;\n    font-weight: bold;\n}"),
        new("ul {\n    list style type none;\n    padding 0;\n}", "ul {\n    list-style-type: none;\n    padding: 0;\n}"),
        new("a {\n    text decoration none;\n    color green;\n}", "a {\n    text-decoration: none;\n    color: green;\n}"),
        new("img {\n    width 100px;\n    height 100px;\n}", "img {\n    width: 100px;\n    height: 100px;\n}")
    };

    // --------------------------------------------------------------


    // ---------------- Private Variables ---------------------------

    /// <summary>
    /// a reference to the notepad script
    /// </summary>
    [SerializeField]
    private Kitchen_Notepad notepad;

    /// <summary>
    ///
    /// </summary>
    private int previousIndex = -1;

    /// <summary>
    /// The list of images in the scroll bar
    /// </summary>
    private readonly List<Image> _scrollImages = new();

    // --------------------------------------------------------------

    private void Start()
    {
        notepad = FindFirstObjectByType<Kitchen_Notepad>();
        if (notepad == null) { Debug.LogError("Notepad not found in scene!"); return; }

        journal = FindFirstObjectByType<Kitchen_Journal>();
        if (journal == null) Debug.Log("journal not initialized");

        Kitchen_ChallengeImage.OnAnyImageClicked -= notepad.SetCssText;
        Kitchen_ChallengeImage.OnAnyImageClicked += notepad.SetCssText;

        SetupLayout();
        StartCoroutine(DelayedLoad());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="clickedIndex"></param>
    /// <param name="css"></param>
    public void HandleImageClick(int clickedIndex, string css)
    {
        Kitchen_ChallengeImage clickedImage = GetImageAtIndex(clickedIndex);
        if (clickedImage != null) clickedImage.NotifyImageClicked(css);

        SetupNotepad(notepad, clickedIndex, true, true);

        if (IsSameButton(clickedIndex, previousIndex)) journal.ToggleJournal();
        else if (!IsJournalOpen(journal)) journal.ToggleJournal();

        previousIndex = clickedIndex;
    }

    /// <summary>
    /// Get the image at the given index and return it.
    /// </summary>
    /// <param name="index">The index of the image to find.</param>
    /// <returns>
    /// A <see cref="ChallengeImage"/> if the index is valid; otherwise, <c>null</c>
    /// if the image doesn't exist or index is out of range.
    /// </returns>
    public Kitchen_ChallengeImage GetImageAtIndex(int index)
    {
        if (index < 0 || index >= _scrollImages.Count)
        {
            Debug.LogError($"Index {index} out of range.");
            return null;
        }

        return _scrollImages[index].GetComponent<Kitchen_ChallengeImage>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="index"></param>
    public void MarkChallengeCompleted(int index)
    {
        if (index < 0 || index >= _scrollImages.Count)
        {
            Debug.LogWarning($"Invalid index {index} for marking challenge as complete.");
            return;
        }

        GameObject button = _scrollImages[index].gameObject;
        Transform Checkmark = button.transform.Find("Checkmark");
        Transform Lock = button.transform.Find("Lock");

        if (Checkmark != null && Lock != null)
        {
            Checkmark.gameObject.SetActive(true);
            Lock.gameObject.SetActive(false);
            if (button.TryGetComponent<Kitchen_ChallengeImage>(out var challengeImage))
            {
                challengeImage.Completed = true;
                challengeImage.Locked = false;
            }
        }
        else
        {
            Debug.LogWarning("Checkmark object not found under button!");
        }
    }

    /// <summary>
    /// wait for one frame until rebuild
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator DelayedLoad()
    {
        yield return null; // Wait one frame
        LoadImagesFromArray(); // Will just add images — no rebuilding inside

        // Now safe to rebuild here
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }

    /// <summary>
    /// Setup the layout of the scroll area
    /// </summary>
    private void SetupLayout()
    {
        if (content == null) return;

        if (!content.TryGetComponent<HorizontalLayoutGroup>(out var layoutGroup))
        {
            layoutGroup = content.gameObject.AddComponent<HorizontalLayoutGroup>();
        }

        layoutGroup.childAlignment = TextAnchor.MiddleLeft;
        layoutGroup.padding = new RectOffset(10, 10, 30, 10);
        layoutGroup.childControlWidth = false;
        layoutGroup.childControlHeight = false;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;

        if (!content.TryGetComponent<ContentSizeFitter>(out var fitter))
        {
            fitter = content.gameObject.AddComponent<ContentSizeFitter>();
        }

        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

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
        if (!content || !imagePrefab)
        {
            Debug.LogError("Missing content or imagePrefab.");
            return false;
        }
        return true;
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
    /// and attaches the related css script to the image
    /// </summary>
    private void AddScriptToImage(GameObject imgObj)
    {
        if (imgObj.TryGetComponent<LayoutElement>(out var layout)) DestroyImmediate(layout);
        var image = imgObj.AddComponent<Kitchen_ChallengeImage>();
        int index = (_scrollImages.Count - 1) % _cssChallenges.Count;
        image.AssociatedCss = _cssChallenges[index].Key;
    }

    /// <summary>
    /// Clear the scrollImages array and the images in the scroll bar
    /// </summary>
    private void ClearImages()
    {
        foreach (var img in _scrollImages.Where(i => i != null)) Destroy(img.gameObject);
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
        foreach (var img in _scrollImages)
        {
            if (img != null) img.GetComponent<RectTransform>().sizeDelta = imageSize * 2;
        }

        if (content)
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        }
    }

    /// <summary>
    /// Check if the button clicked is the same
    /// as the previous button that was clicked
    /// </summary>
    /// <param name="clickedIndex">the current index of the button clicked</param>
    /// <param name="previousIndex">the index of the previous button clicked</param>
    /// <returns>boolean representing whether same button was clicked or not</returns>
    private bool IsSameButton(int clickedIndex, int previousIndex)
    {
        if (clickedIndex == previousIndex) { return true; }
        else { return false; }
    }

    /// <summary>
    /// Checks if the journal is open.
    /// </summary>
    /// <param name="journal"></param>
    /// <returns>boolean representing if the journal was open or not</returns>
    private bool IsJournalOpen(Kitchen_Journal journal)
    {
        if (journal == null) Debug.LogError("journal is null");
        return journal.journalPopup.activeSelf;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="notepad">The notepad reference</param>
    /// <param name="clickedIndex">The index of the button clicked</param>
    /// <param name="canReset">Whether the notepad is able to reset</param>
    /// <param name="canSubmit">Whether the notepad is able to submit</param>
    private void SetupNotepad(Kitchen_Notepad notepad, int clickedIndex, bool canReset, bool canSubmit)
    {
        notepad.buttonindex = clickedIndex;
        notepad.canReset = canReset;
        notepad.canSubmit = canSubmit;
        notepad.LoadChallenge();
    }
}
