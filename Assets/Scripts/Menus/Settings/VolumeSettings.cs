using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    /// <summary>
    /// The slider to change the volume
    /// </summary>
    [SerializeField]
    private Slider volumeSlider;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            LoadVolume();
        }
        else LoadVolume();
    }

    /// <summary>
    /// Change the volume and then set the new value
    /// </summary>
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveNewVolume();
    }

    /// <summary>
    /// Load the saved value of the volume
    /// </summary>
    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    /// <summary>
    /// Save the new volume
    /// </summary>
    private void SaveNewVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

}
