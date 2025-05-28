using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class VillagerAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    private float wanderRadius = 12f;
    private float waitTime = 2f;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.stoppingDistance = 0.5f;        // ✅ не 0!
        agent.autoBraking = true;             // ✅ тормозим перед целью

        GoToNewPosition();
    }

    private void Update()
    {
        // Анимация по скорости
        animator.SetFloat("Speed", agent.velocity.magnitude);

        // Проверка: достиг ли цели (с учётом остановки)
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && agent.velocity.magnitude < 0.1f && !isWaiting)
        {
            isWaiting = true;
            waitTimer = 0f;
        }

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                GoToNewPosition();
            }
        }
    }

    private void GoToNewPosition()
    {
        Vector3 newPos = GetRandomNavMeshPosition();
        agent.SetDestination(newPos);
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }
}
