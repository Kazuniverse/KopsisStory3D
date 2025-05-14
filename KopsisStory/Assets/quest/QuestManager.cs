using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestInfo
{
    public string chapter;
    public string title;
    [TextArea] public string questDescription;
    [SerializeField] [TextArea] public string completionMessage;
    public string activeNPCID; // NPC ID yang terkait dengan quest ini
    public string activeQuestName; // Nama quest yang terkait dengan NPC
    public bool isQuestActive;
    public bool isQuestCompleted;
    public bool isAfterQuest;
    public GameObject questTrig;
}

public class QuestManager : MonoBehaviour
{
    private string NAMA = "Gilang";
    public Color inProgress;
    public Color completeColor;
    public Text message;
    public Text condition;

    public QuestInfo[] quests; // Array untuk menyimpan urutan quest
    private int currentQuestIndex = 0; // Indeks quest yang sedang aktif

    private UIManager ui;

    [Header ("Quest Trigger")]
    public GameObject quest5;
    public GameObject quest6;

    string ProsesTemplate(string template, Dictionary<string, string> data)
    {
        foreach (var pair in data)
        {
            template = template.Replace("{" + pair.Key + "}", pair.Value);
        }
        return template;
    }

    void Start()
    {
        var data = new Dictionary<string, string>() {
            {"nama", NAMA}
        };

        QuestInfo currentQuest = quests[currentQuestIndex];
        string chapterSekarang = ProsesTemplate(currentQuest.chapter, data);
        string titleSekarang = ProsesTemplate(currentQuest.title, data);
        string descriptionSekarang = ProsesTemplate(currentQuest.questDescription, data);
        string completionSekarang = ProsesTemplate(currentQuest.completionMessage, data);
        UIManager.Instance.ShowQuestLog(chapterSekarang, titleSekarang, descriptionSekarang, "In Progress");
    }

    void Update ()
    {
        if (currentQuestIndex < quests.Length)
        {
            QuestInfo currentQuest = quests[currentQuestIndex];

            if (currentQuest != null && currentQuest.isQuestActive && currentQuest.activeNPCID == "" && currentQuest.activeQuestName == "5")
            {
                quest5.SetActive(true);
            }
            else if (currentQuest != null && currentQuest.isQuestActive && currentQuest.activeNPCID == "" && currentQuest.activeQuestName == "6")
            {
                quest6.SetActive(true);
            }
            else
            {
                quest5.SetActive(false);
                quest6.SetActive(false);
            }
        }
    }

    // Mendapatkan quest yang sedang aktif
    public QuestInfo GetCurrentQuest()
    {
        if (currentQuestIndex < quests.Length)
        {
            return quests[currentQuestIndex];
        }
        return new QuestInfo(); // Jika tidak ada quest lagi
    }

    // Mendapatkan quest berdasarkan nama
    public QuestInfo GetQuestByName(string questName)
    {
        foreach (QuestInfo quest in quests)
        {
            if (quest.activeQuestName == questName)
            {
                return quest;
            }
        }
        return null;
    }

    public void StartNextQuest()
    {
        
        if (currentQuestIndex < quests.Length)
        {
            QuestInfo currentQuest = quests[currentQuestIndex];
            currentQuest.isQuestActive = true;
            currentQuest.isQuestCompleted = false;
            currentQuest.isAfterQuest = false;

        var data = new Dictionary<string, string>() {
            {"nama", NAMA}
        };

            string chapterSekarang = ProsesTemplate(currentQuest.chapter, data);
            string titleSekarang = ProsesTemplate(currentQuest.title, data);
            string descriptionSekarang = ProsesTemplate(currentQuest.questDescription, data);
            string completionSekarang = ProsesTemplate(currentQuest.completionMessage, data);

            QuestSystem.SetQuestActive(currentQuest.activeNPCID, currentQuest.activeQuestName);

            StartCoroutine(TransitionToNextQuest(chapterSekarang, titleSekarang, descriptionSekarang, "In Progress"));

            Debug.Log($"Quest '{currentQuest.activeQuestName}' dari NPC '{currentQuest.activeNPCID}' dimulai.");
        }
        else
        {
            Debug.Log("Semua quest telah selesai!");
        }
    }

