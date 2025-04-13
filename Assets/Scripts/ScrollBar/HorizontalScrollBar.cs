using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ScrollBar
{
    public class HorizontalScrollBar : MonoBehaviour
    {
        [Header("References")]
        public RectTransform content;
        public GameObject imagePrefab;

        [Header("Layout")]
        [Tooltip("Space between images in pixels")]
        public float spacing = 20f;
        [Tooltip("Size of each image in pixels")]
        public Vector2 imageSize = new(200f, 200f);

        [Header("Images")]
        public Sprite[] imageSprites;
        private readonly List<Image> _scrollImages = new();

        private void Start()
        {
            SetupLayout();
            LoadImagesFromArray();
        }

        private void SetupLayout()
        {
            if (content == null) return;

            if (!content.TryGetComponent<HorizontalLayoutGroup>(out var layoutGroup))
            {
                layoutGroup = content.gameObject.AddComponent<HorizontalLayoutGroup>();
            }

            // Update layout group settings
            layoutGroup.spacing = spacing;
            layoutGroup.childAlignment = TextAnchor.MiddleLeft;
            layoutGroup.padding = new RectOffset(10, 10, 10, 10);
            layoutGroup.childControlWidth = false;
            layoutGroup.childControlHeight = false;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childForceExpandHeight = false;

            // Ensure content size fitter is present
            if (!content.TryGetComponent<ContentSizeFitter>(out var contentSizeFitter))
            {
                contentSizeFitter = content.gameObject.AddComponent<ContentSizeFitter>();
            }
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        private void LoadImagesFromArray()
        {
            if (imageSprites == null) return;
            ClearImages(); // Clear existing images first

            foreach (Sprite sprite in imageSprites)
            {
                AddImage(sprite);
            }

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        }

        private void AddImage(Sprite sprite)
        {
            if (content == null || imagePrefab == null)
            {
                Debug.LogError("Content or image prefab not assigned!");
                return;
            }

            GameObject imgObj = Instantiate(imagePrefab, content);
            if (imgObj.TryGetComponent<Image>(out var img))
            {
                img.sprite = sprite;
                img.preserveAspect = true;
                _scrollImages.Add(img);

                if (!imgObj.TryGetComponent<LayoutElement>(out var layoutElement))
                    layoutElement = imgObj.AddComponent<LayoutElement>();

                layoutElement.preferredWidth = imageSize.x;
                layoutElement.preferredHeight = imageSize.y;

                imgObj.AddComponent<DraggableImage>();
            }
        }

        private void ClearImages()
        {
            foreach (var img in _scrollImages.Where(img => img != null))
            {
                Destroy(img.gameObject);
            }
            _scrollImages.Clear();
        }
    }
}
