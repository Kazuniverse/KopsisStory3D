using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Slide
{
    public string title;
    [TextArea] public string content;
    public Sprite image;
}

[System.Serializable]
public class SlideCategory
{
    public string category;
    public Slide[] slides;
}

public class SlideManager : MonoBehaviour
{
    [Header("Slide Settings")]
    public SlideCategory[] category;
    [SerializeField] private int currentCategoryIndex = 0;
    [SerializeField] private int currentSlideIndex = 0;
    public Dropdown drop;
    public Scrollbar slider;
    public GameObject tutor;
    public GameObject acara;
    public GameObject hidden;

    [Header("UI References")]
    [SerializeField] private Text titleText;
    [SerializeField] private Text contentText;
    [SerializeField] private Image slideImage;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    private void Start()
    {
        nextButton.onClick.AddListener(NextSlide);
        prevButton.onClick.AddListener(PreviousSlide);
        tutor.SetActive(true);
        acara.SetActive(false);
        hidden.SetActive(false);
        UpdateSlide();
    }

    private void UpdateSlide()
    {
        titleText.text = category[currentCategoryIndex].slides[currentSlideIndex].title;
        contentText.text = category[currentCategoryIndex].slides[currentSlideIndex].content;
        slideImage.sprite = category[currentCategoryIndex].slides[currentSlideIndex].image;

        // Update button states
        prevButton.interactable = currentSlideIndex > 0;
        nextButton.interactable = currentSlideIndex < category[currentCategoryIndex].slides.Length - 1;
    }

    public void NextSlide()
    {
        currentSlideIndex = Mathf.Clamp(currentSlideIndex + 1, 0, category[currentCategoryIndex].slides.Length - 1);
        UpdateSlide();
    }

    public void PreviousSlide()
    {
        currentSlideIndex = Mathf.Clamp(currentSlideIndex - 1, 0, category[currentCategoryIndex].slides.Length - 1);
        UpdateSlide();
    }

    public void GoToSlide(int index)
    {
        currentSlideIndex = Mathf.Clamp(index, 0, category[currentCategoryIndex].slides.Length - 1);
        UpdateSlide();
    }

    public void ChangeCategory()
    {
        currentCategoryIndex = drop.value;
        currentSlideIndex = 0;
        slider.value = 1;

        if (currentCategoryIndex == 0)
        {
            tutor.SetActive(true);
            acara.SetActive(false);
            hidden.SetActive(false);
        }
        if (currentCategoryIndex == 1)
        {
            tutor.SetActive(false);
            acara.SetActive(true);
            hidden.SetActive(false);
        }
        if (currentCategoryIndex == 2)
        {
            tutor.SetActive(false);
            acara.SetActive(false);
            hidden.SetActive(true);
        }

        UpdateSlide();
    }
}