using UnityEngine;
using UnityEngine.UI; // Для работы с Slider
using UnityEngine.EventSystems; // Для проверки, был ли клик на UI элементе

public class UIManager : MonoBehaviour
{
    public GameObject villagerInfoPanel;  // Панель с информацией о жителе
    public Slider strengthSlider;         // Слайдер для силы
    public Slider enduranceSlider;        // Слайдер для выносливости
    public Slider technicalSkillsSlider;  // Слайдер для технических навыков
    public GameObject highlightIndicatorPrefab; // Ссылка на HighlightIndicator (сферу)
    private GameObject currentHighlightIndicator;

    public Villager currentVillager; // Текущий выбранный житель
    public InventoryUI inventoryUI;  // Ссылка на InventoryUI для отображения инвентаря

    private VillageManager villageManager; // Добавляем ссылку на VillageManager

    // Метод для открытия панели с информацией о выбранном жителе
    public void OpenVillagerInfo(Villager villager)
    {
        currentVillager = villager;
        villagerInfoPanel.SetActive(true);  // Показываем панель с информацией о жителе
        if (currentHighlightIndicator != null) Destroy(currentHighlightIndicator.gameObject);
        currentHighlightIndicator = Instantiate(highlightIndicatorPrefab, villager.gameObject.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        currentHighlightIndicator.transform.SetParent(villager.gameObject.transform);
        // Отображаем навыки через слайдеры
        strengthSlider.value = villager.skills.strength;
        enduranceSlider.value = villager.skills.endurance;
        technicalSkillsSlider.value = villager.skills.technicalSkills;
    }

    // Метод для скрытия HighlightIndicator
    public void CloseVillagerInfo()
    {
        villagerInfoPanel.SetActive(false); // Скрываем панель
        if (currentHighlightIndicator != null)
            Destroy(currentHighlightIndicator.gameObject);
    }

    // Метод для открытия инвентаря текущего жителя
    public void OpenInventoryForCurrentVillager()
    {
        if (currentVillager != null && inventoryUI != null)
        {
            inventoryUI.DisplayInventory(villageManager.GetAvailableBooks(), currentVillager); // Передаем выбранного жителя для инвентаря
            villagerInfoPanel.SetActive(false); // Скрываем панель с информацией о жителе
        }
    }

    // Метод для скрытия инвентаря и возвращения к информации о жителе
    public void CloseInventoryAndReturnToVillagerInfo()
    {
        // Удаляем все кнопки инвентаря перед его скрытием
        foreach (Transform child in inventoryUI.inventoryPanel)
        {
            Destroy(child.gameObject);  // Удаляем кнопки книг
        }

        inventoryUI.gameObject.SetActive(false);  // Скрываем инвентарь
        villagerInfoPanel.SetActive(true);  // Показываем панель с информацией о жителе
        if (currentVillager != null)
        {
            // Обновляем слайдеры для текущего жителя
            strengthSlider.value = currentVillager.skills.strength;
            enduranceSlider.value = currentVillager.skills.endurance;
            technicalSkillsSlider.value = currentVillager.skills.technicalSkills;
        }
    }

    // Обновляем позицию HighlightIndicator в каждом кадре
    private void Update()
    {
        // Проверка на клик по объектам в сцене
        if (Input.GetMouseButtonDown(0)) // Левая кнопка мыши
        {
            // Проверяем, был ли клик на UI
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Если клик был на UI, не скрываем панель
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Если кликнули на объект не типа Villager, скрываем панель
                if (hit.collider.GetComponent<Villager>() == null)
                {
                    CloseVillagerInfo();
                }
                else
                {
                    OpenVillagerInfo(hit.collider.GetComponent<Villager>());
                }
            }
        }
    }

    // Инициализация компонента villageManager при старте
    private void Start()
    {
        villagerInfoPanel.SetActive(false); // Скрываем панель с информацией при старте игры
        inventoryUI.gameObject.SetActive(false); // Скрываем инвентарь при старте игры

        // Получаем ссылку на VillageManager
        villageManager = FindObjectOfType<VillageManager>();
        if (villageManager == null)
        {
            Debug.LogError("❌ VillageManager не найден!");
        }
    }
}
