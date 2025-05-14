using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class pembawakegelapan : MonoBehaviour
{
    public bool gelap = false;
    public Image img;
    public Text textDisplay;

    [Header("Text Settings")]
    private string displayText;
    public Color textColor = Color.white;
    public TextAnchor textAlignment = TextAnchor.MiddleCenter;
    private float fadeDuration = 2f;
    public QuestManager quest;

    void Start()
    {
        if (img != null) img.enabled = false;

        if (textDisplay != null)
        {
            textDisplay.enabled = false;
            InitializeTextSettings();
        }
    }

    private void InitializeTextSettings()
    {
        if (textDisplay == null) return;

        textDisplay.text = "";
        textDisplay.color = new Color(textColor.r, textColor.g, textColor.b, 0);
        textDisplay.alignment = textAlignment;
    }

    public IEnumerator Penghitaman()
    {
        QuestInfo currentQuest = quest.GetCurrentQuest();

        if (currentQuest != null && currentQuest.activeNPCID == "D" && currentQuest.activeQuestName == "1")
        {
            yield return StartCoroutine(Hitam("Gelap..."));
        }
    }

    public IEnumerator Hitam(string textToDisplay)
    {
        gelap = true;

        if (img != null)
        {
            img.enabled = true;
            SetAlpha(img, 0f);
        }

        if (textDisplay != null)
        {
            textDisplay.enabled = true;
            textDisplay.text = textToDisplay;
            SetAlpha(textDisplay, 0f);
        }

        // Fade In
        yield return StartCoroutine(FadeAlpha(0f, 1f, fadeDuration));

        // Wait
        yield return new WaitForSeconds(1f);

        // Fade Out
        yield return StartCoroutine(FadeAlpha(1f, 0f, fadeDuration));

        // Hide again
        if (img != null) img.enabled = false;

        if (textDisplay != null)
        {
            textDisplay.enabled = false;
            textDisplay.text = "";
        }

        gelap = false;
    }

    private IEnumerator FadeAlpha(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            SetAlpha(img, alpha);
            SetAlpha(textDisplay, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetAlpha(img, to);
        SetAlpha(textDisplay, to);
    }

    private void SetAlpha(Graphic graphic, float alpha)
    {
        if (graphic == null) return;
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }
}
