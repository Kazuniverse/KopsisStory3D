using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Quest UI References")]
    public GameObject questPanel;
    public bool show;
    public Text chapterTitle;
    public Text questText; // message
    public Text questTitle; // title
    public Text questCon; // condition
    private ControllerMode controllerMode;
    public GameObject enable;
    public GameObject disable;
    private Quest quest;

    [Header("Fade Settings")]
    public float fadeOutDuration = 0.5f;
    public float fadeInDuration = 0.2f;
    public bool isTransitioning = false;

    void Start()
    {
        questPanel.SetActive(false);
        SetTextAlpha(0); // Set initial alpha to 0
    }

    // Helper method to set alpha for the three text elements
    private void SetTextAlpha(float alpha)
    {
        SetTextAlpha(questTitle, alpha);
        SetTextAlpha(questText, alpha);
        SetTextAlpha(questCon, alpha);
    }

    // Helper method to set alpha for a single text element
    private void SetTextAlpha(Text textElement, float alpha)
    {
        if (textElement != null)
        {
            Color color = textElement.color;
            color.a = alpha;
            textElement.color = color;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && controllerMode == ControllerMode.PC && !isTransitioning)
            ToggleQuest();
    }

    public void ToggleQuest()
    {
        show = !show;

        if (show)
        {
            questPanel.SetActive(true);
            enable.SetActive(false);
            disable.SetActive(true);
        }
        else
        {
            HideQuestLog();
        }
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowQuestLog(string chapter, string title, string message, string condition)
    {
        if (isTransitioning)
        {
            StopAllCoroutines();
        }
        
        StartCoroutine(TransitionQuestLog(chapter, title, message, condition));
    }

    public IEnumerator TransitionQuestLog(string chapter, string title, string message, string condition, System.Action onAfterFadeOut = null)
    {
        isTransitioning = true;

        // Fade out current text
        if (questPanel.activeSelf)
        {
            yield return StartCoroutine(FadeOut());
        }

        // 🔥 Jalankan callback di titik ini — antara fade out dan fade in
        onAfterFadeOut?.Invoke();

        // Update content
        chapterTitle.text = chapter;
        questTitle.text = title;
        questText.text = message;
        questCon.text = condition;

        // Fade in new text
        questPanel.SetActive(true);
        yield return StartCoroutine(FadeIn());

        isTransitioning = false;
    }

    public void HideQuestLog()
    {
        questPanel.SetActive(false);
        enable.SetActive(true);
        disable.SetActive(false);
    }

    private IEnumerator FadeIn()
    {
        SetTextAlpha(0);
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeInDuration);
            SetTextAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        SetTextAlpha(1);
    }

    private IEnumerator FadeOut()
    {
        float startAlpha = questTitle.color.a; // Get current alpha from any of the texts
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeOutDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, 0, elapsedTime / fadeOutDuration);
            SetTextAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        SetTextAlpha(0);
    }
}