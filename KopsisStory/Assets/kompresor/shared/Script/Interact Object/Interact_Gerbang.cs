using UnityEngine;

public class Interact_Gerbang : OnRaycast
{
    public GameObject gkiri;
    public GameObject gkanan;
    public bool isopen;
    private Vector3 openposa;
    private Vector3 openposb;
    private Vector3 closedposa;
    private Vector3 closedposb;
    [SerializeField] private float openspeed;

    public QuestManager questManager; // Referensi ke QuestManager

    // private string title;
    // private string completionMessage;

    [System.Obsolete]
    void Start()
    {
        openposa = new Vector3(2.9f, -0.4708914f, 0f);  // Posisi kanan terbuka
        openposb = new Vector3(-2.9f, -0.4708914f, 0f); // Posisi kiri terbuka

        closedposa = gkanan.transform.localPosition;
        closedposb = gkiri.transform.localPosition;

        Debug.Log("Closed Pos Kanan: " + closedposa);
        Debug.Log("Closed Pos Kiri: " + closedposb);

        questManager = FindObjectOfType<QuestManager>();
        if (questManager == null)
        {
            Debug.LogError("QuestManager tidak ditemukan di scene!");
        }
    }

    void Update()
    {
        Debug.Log("Is Open: " + isopen);

        Vector3 targetKiri = isopen ? openposb : closedposb;
        Vector3 targetKanan = isopen ? openposa : closedposa;

        gkiri.transform.localPosition = Vector3.MoveTowards(gkiri.transform.localPosition, targetKiri, openspeed * Time.deltaTime);
        gkanan.transform.localPosition = Vector3.MoveTowards(gkanan.transform.localPosition, targetKanan, openspeed * Time.deltaTime);
    }

    public override void OnInteract()
    {
        bool previousState = isopen;
        isopen = !isopen;
        Debug.Log("Interacted: " + isopen);

        QuestInfo currentQuest = questManager.GetCurrentQuest();
        if (currentQuest != null && currentQuest.isQuestActive && currentQuest.activeNPCID == "A" && currentQuest.activeQuestName == "1")
        {
            questManager.CompleteCurrentQuest();
        }
    }
}