using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Bedroom1_HorizontalScrollBar : MonoBehaviour
{
    [Header("References")]
    public RectTransform content;
    public GameObject imagePrefab;

    [Header("Layout")]
    public float spacing;
    public Vector2 imageSize;

    [Header("Images")]
    public Sprite[] imageSprites;

    [SerializeField] private Bedroom1_Notepad notepad;

    private readonly List<Image> _scrollImages = new();

    private readonly List<KeyValuePair<string, string>> _cssChallenges = new()
    {
        new("div {\n    background color blue;\n    width: 100px;\n}", "div {\n    background-color: blue;\n    width: 100px;\n}"),
        new("p {\n    font size 20px;\n    text align center;\n}", "p {\n    font-size: 20px;\n    text-align: center;\n}"),
        new(".box {\n    border 2px solid black;\n    margin top 10px;\n}", ".box {\n    border: 2px solid black;\n    margin-top: 10px;\n}"),
        new("#header {\n    color red;\n    font weight bold;\n}", "#header {\n    color: red;\n    font-weight: bold;\n}"),
        new("ul {\n    list style type none;\n    padding 0;\n}", "ul {\n    list-style-type: none;\n    padding: 0;\n}"),
        new("a {\n    text decoration none;\n    color green;\n}", "a {\n    text-decoration: none;\n    color: green;\n}"),
        new("img {\n    width 100px;\n    height 100px;\n}", "img {\n    width: 100px;\n    height: 100px;\n}"),
    };

    private void Start()
    {
        if (notepad == null)
        {
            notepad = FindFirstObjectByType<Bedroom1Notepad>();
            if (notepad == null)
            {
                Debug.LogError("Notepad not found in scene!");
                return;
            }
        }

        Bedroom1_ChallengeImage.OnAnyImageClicked -= notepad.SetCssText;
        Bedroom1_ChallengeImage.OnAnyImageClicked += notepad.SetCssText;

        SetupLayout();
        LoadImagesFromArray();
    }

    private void SetupLayout()
    {
        if (content == null) return;

        var layoutGroup = content.GetComponent<HorizontalLayoutGroup>() ?? content.gameObject.AddComponent<HorizontalLayoutGroup>();
        layoutGroup.childAlignment = TextAnchor.MiddleLeft;
        layoutGroup.padding = new RectOffset(10, 10, 30, 10);
        layoutGroup.childControlWidth = false;
        layoutGroup.childControlHeight = false;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;

        var fitter = content.GetComponent<ContentSizeFitter>() ?? content.gameObject.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        UpdateLayout();
    }

    private void LoadImagesFromArray()
    {
        if (imageSprites == null) return;

        ClearImages();

        foreach (var sprite in imageSprites)
            AddImage(sprite);

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }

    private void AddImage(Sprite sprite)
    {
        if (!content || !imagePrefab)
        {
            Debug.LogError("Missing content or imagePrefab.");
            return;
        }

        var imgObj = Instantiate(imagePrefab, content);

        if (!imgObj.TryGetComponent<Image>(out var img)) return;

        img.sprite = sprite;
        img.preserveAspect = true;
        _scrollImages.Add(img);

        var rect = img.GetComponent<RectTransform>();
        rect.sizeDelta = imageSize * 2;

        if (imgObj.TryGetComponent<LayoutElement>(out var layout))
            DestroyImmediate(layout);

        var draggable = imgObj.AddComponent<Bedroom1_ChallengeImage>();
        int index = (_scrollImages.Count - 1) % _cssChallenges.Count;
        draggable.AssociatedCss = _cssChallenges[index].Key;
    }

    private void ClearImages()
    {
        foreach (var img in _scrollImages.Where(i => i != null))
            Destroy(img.gameObject);

        _scrollImages.Clear();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
            UpdateImageSizes();
    }

    private void UpdateImageSizes()
    {
        foreach (var img in _scrollImages)
        {
            if (img != null)
                img.GetComponent<RectTransform>().sizeDelta = imageSize * 2;
        }

        if (content)
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        }
    }

    private void UpdateLayout()
    {
        if (content == null) return;

        if (content.TryGetComponent<HorizontalLayoutGroup>(out var layoutGroup))
            layoutGroup.spacing = spacing;

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }

    public Bedroom1_ChallengeImage GetImageAtIndex(int index)
    {
        if (index < 0 || index >= _scrollImages.Count)
        {
            Debug.LogError($"Index {index} out of range.");
            return null;
        }

        return _scrollImages[index].GetComponent<Bedroom1_ChallengeImage>();
    }
}
