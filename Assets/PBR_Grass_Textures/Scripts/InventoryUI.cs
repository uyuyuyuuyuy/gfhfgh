using UnityEngine;
using UnityEngine.UI;
using TMPro; // Для использования TextMeshPro
using UnityEngine.EventSystems; // Для проверки кликов

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI resourceText;  // Для отображения ресурсов

    public GameObject bookButtonPrefab; // Префаб кнопки для книги
    public Transform inventoryPanel;    // Панель для отображения книг

    private Inventory inventory;        // Инвентарь
    private Villager selectedVillager;  // Выбранный житель для использования книг

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();  // Получаем инвентарь из сцены
    }

    private void Update()
    {
        if (inventory != null && resourceText != null)
        {
            int amount = inventory.GetResourceAmount("Металлолом");
            resourceText.text = $"Металлолом: {amount}";
        }

        // Проверка на клик вне панели инвентаря
        if (Input.GetMouseButtonDown(0)) // Левая кнопка мыши
        {
            if (!EventSystem.current.IsPointerOverGameObject() || !inventoryPanel.gameObject.activeSelf)
            {
                HideInventory(); // Скрываем инвентарь, если клик был вне панели
            }
        }
    }

    // Метод для отображения книг в инвентаре
    public void DisplayInventory(Book[] books, Villager villager)
    {
        selectedVillager = villager; // Устанавливаем выбранного жителя

        // Очищаем панель инвентаря перед добавлением новых элементов
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        Vector3 offset = new Vector3(-300, 0, 0);
        // Показываем панель инвентаря
        ShowInventory();

        // Создаем кнопки для каждой книги
        foreach (Book book in books)
        {
            // Создаем кнопку для книги
            GameObject bookButton = Instantiate(bookButtonPrefab, inventoryPanel.position + offset, Quaternion.identity);
            offset.x += 300f;
            bookButton.transform.SetParent(inventoryPanel);
            Debug.Log(book.bookImage);
            bookButton.GetComponent<Image>().sprite = book.bookImage; // Устанавливаем изображение книги

            // Добавляем обработчик для кнопки
            Button button = bookButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners(); // Убираем старые слушатели
                button.onClick.AddListener(() => EquipBook(book)); // Добавляем слушатель для этой книги
                Debug.Log($"Слушатель добавлен для книги: {book.bookType}");
            }
            else
            {
                Debug.LogError("Не найден компонент Button на кнопке!");
            }
        }
    }

    // Метод для использования книги
    private void EquipBook(Book book)
    {
        if (selectedVillager != null)
        {
            // Логирование, чтобы убедиться, что метод вызывается
            Debug.Log($"Нажата книга: {book.bookType}. Применяем бафф к жителю: {selectedVillager.name}");

            // Применяем книгу к выбранному жителю
            selectedVillager.EquipBook(book);

            // Логирование, чтобы проверить, что навыки изменяются
            Debug.Log($"Навыки после применения книги: Сила = {selectedVillager.skills.strength}, Выносливость = {selectedVillager.skills.endurance}, Технические навыки = {selectedVillager.skills.technicalSkills}");
            HideInventory();
        }
        else
        {
            Debug.LogWarning("Не выбран житель для применения книги!");
        }
    }

    // Метод для отображения панели инвентаря
    public void ShowInventory()
    {
        inventoryPanel.gameObject.SetActive(true); // Делаем панель инвентаря видимой
    }

    // Метод для скрытия панели инвентаря
    public void HideInventory()
    {
        inventoryPanel.gameObject.SetActive(false); // Скрываем панель инвентаря
    }
}
