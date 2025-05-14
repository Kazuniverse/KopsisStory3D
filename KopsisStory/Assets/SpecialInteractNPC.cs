using System.Collections;
using cherrydev;
using UnityEngine;
using UnityEngine.UI;

public class SpecialInteractNPC : MonoBehaviour
{
    [Header("Dialog System")]
    public DialogBehaviour dialogBehaviour;
    public DialogNodeGraph dialogGraphs;

    [Header("Camera Settings")]
    public GameObject cam1;
    public GameObject cam2;

    [Header("Fade UI")]
    public Image img;
    private float fadeDuration = 2f;

    [Header("System References")]
    public GameObject[] canvas;
    public Movement playerMovement;
    public GameObject gManager;

    [Header("Animation")]
    public Animator animate;
    private bool conversation = false;
    public tabrakafterA2 tabrak;
    public NPCLocation locate;
    void Start()
    {
        img.enabled = false;
    }

    private void OnEnable()
    {
        if (dialogBehaviour != null)
        {
            dialogBehaviour.OnDialogEnded += OnConversationEnded;
        }
    }

    private void OnDisable()
    {
        if (dialogBehaviour != null)
        {
            dialogBehaviour.OnDialogEnded -= OnConversationEnded;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !conversation)
        {
            conversation = true;

            // Nonaktifkan movement & simpan posisi kamera
            playerMovement.enabled = false;

            StartCoroutine(StartConversationSequence());
        }
    }

    private IEnumerator StartConversationSequence()
    {
        // Fade to black
        img.enabled = true;
        yield return Fade(0, 1);

        cam1.SetActive(false);
        cam2.SetActive(true);

        // Nonaktifkan canvas dan pause manager
        foreach (GameObject isi in canvas)
        {
            isi.SetActive(false);
        }

        if (gManager != null)
        {
            gManager.GetComponent<PauseManager>().enabled = false;
        }

        // Set animasi idle saat dialog
        if (animate != null)
        {
            animate.SetFloat("walk", 0f);
            animate.SetBool("fall", false);
        }

        // Fade back to game view
        yield return Fade(1, 0);

        // Tampilkan cursor dan mulai dialog
        ShowCursor(true);
        dialogBehaviour.SetCurrentDialogOwner(this);
        dialogBehaviour.StartDialog(dialogGraphs);
        Debug.Log("Special interaction with NPC triggered!");
    }

    private void OnConversationEnded()
    {
        if (dialogBehaviour.CurrentDialogOwner != this) return;

        Debug.Log("Conversation ended!");
        StartCoroutine(EndConversationSequence());
    }

    private IEnumerator EndConversationSequence()
    {
        // Fade to black
        img.enabled = true;
        yield return Fade(0, 1);

        cam1.SetActive(true);
        cam2.SetActive(false);

        // Aktifkan kembali HUD dan pause manager
        foreach (GameObject isi in canvas)
        {
            isi.SetActive(true);
        }

        if (gManager != null)
        {
            gManager.GetComponent<PauseManager>().enabled = true;
        }

        // Kunci kembali cursor
        ShowCursor(false);

        // Fade out
        yield return Fade(1, 0);

        playerMovement.enabled = true;
        conversation = false;
        tabrak.Penghancuran();
        locate.Pindah6();
        
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0;
        Color color = img.color;

        while (elapsed < fadeDuration)
        {
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            img.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        img.color = color;

        if (endAlpha == 0)
        {
            img.enabled = false;
        }
    }

    private void ShowCursor(bool show)
    {
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }
}
