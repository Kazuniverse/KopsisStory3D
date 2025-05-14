using UnityEngine;
using System.Linq;
using cherrydev;

[System.Serializable]
public class NPC
{
    public string questName;
    public DialogNodeGraph[] dialogGraphs; // 0=default, 1=active, 2=completed, 3=after
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
            dialogBehaviour.BindExternalFunction("next", () => questManager.StartNextQuest());
            dialogBehaviour.BindExternalFunction("done", () => questManager.CompleteCurrentQuest());
            dialogBehaviour.BindExternalFunction("after", () => questManager.AfterQuest());
            dialogBehaviour.BindExternalFunction("instant", () => questManager.InstantQuest());
        }
    }

    public void StartDialogForNPC()
    {
        QuestInfo currentQuest = questManager.GetCurrentQuest();
        QuestInfo[] allQuests = questManager.quests;

        int currentIndex = System.Array.IndexOf(allQuests, currentQuest);

        // 1. Jika quest aktif dan milik NPC ini
        if (currentQuest != null && currentQuest.activeNPCID == npcID)
        {
            NPC npc = FindNPCByQuestName(currentQuest.activeQuestName);
            if (npc != null)
            {
                ShowQuestStateDialog(npc, currentQuest);
                return;
            }
        }

        // 2. Cari quest terakhir SEBELUM quest aktif yang pernah melibatkan NPC ini
        for (int i = currentIndex - 1; i >= 0; i--)
        {
            if (allQuests[i].activeNPCID == npcID)
            {
                NPC npc = FindNPCByQuestName(allQuests[i].activeQuestName);
                if (npc != null)
                {
                    ShowQuestStateDialog(npc, allQuests[i]);
                    return;
                }
            }
        }

        // 3. Jika tidak ada quest yang cocok, gunakan dialog default
        UseDefaultDialog();
    }

    private void ShowQuestStateDialog(NPC npc, QuestInfo quest)
    {
        int stateIndex = 0;
        if (quest.isAfterQuest) stateIndex = 3;
        else if (quest.isQuestCompleted) stateIndex = 2;
        else if (quest.isQuestActive) stateIndex = 1;

        if (npc.dialogGraphs.Length > stateIndex && npc.dialogGraphs[stateIndex] != null)
        {
            dialogBehaviour.StartDialog(npc.dialogGraphs[stateIndex]);
        }
        else
        {
            Debug.LogWarning($"Dialog graph untuk state {stateIndex} tidak tersedia. Jalankan default.");
            UseDefaultDialog();
        }
    }

    private void UseDefaultDialog()
    {
        // Menemukan NPC dengan questName kosong sebagai default
        NPC defaultNPC = npcs.FirstOrDefault(n =>
            string.IsNullOrEmpty(n.questName) &&
            n.dialogGraphs.Length > 0 &&
            n.dialogGraphs[0] != null);

        if (defaultNPC != null)
        {
            dialogBehaviour.StartDialog(defaultNPC.dialogGraphs[0]);
        }
        else
        {
            Debug.LogError($"Dialog default tidak ditemukan untuk NPC {npcID}");
        }
    }

    private NPC FindNPCByQuestName(string questName)
    {
        // Menemukan NPC berdasarkan nama quest
        return npcs.FirstOrDefault(npc => npc.questName == questName);
    }
}
