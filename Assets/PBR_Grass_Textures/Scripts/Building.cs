using UnityEngine;
using System.Collections; // Для IEnumerator
using System.Collections.Generic; // Для List<>

public abstract class Building : MonoBehaviour
{
    public enum BuildingType
    {
        Farm,
        WaterStation,
        Generator
    }

    public BuildingType buildingType; // Тип здания

    public float resourceAmount = 1f;
    public float generationInterval = 3f;
    public int requiredWorkers = 1;
    public int requiredVillagersToBuild = 0; // Минимальное количество жителей для постройки
    public List<Villager> assignedWorkers = new List<Villager>();
    protected VillageManager villageManager;

    protected virtual void Start()
    {
        villageManager = FindObjectOfType<VillageManager>();

        if (villageManager == null)
        {
            Debug.LogError("❌ VillageManager не найден!");
            return;
        }

        // Проверяем, достаточно ли жителей для постройки
        if (villageManager.GetVillagerCount() < requiredVillagersToBuild)
        {
            Debug.LogWarning($"⚠️ Недостаточно жителей для строительства {gameObject.name}. Требуется: {requiredVillagersToBuild}");
            gameObject.SetActive(false); // Отключаем здание, если недостаточно жителей
        }
        else
        {
            InvokeRepeating(nameof(TryGenerateResources), generationInterval, generationInterval); // Генерация ресурсов
        }
    }

    public BuildingType GetBuildingType()
    {
        return buildingType; // Возвращаем тип здания
    }

    public int GetWorkerCount() => assignedWorkers.Count;

    private void OnMouseDown()
    {
        Debug.Log($"📌 Клик по {gameObject.name}");

        BuildingUI ui = FindObjectOfType<BuildingUI>();
        if (ui != null)
        {
            ui.OpenUI(this);
        }
        else
        {
            Debug.LogError("❌ BuildingUI не найден!");
        }
    }

    public bool AssignWorker(Villager villager)
    {
        if (assignedWorkers.Count >= requiredWorkers)
        {
            Debug.Log($"❌ В здании {gameObject.name} уже достаточно рабочих!");
            return false;
        }

        if (villager.IsAssigned)
        {
            Debug.Log($"⚠️ Житель {villager.name} уже работает в {villager.AssignedBuilding.name}!");
            return false;
        }

        assignedWorkers.Add(villager);
        villager.SetAssignedBuilding(this);
        villager.gameObject.SetActive(false);

        villageManager.UpdateFreeVillagers(); // Обновление списка свободных жителей

        Debug.Log($"✅ Житель {villager.name} добавлен в {gameObject.name}!");
        return true;
    }

    public bool RemoveWorker(Villager villager)
    {
        if (assignedWorkers.Contains(villager))
        {
            assignedWorkers.Remove(villager);
            villageManager.ReleaseVillager(villager);
            villager.SetAssignedBuilding(null);
            villager.gameObject.SetActive(true);

            villageManager.UpdateFreeVillagers(); // Обновление после удаления

            Debug.Log($"✅ Житель {villager.name} удалён из {gameObject.name}");
            return true;
        }

        Debug.LogWarning($"⚠️ Жителя {villager.name} нет в {gameObject.name}!");
        return false;
    }

    private void TryGenerateResources()
    {
        if (assignedWorkers.Count >= requiredWorkers)
        {
            GenerateResources(); // Генерация ресурсов
        }
    }

    // Метод для получения коэффициента для каждого типа здания
    protected virtual float GetResourceCoefficient()
    {
        return 1f; // По умолчанию коэффициент равен 1
    }

    protected abstract void GenerateResources();
}
