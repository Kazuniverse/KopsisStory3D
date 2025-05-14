using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class carroute : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator animator;
    public Transform[] waypoints;
    private int waypointIndex;
    public float pauseDuration = 0f;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints tidak diisi! Harap tambahkan waypoints di Inspector.");
            return;
        }

        waypointIndex = 0;
        MoveToWaypoint(); // Gunakan fungsi terpisah untuk kejelasan
    }

    void Update()
    {
        if (!isWaiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(PauseBeforeNextDestination());
        }
    }

    void MoveToWaypoint()
    {
        if (waypoints.Length == 0) return;
        agent.SetDestination(waypoints[waypointIndex].position);
    }

    void IterateWaypointIndex()
    {
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
    }

    IEnumerator PauseBeforeNextDestination()
    {
        isWaiting = true;
        agent.isStopped = true;

        yield return new WaitForSeconds(pauseDuration);

        IterateWaypointIndex();
        agent.isStopped = false;
        MoveToWaypoint();

        isWaiting = false;
    }
}
