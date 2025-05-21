using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorySlideController : MonoBehaviour
{
    /// <summary>
    /// The images for the slides
    /// </summary>
    [Header("Slides")]
    public List<Sprite> slideImages;
    
    /// <summary>
    /// A list of audio tracks for the slides
    /// </summary>
    public List<AudioClip> slideMusic;
    
    /// <summary>
    /// The root GameObject of the story
    /// </summary>
    [Header("End Behavior")]
    public GameObject storyRoot;
    
    /// <summary>
    /// The challenge that needs to be enabled
    /// </summary>
    public GameObject challengeUIToEnable;

    /// <summary>
    /// The UI element used to display the current slide image
    /// </summary>
    [Header("UI References")] 
    public Image slideDisplay;

    /// <summary>
    /// The button used to navigate to the next slide
    /// </summary>
    public Button nextButton;

    /// <summary>
    /// The button used to navigate to the previous slide
    /// </summary>
    public Button prevButton;

    /// <summary>
    /// The duration of the fade effect in seconds
    /// </summary>
    [Header("Fade Settings")] 
    public float fadeDuration = 1f;

    /// <summary>
    /// The audio source responsible for playing music tracks during the slides
    /// </summary>
    [Header("Audio")] 
    public AudioSource musicSource;

    /// <summary>
    /// The index of the currently displayed slide
    /// </summary>
    private int currentSlide;

    private void Start()
    {
        nextButton.onClick.AddListener(NextSlide);
        prevButton.onClick.AddListener(PrevSlide);
        ShowSlide(currentSlide);
    }

    /// <summary>
    /// Displays a specific slide, updates the UI, and plays the corresponding audio if available.
    /// </summary>
    /// <param name="index">The index of the slide to display.</param>
    private void ShowSlide(int index)
    {
        StartCoroutine(FadeInSlide(slideImages[index]));

        if (slideMusic != null && index < slideMusic.Count && slideMusic[index])
        {
            musicSource.Stop();
            musicSource.clip = slideMusic[index];
            musicSource.Play();
        }

        prevButton.interactable = index > 0;
        nextButton.interactable = true;
    }

    /// <summary>
    /// Fades in the provided slide sprite by gradually increasing its transparency over a set duration.
    /// </summary>
    /// <param name="newSlide">The sprite to be displayed and faded in.</param>
    /// <returns>A coroutine that completes after the fade-in effect is finished.</returns>
    private IEnumerator FadeInSlide(Sprite newSlide)
    {
        slideDisplay.canvasRenderer.SetAlpha(0f);
        slideDisplay.sprite = newSlide;
        slideDisplay.SetNativeSize();
        slideDisplay.CrossFadeAlpha(1f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
    }

    /// <summary>
    /// Exits the story mode by hiding the story UI and enabling the challenge UI.
    /// </summary>
    private void ExitStoryMode()
    {
        if (storyRoot) storyRoot.SetActive(false);
        if (challengeUIToEnable) challengeUIToEnable.SetActive(true);
    }

    /// <summary>
    /// Advances to the next slide in the sequence.
    /// Updates the displayed content and plays corresponding media.
    /// If the last slide is reached, ends the story mode.
    /// </summary>
    private void NextSlide()
    {
        if (currentSlide < slideImages.Count - 1)
        {
            currentSlide++;
            ShowSlide(currentSlide);
        }
        else ExitStoryMode();
    }

    /// <summary>
    /// Navigates to the previous slide, updates the UI, and plays the associated audio if available.
    /// </summary>
    private void PrevSlide()
    {
        if (currentSlide <= 0) return;
        currentSlide--;
        ShowSlide(currentSlide);
    }
}
