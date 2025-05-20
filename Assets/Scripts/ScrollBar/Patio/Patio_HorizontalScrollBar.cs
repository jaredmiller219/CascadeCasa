using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Patio_HorizontalScrollBar : MonoBehaviour
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
    /// An array of sprites to add
    /// </summary>
    [Header("Images")]
    public Sprite[] imageSprites;

    /// <summary>
    /// A reference to the journal
    /// </summary>
    [HideInInspector]
    public Patio_Journal journal;

    /// <summary>
    /// A list of challenges for each image index
    /// </summary>
    [HideInInspector]
    public readonly List<KeyValuePair<string, string>> CssChallenges = new()
{
    new KeyValuePair<string, string>(
        ".card {\n    border radius 10px;\n}",
        ".card {\n    border-radius: 10px;\n}"
    ),
    new KeyValuePair<string, string>(
        ".btn {\n    transition all 0.3s ease;\n}",
        ".btn {\n    transition: all 0.3s ease;\n}"
    ),
    new KeyValuePair<string, string>(
        ".shadowbox {\n    box shadow 0px 4px 6px rgba(0, 0, 0, 0.1);\n}",
        ".shadowbox {\n    box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);\n}"
    ),
    new KeyValuePair<string, string>(
        "h2 {\n    text shadow 1px 1px 2px gray;\n}",
        "h2 {\n    text-shadow: 1px 1px 2px gray;\n}"
    ),
    new KeyValuePair<string, string>(
        ".pill {\n    background color #eee;\n    border radius 20px;\n}",
        ".pill {\n    background-color: #eee;\n    border-radius: 20px;\n}"
    ),
    new KeyValuePair<string, string>(
        ".image:hover {\n    box shadow 0px 8px 12px rgba(0, 0, 0, 0.2);\n}",
        ".image:hover {\n    box-shadow: 0px 8px 12px rgba(0, 0, 0, 0.2);\n}"
    ),
    new KeyValuePair<string, string>(
        ".hovercard {\n    transition all 0.2s ease;\n    border radius 6px;\n}",
        ".hovercard {\n    transition: all 0.2s ease;\n    border-radius: 6px;\n}"
    ),
    new KeyValuePair<string, string>(
        "button {\n    background color white;\n    box shadow 2px 2px 5px rgba(0, 0, 0, 0.1);\n}",
        "button {\n    background-color: white;\n    box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.1);\n}"
    ),
    new KeyValuePair<string, string>(
        ".title:hover {\n    text shadow 2px 2px 5px #999;\n    color #222;\n}",
        ".title:hover {\n    text-shadow: 2px 2px 5px #999;\n    color: #222;\n}"
    ),
    new KeyValuePair<string, string>(
        ".final {\n    padding 20px;\n    border radius 12px;\n    background color #f8f8f8;\n}",
        ".final {\n    padding: 20px;\n    border-radius: 12px;\n    background-color: #f8f8f8;\n}"
    )
};


    // --------------------------------------------------------------


    // ---------------- Private Variables ---------------------------

    /// <summary>
    /// a reference to the notepad script
    /// </summary>
    [SerializeField]
    private Patio_Notepad notepad;

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
        notepad = FindFirstObjectByType<Patio_Notepad>();
        if (!notepad) Debug.LogError("Notepad not found in scene!");

        journal = FindFirstObjectByType<Patio_Journal>();
        if (!journal) Debug.Log("journal not initialized");

        Patio_ChallengeImage.OnAnyImageClicked -= notepad.SetCssText;
        Patio_ChallengeImage.OnAnyImageClicked += notepad.SetCssText;

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
        var clickedImage = GetImageAtIndex(clickedIndex);
        if (clickedImage) Patio_ChallengeImage.NotifyImageClicked(css);
        SetupNotepad(notepad, clickedIndex, true, true);
        if (IsSameButton(clickedIndex, previousIndex) || !IsJournalOpen(journal)) journal.ToggleJournal();
        previousIndex = clickedIndex;
    }

    /// <summary>
    /// Get the image at the given index and return it.
    /// </summary>
    /// <param name="index">The index of the image to find.</param>
    /// <returns>
    /// A <see cref="Patio_ChallengeImage"/> if the index is valid; otherwise, <c>null</c>
    /// if the image doesn't exist or index is out of range.
    /// </returns>
    private Patio_ChallengeImage GetImageAtIndex(int index)
    {
        if (index >= 0 && index < _scrollImages.Count) return _scrollImages[index].GetComponent<Patio_ChallengeImage>();
        Debug.LogError($"Index {index} out of range.");
        return null;

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

        var button = _scrollImages[index].gameObject;
        var checkmark = button.transform.Find("Checkmark");
        var lockIcon = button.transform.Find("Lock");

        if (checkmark && lockIcon)
        {
            checkmark.gameObject.SetActive(true);
            lockIcon.gameObject.SetActive(false);
            if (!button.TryGetComponent<Patio_ChallengeImage>(out var challengeImage)) return;
            challengeImage.Completed = true;
            challengeImage.Locked = false;
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
        if (!content) return;

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
    /// and attaches the related css script to the image
    /// </summary>
    private void AddScriptToImage(GameObject imgObj)
    {
        if (imgObj.TryGetComponent<LayoutElement>(out var layout)) DestroyImmediate(layout);
        var image = imgObj.AddComponent<Patio_ChallengeImage>();
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
    /// <returns>boolean representing whether same button was clicked or not</returns>
    private static bool IsSameButton(int clickedIndex, int previousIndex)
    {
        return clickedIndex == previousIndex;
    }

    /// <summary>
    /// Checks if the journal is open.
    /// </summary>
    /// <param name="journal"></param>
    /// <returns>boolean representing if the journal was open or not</returns>
    private static bool IsJournalOpen(Patio_Journal journal)
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
    private static void SetupNotepad(Patio_Notepad notepad, int clickedIndex, bool canReset, bool canSubmit)
    {
        notepad.buttonIndex = clickedIndex;
        notepad.canReset = canReset;
        notepad.canSubmit = canSubmit;
        notepad.LoadChallenge();
    }

    private void OnDestroy()
    {
        if (notepad) Patio_ChallengeImage.OnAnyImageClicked -= notepad.SetCssText;
    }
}
