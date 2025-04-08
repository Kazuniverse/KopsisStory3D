using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class carroute : MonoBehaviour
{
    private NavMeshAgent agent; // Komponen NavMeshAgent untuk pergerakan NPC
    public Animator animator;
    public Transform[] waypoints; // Daftar waypoint yang akan dilalui NPC
    private int waypointIndex; // Indeks waypoint saat ini
    private Vector3 target; // Target posisi waypoint
    public float pauseDuration = 0f; // Durasi berhenti sejenak di setiap waypoint
    private bool isWaiting = false; // Flag untuk menandai apakah NPC sedang menunggu

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints tidak diisi! Harap tambahkan waypoints di Inspector.");
            return;
        }
        
        waypointIndex = 0; // Atur indeks waypoint ke 0
        UpdateDestination();
    }

    void Update()
    {
        // Jika NPC tidak sedang menunggu dan sudah mencapai target
        if (!isWaiting && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(PauseBeforeNextDestination());
        }
    }

    // Memperbarui tujuan NPC ke waypoint berikutnya
    void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    // Pindah ke waypoint berikutnya dalam array
    void IterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0; // Kembali ke waypoint pertama jika sudah mencapai akhir
        }
    }

    // Coroutine untuk berhenti sejenak sebelum pindah ke waypoint berikutnya
    IEnumerator PauseBeforeNextDestination()
    {
        isWaiting = true; // Set flag isWaiting menjadi true
        agent.isStopped = true; // Hentikan pergerakan NPC

        yield return new WaitForSeconds(pauseDuration); // Tunggu selama pauseDuration

        agent.isStopped = false; // Lanjutkan pergerakan NPC
        IterateWaypointIndex(); // Pindah ke waypoint berikutnya
        UpdateDestination(); // Perbarui tujuan NPC

        isWaiting = false; // Set flag isWaiting menjadi false
    }
}