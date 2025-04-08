using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questID;
    public string questName;
    public string npcID;
    public string title;
    public string description;
    public bool isActive;
    public bool isCompleted;
    public bool isAfterQuest;

    public Quest(string id, string name, string npc)
    {
        questID = id;
        questName = name;
        npcID = npc;
        isActive = false;
        isCompleted = false;
        isAfterQuest = false;
    
    }
}