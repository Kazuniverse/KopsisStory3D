using UnityEngine;
using System.Collections.Generic;

public class QuestSystem : MonoBehaviour
{
    private static Dictionary<string, bool> activeQuests = new Dictionary<string, bool>();
    private static Dictionary<string, bool> completedQuests = new Dictionary<string, bool>();
    private static Dictionary<string, bool> afterQuests = new Dictionary<string, bool>();

    public static void SetQuestActive(string npcID, string questName)
    {
        string key = $"{npcID}_{questName}";
        Debug.Log($"Quest '{questName}' dari NPC '{npcID}' telah aktif.");
        activeQuests[key] = true;
    }

    public static void SetQuestCompleted(string npcID, string questName)
    {
        string key = $"{npcID}_{questName}";
        Debug.Log($"Quest '{questName}' dari NPC '{npcID}' telah selesai.");
        completedQuests[key] = true;
        activeQuests[key] = false; // Quest tidak lagi aktif
    }

    public static void SetAfterQuest(string npcID, string questName)
    {
        string key = $"{npcID}_{questName}";
        Debug.Log($"Quest '{questName}' dari NPC '{npcID}' telah masuk fase after quest.");
        afterQuests[key] = true;
        completedQuests[key] = false; // Quest tidak lagi dalam status selesai
    }

    public static bool IsQuestActive(string npcID, string questName)
    {
        string key = $"{npcID}_{questName}";
        return activeQuests.ContainsKey(key) && activeQuests[key];
    }

    public static bool IsQuestCompleted(string npcID, string questName)
    {
        string key = $"{npcID}_{questName}";
        return completedQuests.ContainsKey(key) && completedQuests[key];
    }

    public static bool IsAfterQuest(string npcID, string questName)
    {
        string key = $"{npcID}_{questName}";
        return afterQuests.ContainsKey(key) && afterQuests[key];
    }
}