    public void CompleteCurrentQuest()
    {
        if (currentQuestIndex >= 0 && currentQuestIndex < quests.Length)
        {
            QuestInfo currentQuest = quests[currentQuestIndex];
            currentQuest.isQuestCompleted = true;
            currentQuest.isQuestActive = false;
            currentQuest.isAfterQuest = false;

            var data = new Dictionary<string, string>() {
                {"nama", NAMA}
            };

            string chapterSekarang = ProsesTemplate(currentQuest.chapter, data);
            string titleSekarang = ProsesTemplate(currentQuest.title, data);
            string descriptionSekarang = ProsesTemplate(currentQuest.questDescription, data);
            string completionSekarang = ProsesTemplate(currentQuest.completionMessage, data);

            QuestSystem.SetQuestCompleted(currentQuest.activeNPCID, currentQuest.activeQuestName);

            StartCoroutine(TransitionToNextQuest(chapterSekarang, titleSekarang, completionSekarang, "Complete"));

            Debug.Log($"Quest '{currentQuest.activeQuestName}' dari NPC '{currentQuest.activeNPCID}' selesai.");
        }
    }

    public void InstantQuest()
    {
        QuestInfo currentQuest = quests[currentQuestIndex];

        var data = new Dictionary<string, string>() {
            {"nama", NAMA}
        };
        if (currentQuestIndex >= 0 && currentQuestIndex < quests.Length && !currentQuest.isQuestCompleted)
        {
            currentQuest.isQuestCompleted = false;
            currentQuest.isQuestActive = false;
            currentQuest.isAfterQuest = true;

            string chapterSekarang = ProsesTemplate(currentQuest.chapter, data);
            string titleSekarang = ProsesTemplate(currentQuest.title, data);
            string descriptionSekarang = ProsesTemplate(currentQuest.questDescription, data);
            string completionSekarang = ProsesTemplate(currentQuest.completionMessage, data);

            QuestSystem.SetAfterQuest(currentQuest.activeNPCID, currentQuest.activeQuestName);

            StartCoroutine(TransitionToNextQuest(chapterSekarang, titleSekarang, completionSekarang, "Complete"));

            currentQuestIndex++;
            StartNextQuest();

            Debug.Log($"Quest Berikutnya");
        }
        else if (currentQuestIndex >= 0 && currentQuestIndex < quests.Length && currentQuest.isQuestCompleted)
        {
            currentQuest.isQuestCompleted = false;
            currentQuest.isQuestActive = false;
            currentQuest.isAfterQuest = true;

            QuestSystem.SetAfterQuest(currentQuest.activeNPCID, currentQuest.activeQuestName);

            currentQuestIndex++;
            StartNextQuest();

            Debug.Log($"Quest Berikutnya");
        }
    }

    public void AfterQuest()
    {
        QuestInfo currentQuest = quests[currentQuestIndex];

        var data = new Dictionary<string, string>() {
            {"nama", NAMA}
        };
        if (currentQuestIndex >= 0 && currentQuestIndex < quests.Length && !currentQuest.isQuestCompleted)
        {
            currentQuest.isQuestCompleted = false;
            currentQuest.isQuestActive = false;
            currentQuest.isAfterQuest = true;

            string chapterSekarang = ProsesTemplate(currentQuest.chapter, data);
            string titleSekarang = ProsesTemplate(currentQuest.title, data);
            string descriptionSekarang = ProsesTemplate(currentQuest.questDescription, data);
            string completionSekarang = ProsesTemplate(currentQuest.completionMessage, data);

            QuestSystem.SetAfterQuest(currentQuest.activeNPCID, currentQuest.activeQuestName);

            StartCoroutine(TransitionToNextQuest(chapterSekarang, titleSekarang, completionSekarang, "Complete"));

            currentQuestIndex++;

            Debug.Log($"Quest Berikutnya");
        }
        else if (currentQuestIndex >= 0 && currentQuestIndex < quests.Length && currentQuest.isQuestCompleted)
        {
            currentQuest.isQuestCompleted = false;
            currentQuest.isQuestActive = false;
            currentQuest.isAfterQuest = true;

            QuestSystem.SetAfterQuest(currentQuest.activeNPCID, currentQuest.activeQuestName);

            currentQuestIndex++;

            Debug.Log($"Quest Berikutnya");
        }
    }

    private IEnumerator TransitionToNextQuest(string chapter, string title, string messageText, string conditionText)
    {
        // Tunggu jika UI sedang transisi
        while (UIManager.Instance.isTransitioning)
        {
            yield return null;
        }

        // Kirim callback untuk mengubah warna setelah fade out
        yield return UIManager.Instance.TransitionQuestLog(chapter, title, messageText, conditionText, () =>
        {
            if (conditionText == "In Progress")
            {
                message.color = inProgress;
                condition.color = inProgress;
            }
            else if (conditionText == "Complete")
            {
                message.color = completeColor;
                condition.color = completeColor;
            }
        });
    }
}