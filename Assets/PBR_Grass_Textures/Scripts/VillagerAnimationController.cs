using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class VillagerAnimationController : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }
}
