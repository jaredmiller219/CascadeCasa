using UnityEngine;
// using UnityEngine.UI;
using TMPro;

public class DropdownActionMenu : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    void Start()
    {
        dropdown.onValueChanged.AddListener(OnOptionSelected);
    }

    void OnOptionSelected(int index)
    {
        // Immediately reset so no option is visually "selected"
        dropdown.RefreshShownValue();

        // Do something based on what was clicked
        switch (index)
        {
            case 0:
                Debug.Log("Option 1 clicked");
                break;
            case 1:
                Debug.Log("Option 2 clicked");
                break;
            case 2:
                Debug.Log("Option 3 clicked");
                break;
        }
    }
}
