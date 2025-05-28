using UnityEngine;
using System.Collections.Generic;
using TMPro;  // Если используешь TMP для вывода информации на экран

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private Dictionary<string, int> resources = new Dictionary<string, int>();

    [Header("UI Elements")]
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI energyText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddResource(string resourceName, int amount)
    {
        if (resources.ContainsKey(resourceName))
        {
            resources[resourceName] += amount;
        }
        else
        {
            resources[resourceName] = amount;
        }

        Debug.Log($"Добавлено {amount} {resourceName}. Всего: {resources[resourceName]}");
        UpdateResourceUI();

        Debug.Log($"После добавления ресурсов: Еда={GetResourceAmount("Еда")}, Вода={GetResourceAmount("Вода")}, Энергия={GetResourceAmount("Энергия")}");
        CheckGameOver();
    }

    public bool UseResource(string resourceName, int amount)
    {
        if (resources.ContainsKey(resourceName) && resources[resourceName] >= amount)
        {
            resources[resourceName] -= amount;
            Debug.Log($"Потрачено {amount} {resourceName}. Осталось: {resources[resourceName]}");
            UpdateResourceUI();

            Debug.Log($"После использования ресурсов: Еда={GetResourceAmount("Еда")}, Вода={GetResourceAmount("Вода")}, Энергия={GetResourceAmount("Энергия")}");
            CheckGameOver();
            return true;
        }
        else
        {
            Debug.Log($"Недостаточно {resourceName}!");
            return false;
        }
    }

    public int GetResourceAmount(string resourceName)
    {
        return resources.ContainsKey(resourceName) ? resources[resourceName] : 0;
    }

    private void UpdateResourceUI()
    {
        if (foodText != null)
            foodText.text = $"🍞 Еда: {(resources.ContainsKey("Еда") ? resources["Еда"] : 0)}";
        if (waterText != null)
            waterText.text = $"💧 Вода: {(resources.ContainsKey("Вода") ? resources["Вода"] : 0)}";
        if (energyText != null)
            energyText.text = $"⚡ Энергия: {(resources.ContainsKey("Энергия") ? resources["Энергия"] : 0)}";
    }

    private void CheckGameOver()
    {
        int foodAmount = GetResourceAmount("Еда");
        int waterAmount = GetResourceAmount("Вода");
        int energyAmount = GetResourceAmount("Энергия");

        Debug.Log($"Проверка ресурсов: Еда={foodAmount}, Вода={waterAmount}, Энергия={energyAmount}");

        if (foodAmount <= 0 || waterAmount <= 0 || energyAmount <= 0)
        {
            Debug.Log("Игра окончена! Один из ресурсов исчерпан.");
            GameOver();
        }
    }


    private void GameOver()
    {
        Debug.Log("GameOver вызван!");
        Application.Quit();

#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
