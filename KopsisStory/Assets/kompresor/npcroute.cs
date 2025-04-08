using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class npcroute : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator animator;
    [SerializeField] private Transform[] schoolRoute;

    private int waypointIndex;
    private Vector3 target;
    private float pauseDuration = Random.Range(0, 3);
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
        UpdateDestination(); 
        animator.SetBool("jalan", true);
    }

    void Update()
    {
        UpdateDestination();
        if (!isWaiting && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(PauseBeforeNextDestination());
        }
    }

    public void UpdateDestination()
    {
        target = schoolRoute[waypointIndex].position;
        agent.SetDestination(target);
    }

    public IEnumerator PauseBeforeNextDestination()
    {
        isWaiting = true;
        animator.SetBool("jalan", false);
        agent.isStopped = true;

        yield return new WaitForSeconds(pauseDuration);

        animator.SetBool("jalan", true);
        agent.isStopped = false;

        waypointIndex++;
        if (waypointIndex >= schoolRoute.Length)
        {
            waypointIndex = 0;
        }

        isWaiting = false;
    }
}