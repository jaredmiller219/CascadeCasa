using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorySlideController : MonoBehaviour
{
    [Header("Slides")]
    public List<Sprite> slideImages;
    public List<AudioClip> slideMusic;
    [Header("End Behavior")]
    public GameObject storyRoot;
    public GameObject challengeUIToEnable;


    [Header("UI References")]
    public Image slideDisplay;
    public Button nextButton;
    public Button prevButton;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    [Header("Audio")]
    public AudioSource musicSource;

    private int currentSlide = 0;

    private void Start()
    {
        nextButton.onClick.AddListener(NextSlide);
        prevButton.onClick.AddListener(PrevSlide);

        ShowSlide(currentSlide);
    }

    private void ShowSlide(int index)
    {
        StartCoroutine(FadeInSlide(slideImages[index]));

        // Change music
        if (slideMusic != null && index < slideMusic.Count && slideMusic[index] != null)
        {
            musicSource.Stop();
            musicSource.clip = slideMusic[index];
            musicSource.Play();
        }

        // Button state
        prevButton.interactable = index > 0;
        nextButton.interactable = true;
        

    }
    private IEnumerator ExitStoryAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (storyRoot != null)
            storyRoot.SetActive(false);

        if (challengeUIToEnable != null)
            challengeUIToEnable.SetActive(true);
    }

    private IEnumerator FadeInSlide(Sprite newSlide)
    {
        slideDisplay.canvasRenderer.SetAlpha(0f);
        slideDisplay.sprite = newSlide;
        slideDisplay.SetNativeSize();
        slideDisplay.CrossFadeAlpha(1f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
    }
    private void ExitStoryMode()
    {
        if (storyRoot != null)
            storyRoot.SetActive(false);

        if (challengeUIToEnable != null)
            challengeUIToEnable.SetActive(true);
    }

    private void NextSlide()
    {
        if (currentSlide < slideImages.Count - 1)
        {
            currentSlide++;
            ShowSlide(currentSlide);
        }
        else
        {
            // We're at the last slide and the user just clicked "Next"
            ExitStoryMode();
        }
    }


    private void PrevSlide()
    {
        if (currentSlide > 0)
        {
            currentSlide--;
            ShowSlide(currentSlide);
        }
    }
}
