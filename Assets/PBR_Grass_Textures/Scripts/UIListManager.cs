using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIListManager : MonoBehaviour
{
    public GameObject villagerSelectionPanel;  // ������ � ������� ������
    public GameObject villagerButtonPrefab;    // ������ ������ ��� ������ ������
    public Transform villagerListContainer;    // ��������� ��� ������ �������

    private VillageManager villageManager;
    private Building currentBuilding;

    void Start()
    {
        villageManager = FindObjectOfType<VillageManager>();
        villagerSelectionPanel.SetActive(false); // �������� ������ �� ���������
    }

    public void OpenVillagerSelectionPanel(Building building)
    {
        currentBuilding = building;
        villagerSelectionPanel.SetActive(true); // ���������� ������ ������ ������

        // ������� ��������� ������
        foreach (Transform child in villagerListContainer)
        {
            Destroy(child.gameObject);
        }

        // ������� ������ ��� ���� ��������� �������
        foreach (Villager villager in villageManager.GetFreeVillagers())  // ���������� GetFreeVillagers ��� ��������� ��������� �������
        {
            CreateVillagerButton(villager);
        }
    }

    private void CreateVillagerButton(Villager villager)
    {
        GameObject villagerButton = Instantiate(villagerButtonPrefab, villagerListContainer);
        Button button = villagerButton.GetComponent<Button>();

        // ������������� ����� � �������� ��� ������
        TextMeshProUGUI buttonText = villagerButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = $"{villager.name}\n����: {villager.skills.strength}\n������������: {villager.skills.endurance}\n���. ������: {villager.skills.technicalSkills}";

        // ��������� ������� ��� �������
        button.onClick.AddListener(() => AssignVillagerToBuilding(villager));
    }

    private void AssignVillagerToBuilding(Villager villager)
    {
        if (currentBuilding != null)
        {
            currentBuilding.AssignWorker(villager); // ��������� ���������� ������ �� ������
            villagerSelectionPanel.SetActive(false); // ��������� ������
        }
    }
}
