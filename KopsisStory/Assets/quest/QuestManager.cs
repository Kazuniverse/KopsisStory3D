using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestInfo
{
    public string title;
    public string questDescription;
    public string completionMessage;
    public string activeNPCID; // NPC ID yang terkait dengan quest ini
    public string activeQuestName; // Nama quest yang terkait dengan NPC
    public bool isQuestActive;
    public bool isQuestCompleted;
    public bool isAfterQuest;
}

public class QuestManager : MonoBehaviour
{
    public Color inProgress;
    public Color completeColor;
    public Text message;
    public Text condition;

    public QuestInfo[] quests; // Array untuk menyimpan urutan quest
    private int currentQuestIndex = 0; // Indeks quest yang sedang aktif

    private UIManager ui;

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

            QuestSystem.SetQuestActive(currentQuest.activeNPCID, currentQuest.activeQuestName);

            UIManager.Instance.ShowQuestLog(currentQuest.title, currentQuest.questDescription, "In Progress");

            message.color = inProgress;
            condition.color = inProgress;

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

            QuestSystem.SetQuestCompleted(currentQuest.activeNPCID, currentQuest.activeQuestName);

            UIManager.Instance.ShowQuestLog(currentQuest.title, currentQuest.completionMessage, "Complete");

            message.color = completeColor;
            condition.color = completeColor;

            Debug.Log($"Quest '{currentQuest.activeQuestName}' dari NPC '{currentQuest.activeNPCID}' selesai.");
        }
    }

    public void AfterQuest()
    {
        if (currentQuestIndex >= 0 && currentQuestIndex < quests.Length)
        {
            QuestInfo currentQuest = quests[currentQuestIndex];
            currentQuest.isQuestCompleted = false;
            currentQuest.isQuestActive = false;
            currentQuest.isAfterQuest = true;

            QuestSystem.SetAfterQuest(currentQuest.activeNPCID, currentQuest.activeQuestName);

            currentQuestIndex++;
            StartNextQuest();

            Debug.Log($"Quest Berikutnya");
        }
    }
}