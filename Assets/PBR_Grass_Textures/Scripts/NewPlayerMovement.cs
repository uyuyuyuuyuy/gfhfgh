using UnityEngine;
using UnityEngine.AI;

public class NewPlayerMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private bool isBuildingMode = false;

    // Радиус проверки точки на NavMesh
    public float navMeshSampleRadius = 1.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component missing on " + gameObject.name);
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator component missing on " + gameObject.name);
        }
    }

    void Update()
    {
        if (isBuildingMode)
        {
            if (agent != null)
            {
                agent.isStopped = true;
            }
            return; // Не двигаемся в режиме строительства
        }
        else
        {
            if (agent != null)
            {
                agent.isStopped = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast hit point: " + hit.point);

                if (agent != null)
                {
                    NavMeshHit navHit;
                    // Проверяем, лежит ли точка на NavMesh
                    if (NavMesh.SamplePosition(hit.point, out navHit, navMeshSampleRadius, NavMesh.AllAreas))
                    {
                        Debug.Log("NavMesh position found: " + navHit.position);
                        agent.SetDestination(navHit.position);
                        agent.isStopped = false;
                    }
                    else
                    {
                        Debug.LogWarning("No valid NavMesh position near hit.point");
                    }
                }
            }
            else
            {
                Debug.Log("Raycast did not hit any collider");
            }
        }

        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (animator == null || agent == null) return;

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    public void SetBuildingMode(bool state)
    {
        isBuildingMode = state;
        if (agent != null)
        {
            agent.isStopped = state;
        }
        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
        }
    }
}
