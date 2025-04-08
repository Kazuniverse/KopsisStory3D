using UnityEngine;
using System.Linq;
using cherrydev;

[System.Serializable]
public class NPC
{
    public string questName; // Nama quest yang terkait dengan NPC
    public DialogNodeGraph[] dialogGraphs; // Array dari dialog graph yang terkait dengan NPC
}

public class DialogSetup : MonoBehaviour
{
    public DialogBehaviour dialogBehaviour;
    public NPC[] npcs;
    public QuestManager questManager;
    public string npcID;

    private void Start()
    {
        if (dialogBehaviour != null)
        {
            dialogBehaviour.BindExternalFunction("bukapgr", () => questManager.StartNextQuest());
            dialogBehaviour.BindExternalFunction("to_after", () => questManager.AfterQuest());
        }
    }

    public void StartDialogForNPC()
    {
        QuestInfo currentQuest = questManager.GetCurrentQuest();
        NPC targetNPC = null;
        QuestInfo targetQuest = null;

        // Cek quest aktif yang terkait dengan NPC ini
        if (currentQuest != null && currentQuest.activeNPCID == npcID)
        {
            targetNPC = FindNPCByQuestName(currentQuest.activeQuestName);
            targetQuest = currentQuest;
        }
        else
        {
            // Cari quest terakhir dari NPC ini
            string lastQuestName = FindLastQuestForNPC();
            if (!string.IsNullOrEmpty(lastQuestName))
            {
                targetQuest = questManager.GetQuestByName(lastQuestName);
                targetNPC = FindNPCByQuestName(lastQuestName);
            }
        }

        if (targetNPC != null && targetQuest != null)
        {
            ShowQuestStateDialog(targetNPC, targetQuest);
        }
        else
        {
            UseDefaultDialog();
        }
    }

    private void ShowQuestStateDialog(NPC npc, QuestInfo quest)
    {
        int stateIndex = 0;
        if (quest.isAfterQuest) stateIndex = 3;
        else if (quest.isQuestCompleted) stateIndex = 2;
        else if (quest.isQuestActive) stateIndex = 1;

        if (npc.dialogGraphs.Length > stateIndex)
        {
            dialogBehaviour.StartDialog(npc.dialogGraphs[stateIndex]);
        }
        else
        {
            Debug.LogError($"Dialog graph tidak tersedia untuk state {stateIndex}");
            UseDefaultDialog();
        }
    }

    private void UseDefaultDialog()
    {
        NPC defaultNPC = npcs.FirstOrDefault(n => string.IsNullOrEmpty(n.questName));
        
        if (defaultNPC != null && defaultNPC.dialogGraphs.Length > 0)
        {
            dialogBehaviour.StartDialog(defaultNPC.dialogGraphs[0]);
        }
        else if (npcs.Length > 0)
        {
            dialogBehaviour.StartDialog(npcs[0].dialogGraphs[0]);
        }
        else
        {
            Debug.LogError("Tidak ada dialog yang tersedia");
        }
    }

    private NPC FindNPCByQuestName(string questName)
    {
        return npcs.FirstOrDefault(npc => npc.questName == questName);
    }

    private string FindLastQuestForNPC()
    {
        // Mencari quest terakhir yang dimiliki NPC ini berdasarkan urutan quests
        for (int i = questManager.quests.Length - 1; i >= 0; i--)
        {
            if (questManager.quests[i].activeNPCID == npcID)
            {
                return questManager.quests[i].activeQuestName;
            }
        }
        return null;
    }
}