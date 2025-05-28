using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool isBuildingMode = false; // �������� ������ �������������

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isBuildingMode) return; // ��������� �������� �� ����� �������������

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }

    public void SetBuildingMode(bool state)
    {
        isBuildingMode = state;
        agent.isStopped = state; // ��������� ������������� ���������
    }
}
