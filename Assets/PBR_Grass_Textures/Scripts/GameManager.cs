using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // ������ ��� ���������� ���������
    public float savedResources = 100f;
    public string[] savedBuildings = { "Building1", "Building2" };
    public string[] savedVillagers = { "Villager1", "Villager2" };

    // ������, ������� �� ����� ��������� ��� ���������
    void Awake()
    {
        // ���� ������ ��� ����������, ���������� ���
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ��������� ������ ����� �������
        }
        else
        {
            Destroy(gameObject); // ���������� ��������
        }
    }

    // ����� ��� ���������� ���������
    public void SaveState(float resources, string[] buildings, string[] villagers)
    {
        savedResources = resources;
        savedBuildings = buildings;
        savedVillagers = villagers;
    }

    // ����� ��� �������������� ���������
    public void RestoreState()
    {
        Debug.Log("Restored Resources: " + savedResources);
        Debug.Log("Restored Buildings: " + string.Join(", ", savedBuildings));
        Debug.Log("Restored Villagers: " + string.Join(", ", savedVillagers));
    }
}
