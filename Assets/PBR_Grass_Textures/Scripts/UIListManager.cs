using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIListManager : MonoBehaviour
{
    public GameObject villagerSelectionPanel;  // Панель с выбором жителя
    public GameObject villagerButtonPrefab;    // Префаб кнопки для выбора жителя
    public Transform villagerListContainer;    // Контейнер для кнопок жителей

    private VillageManager villageManager;
    private Building currentBuilding;

    void Start()
    {
        villageManager = FindObjectOfType<VillageManager>();
        villagerSelectionPanel.SetActive(false); // Скрываем панель по умолчанию
    }

    public void OpenVillagerSelectionPanel(Building building)
    {
        currentBuilding = building;
        villagerSelectionPanel.SetActive(true); // Показываем панель выбора жителя

        // Очищаем контейнер кнопок
        foreach (Transform child in villagerListContainer)
        {
            Destroy(child.gameObject);
        }

        // Создаем кнопки для всех свободных жителей
        foreach (Villager villager in villageManager.GetFreeVillagers())  // Используем GetFreeVillagers для получения свободных жителей
        {
            CreateVillagerButton(villager);
        }
    }

    private void CreateVillagerButton(Villager villager)
    {
        GameObject villagerButton = Instantiate(villagerButtonPrefab, villagerListContainer);
        Button button = villagerButton.GetComponent<Button>();

        // Устанавливаем текст с навыками для кнопки
        TextMeshProUGUI buttonText = villagerButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = $"{villager.name}\nСила: {villager.skills.strength}\nВыносливость: {villager.skills.endurance}\nТех. Навыки: {villager.skills.technicalSkills}";

        // Назначаем событие при нажатии
        button.onClick.AddListener(() => AssignVillagerToBuilding(villager));
    }

    private void AssignVillagerToBuilding(Villager villager)
    {
        if (currentBuilding != null)
        {
            currentBuilding.AssignWorker(villager); // Назначаем выбранного жителя на работу
            villagerSelectionPanel.SetActive(false); // Закрываем панель
        }
    }
}
