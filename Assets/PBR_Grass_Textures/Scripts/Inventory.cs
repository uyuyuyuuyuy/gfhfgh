using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    private static Inventory _instance; // Для создания синглтона инвентаря
    private Dictionary<string, int> resources = new Dictionary<string, int>();
    public TextMeshProUGUI scrap;
    // Метод для создания или получения экземпляра инвентаря
    public static Inventory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Inventory>(); // Находим инвентарь в сцене
                if (_instance == null)
                {
                    GameObject inventoryObject = new GameObject("Inventory");
                    _instance = inventoryObject.AddComponent<Inventory>(); // Создаем новый инвентарь, если его нет
                }
               DontDestroyOnLoad(_instance.gameObject); // Сохраняем инвентарь между сценами
            }
            return _instance;
        }
    }

    // Метод для добавления ресурсов
    public void AddResource(string resourceName, int amount)
    {
        if (resources.ContainsKey(resourceName))
        { 
            
            resources[resourceName] += amount;
        
        }
        else
        {
            resources.Add(resourceName, amount);
        }
        scrap.text = "Металлолом: " + resources[resourceName];
        // Выводим ресурсы в консоль (для проверки)
        Debug.Log($"{resourceName} добавлен. Текущее количество: {resources[resourceName]}");
    }

    // Метод для получения количества ресурса
    public int GetResourceAmount(string resourceName)
    {
        if (resources.ContainsKey(resourceName))
        {
            return resources[resourceName];
        }
        return 0;
    }
}
