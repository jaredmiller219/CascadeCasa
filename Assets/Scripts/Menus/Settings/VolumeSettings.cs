using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            LoadVolume();
        }
        else LoadVolume();
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveNewVolume();
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void SaveNewVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

}
