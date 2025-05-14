using Unity.VisualScripting;
using UnityEngine;

public class NPCLocation : MonoBehaviour
{
    public Transform[] npc;
    public Transform[] npcLocation;
    public Transform[] npcLocation2;
    public GameObject collide;
    public QuestManager quest;

    public bool playerTriggered = false;

    void Start()
    {
        
    }

    void Update()
    {
        QuestInfo currentQuest = quest.GetCurrentQuest();
        
        if (currentQuest != null && currentQuest.isQuestCompleted && currentQuest.activeNPCID == "B" && currentQuest.activeQuestName == "1")
        {
            npc[1].gameObject.SetActive(true);
        }
    }

    public void Pindah6()
    {
        QuestInfo currentQuest = quest.GetCurrentQuest();

        if (currentQuest != null && currentQuest.activeNPCID == "A" && currentQuest.activeQuestName == "3")
        {
            for (int i = 1; i <= 3; i++)
            {
                if (i < npc.Length)
                {
                    npc[i].gameObject.SetActive(false);
                }
            }
            npc[0].position = npcLocation[4].position;
            npc[0].rotation = npcLocation[4].rotation;
        }
    }
}
