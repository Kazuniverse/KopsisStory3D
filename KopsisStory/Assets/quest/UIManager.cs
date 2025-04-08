using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // Singleton instance

    [Header("Quest UI References")]
    public GameObject questPanel; // Referensi ke Quest Panel
    public bool show;
    public Text questText; 
    public Text questTitle; 
    public Text questCon; 
    private ControllerMode controllerMode;
    public GameObject enable;
    public GameObject disable;
    private Quest quest;

    void Start()
    {
        questPanel.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && controllerMode == ControllerMode.PC)
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
        if (!show)
        {
            questPanel.SetActive(false);
            enable.SetActive(true);
            disable.SetActive(false);
        }
    }

    public void Awake()
    {
        // Implementasi singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Jika Anda ingin UIManager bertahan di antara scene
        }
        else
        {
            Destroy(gameObject); // Hancurkan instance duplikat
        }
    }

    // Metode untuk menampilkan quest log
    public void ShowQuestLog(string title, string message, string condition)
    {
        Debug.Log($"UI Quest: {message}");

        // Aktifkan Quest Panel
        if (questPanel != null)
        {
            questPanel.SetActive(true);
        }

        // Set teks quest ke dalam UI Text
        if (questText != null && questTitle != null && questCon != null)
        {
            questTitle.text = title;
            questText.text = message;
            questCon.text = condition;
        }
    }

    public void HideQuestLog()
    {
        questPanel.SetActive(false);
    }
}
