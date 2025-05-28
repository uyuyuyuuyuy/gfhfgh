using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            Debug.Log("������ ������!");

            // ���������� ��������� ����� ��������
            Inventory.Instance.AddResource("����������", 10);  // ����� ��������� 10 ������ �����������

            // ���������� ������ (���)
            Destroy(other.gameObject);
        }
    }
}
