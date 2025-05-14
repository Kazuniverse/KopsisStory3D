using UnityEngine;

public class tabrakafterA2 : MonoBehaviour
{
    public QuestManager quest;
    public NPCLocation locate;
    public GameObject quest5;
    public GameObject quest6;
    public GameObject dani;
    public GameObject targetDani;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider other)
    {
        QuestInfo currentQuest = quest.GetCurrentQuest();
        if(other.CompareTag("Player") && currentQuest != null && !currentQuest.isQuestActive && currentQuest.activeNPCID == "B" && currentQuest.activeQuestName == "1")
        {
            quest.StartNextQuest();
            dani.transform.position = targetDani.transform.position;
            dani.transform.rotation = targetDani.transform.rotation;
            Destroy(targetDani);
        }

        if (currentQuest != null && currentQuest.activeNPCID == "" && currentQuest.activeQuestName == "5")
        {
            for (int i = 0; i <= 3; i++)
            {
                if (i < locate.npc.Length && i < locate.npcLocation.Length && locate.npc[i] != null && locate.npcLocation[i] != null)
                {
                    locate.npc[i].position = locate.npcLocation[i].position;
                    locate.npc[i].rotation = locate.npcLocation[i].rotation;
                }
            }
        }

        if (currentQuest != null && currentQuest.activeNPCID == "B" && currentQuest.activeQuestName == "2")
        {
            dani.SetActive(false);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            QuestInfo currentQuest = quest.GetCurrentQuest();
            if (currentQuest != null && currentQuest.activeNPCID == "" && currentQuest.activeQuestName == "6")
            {
                for (int i = 0; i <= 3; i++)
                {
                    if (i < locate.npc.Length && i < locate.npcLocation.Length && locate.npc[i] != null && locate.npcLocation[i] != null)
                    {
                        locate.npc[i].position = locate.npcLocation2[i].position;
                        locate.npc[i].rotation = locate.npcLocation2[i].rotation;
                    }
                }
            }
        }
    }

    public void Penghancuran()
    {
        QuestInfo currentQuest = quest.GetCurrentQuest();

        if (currentQuest != null && currentQuest.activeNPCID == "" && currentQuest.activeQuestName == "6")
        {
            Destroy(quest5);
        }
        else if (currentQuest != null && currentQuest.activeNPCID == "A" && currentQuest.activeQuestName == "3")
        {
            Destroy(quest6);
        }
    }
}
