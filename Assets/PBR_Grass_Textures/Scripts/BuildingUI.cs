using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    public GameObject uiPanel;
    public TextMeshProUGUI buildingNameText;
    public TextMeshProUGUI workerCountText;
    public Button addWorkerButton;  // Кнопка "Добавить работника"
    public Button removeWorkerButton; // Кнопка "Удалить работника"

    private Building currentBuilding;
    private VillageManager villageManager;

    private void Start()
    {
        villageManager = FindObjectOfType<VillageManager>();
        uiPanel.SetActive(false);

        // Привязка метода к кнопке
        addWorkerButton.onClick.AddListener(AddWorker);
        removeWorkerButton.onClick.AddListener(RemoveWorker);
    }

    public void OpenUI(Building building)
    {
        currentBuilding = building;
        uiPanel.SetActive(true);

        buildingNameText.text = building.name;
        UpdateWorkerCount();
    }

    public void CloseUI()
    {
        currentBuilding = null;
        uiPanel.SetActive(false);
    }

    private void UpdateWorkerCount()
    {
        if (currentBuilding != null)
            workerCountText.text = $"{currentBuilding.GetWorkerCount()}/{currentBuilding.requiredWorkers}";
    }

    public void AddWorker()
    {
        if (currentBuilding == null)
        {
            Debug.LogError("❌ Нет выбранного здания!");
            return;
        }

        // Открытие панели выбора жителя при нажатии на "+"
        UIListManager uiListManager = FindObjectOfType<UIListManager>();
        if (uiListManager != null)
        {
            uiListManager.OpenVillagerSelectionPanel(currentBuilding); // Открытие панели выбора жителя
        }
        else
        {
            Debug.LogError("❌ UIListManager не найден!");
        }
    }

    private void RemoveWorker()
    {
        if (currentBuilding == null)
        {
            Debug.LogError("❌ Нет выбранного здания!");
            return;
        }

        Villager workerToRemove = null;
        foreach (Villager villager in currentBuilding.assignedWorkers) // Используем список здания!
        {
            workerToRemove = villager;
            break;
        }

        if (workerToRemove != null && currentBuilding.RemoveWorker(workerToRemove))
        {
            UpdateWorkerCount();
        }
        else
        {
            Debug.Log("⚠️ Нет работников в этом здании!");
        }
    }
}
