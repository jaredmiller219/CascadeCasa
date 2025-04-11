// using UnityEngine;

// public class HorizontalScrollBar : MonoBehaviour
// {
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }
// }

using UnityEngine;
using UnityEngine.UI;

public class HorizontalScrollBar : MonoBehaviour
{
    public RectTransform content;         // Assign in Inspector: Content of ScrollRect
    public Sprite[] imageSprites;         // Assign image sprites via Inspector
    public GameObject imagePrefab;        // A prefab with an Image component

    public void Start()
    {
        foreach (Sprite sprite in imageSprites)
        {
            GameObject imgObj = Instantiate(imagePrefab, content);
            Image img = imgObj.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = sprite;
            }
        }
    }
}
