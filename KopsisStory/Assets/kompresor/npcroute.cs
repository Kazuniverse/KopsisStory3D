using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class npcroute : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator animator;
    [SerializeField] private Transform[] schoolRoute;

    private int waypointIndex;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (schoolRoute == null || schoolRoute.Length == 0)
        {
            Debug.LogError("School waypoints not assigned!");
            return;
        }

        waypointIndex = 0;
        MoveToWaypoint();
        if (animator != null) animator.SetBool("jalan", true);
    }

    void Update()
    {
        if (!isWaiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(PauseBeforeNextDestination());
        }
    }

    private void MoveToWaypoint()
    {
        if (schoolRoute.Length == 0) return;
        agent.SetDestination(schoolRoute[waypointIndex].position);
    }

    private IEnumerator PauseBeforeNextDestination()
    {
        isWaiting = true;

        if (animator != null) animator.SetBool("jalan", false);
        agent.isStopped = true;

        float pauseDuration = Random.Range(0.5f, 3f);
        yield return new WaitForSeconds(pauseDuration);

        waypointIndex = (waypointIndex + 1) % schoolRoute.Length;

        agent.isStopped = false;
        MoveToWaypoint();
        if (animator != null) animator.SetBool("jalan", true);

        isWaiting = false;
    }
}
