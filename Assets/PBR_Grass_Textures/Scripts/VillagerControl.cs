using UnityEngine;
using UnityEngine.AI;

public class VillagerControl : MonoBehaviour
{
    private Camera mainCamera;
    private NavMeshAgent agent;

    private void Start()
    {
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent ����������� �� �����, ��������� ���!");
            agent = gameObject.AddComponent<NavMeshAgent>();
            agent.speed = 3.0f; // �������� ������������
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                Debug.Log($"���� ���� � {hit.point}");
            }
        }
    }
}
