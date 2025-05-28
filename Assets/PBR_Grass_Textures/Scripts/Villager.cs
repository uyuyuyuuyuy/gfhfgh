using UnityEngine;
using System.Collections;

public class Villager : MonoBehaviour
{
    public Building AssignedBuilding { get; private set; } = null;
    public bool IsAssigned => AssignedBuilding != null;

    private VillageManager villageManager;
    public VillagerSkills skills; // Навыки

    private Renderer _renderer;
    private Color originalColor; // Оригинальный цвет жителя
    private MaterialPropertyBlock _materialPropertyBlock; // Для изменения цвета без влияния на остальные объекты

    private GameObject highlightIndicator; // Для создания индикатора над головой

    [Header("Skill Progression Settings")]
    public float skillIncreaseInterval = 5f; // Время между увеличениями навыков (в секундах)

    public Book equippedBook; // Слот для книги (одна книга на жителя)
    private Coroutine skillProgressionCoroutine; // Храним ссылку на корутину для остановки

    [Header("UI Elements")]
    public UnityEngine.UI.Button bookSlotButton; // Кнопка для выбора книги

    private void Start()
    {
        villageManager = FindObjectOfType<VillageManager>();
        if (villageManager == null)
        {
            Debug.LogError("❌ VillageManager не найден!");
        }
        else
        {
            skills = new VillagerSkills(); // Генерируем случайные навыки при старте
            skills.PrintSkills(); // Выводим их в консоль для проверки
        }

        // Привязка кнопки для использования книги
        if (bookSlotButton != null)
        {
            bookSlotButton.onClick.AddListener(OpenInventoryForBook);
        }

        // Запускаем прогрессию навыков
        StartCoroutine(ConsumeResourcesRoutine()); // Запускаем потребление ресурсов
    }

    // Метод для открытия инвентаря и выбора книги
    public void OpenInventoryForBook()
    {
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.DisplayInventory(villageManager.GetAvailableBooks(), this); // Передаем текущего жителя для применения книги
        }
    }

    // Метод для использования книги
    public void EquipBook(Book book)
    {
        equippedBook = book; // Применяем книгу к жителю
        book.Use(this); // Применяем бафф книги к жителю
        UpdateUI(); // Обновляем UI
    }

    private IEnumerator ConsumeResourcesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f); // Потребление ресурсов
            ConsumeResources();
        }
    }

    private void ConsumeResources()
    {
        if (villageManager == null) return;

        float foodBefore = villageManager.food;
        float waterBefore = villageManager.water;

        villageManager.UseResources(1f, 1f, 0f); // Житель потребляет 1 еды и 1 воды

        
    }

    public void SetAssignedBuilding(Building building)
    {
        AssignedBuilding = building;
        if (building == null)
        {
            villageManager.ReleaseVillager(this); // Добавляем обратно в свободные
        }
    }

    public bool AssignWorker(Building building)
    {
        if (!IsAssigned)
        {
            AssignedBuilding = building;
            gameObject.SetActive(false);
            villageManager.AssignVillager(this); // Житель теперь занят
            Debug.Log($"✅ {name} назначен в {building.name}");
            return true;
        }

        Debug.Log($"⚠️ {name} уже работает в {AssignedBuilding.name}");
        return false;
    }

    public bool RemoveWorker()
    {
        if (AssignedBuilding != null)
        {
            Debug.Log($"✅ {name} освобождён из {AssignedBuilding.name}");

            AssignedBuilding = null;
            villageManager.ReleaseVillager(this); // Освобождаем жителя
            gameObject.SetActive(true); // Житель снова видим

            // Обновляем UI при удалении из здания
            UpdateUI();
            return true;
        }

        Debug.LogWarning($"⚠️ {name} не был назначен в здание!");
        return false;
    }

    // Метод для обновления UI после изменения навыков
    public void UpdateUI()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null && uiManager.villagerInfoPanel.activeSelf && uiManager.currentVillager == this)
        {
            // Обновляем слайдеры UI
            uiManager.strengthSlider.value = skills.strength;
            uiManager.enduranceSlider.value = skills.endurance;
            uiManager.technicalSkillsSlider.value = skills.technicalSkills;
        }
    }
}
