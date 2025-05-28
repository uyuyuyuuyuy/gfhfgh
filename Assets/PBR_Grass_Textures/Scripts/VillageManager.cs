using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class VillageManager : MonoBehaviour
{
    public float food = 10f;
    public float water = 10f;
    public float energy = 10f;

    private List<Villager> allVillagers = new List<Villager>();
    private List<Villager> freeVillagers = new List<Villager>();

    [Header("UI Elements")]
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI energyText;

    [Header("Resource Consumption")]
    public float foodConsumptionPerVillager = 1f;
    public float waterConsumptionPerVillager = 1f;
    public float energyConsumptionPerVillager = 0f;
    public float resourceConsumptionInterval = 6f;

    [Header("Books")]
    public Book[] availableBooks;  // Массив доступных книг
    public Sprite strenghtBook;
    public Sprite enduranceBook;
    public Sprite technicalBook;

    private void Start()
    {
        allVillagers.AddRange(FindObjectsOfType<Villager>());
        UpdateFreeVillagers();
        UpdateResourceUI();
        InvokeRepeating(nameof(ConsumeResources), resourceConsumptionInterval, resourceConsumptionInterval);
        StartCoroutine(RandomVillagerDeath());
        StartCoroutine(ProgressVillagersSkills());

        InitializeBooks();
    }

    private void InitializeBooks()
    {
        availableBooks = new Book[3];

        availableBooks[0] = new Book
        {
            bookType = Book.BookType.Strength,
            bookImage = strenghtBook,
            buffAmount = 1
        };

        availableBooks[1] = new Book
        {
            bookType = Book.BookType.Endurance,
            bookImage = enduranceBook,
            buffAmount = 2
        };

        availableBooks[2] = new Book
        {
            bookType = Book.BookType.Technical,
            bookImage = technicalBook,
            buffAmount = 3
        };
    }

    public Book[] GetAvailableBooks()
    {
        return availableBooks;
    }

    private IEnumerator ProgressVillagersSkills()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            foreach (var villager in allVillagers)
            {
                if (villager.IsAssigned)
                {
                    villager.skills.IncreaseSkill(villager.AssignedBuilding, villager);
                }
            }
        }
    }

    public List<Villager> GetFreeVillagers()
    {
        return freeVillagers;
    }

    public void AddVillager(Villager villager)
    {
        if (!allVillagers.Contains(villager))
        {
            allVillagers.Add(villager);
            UpdateFreeVillagers();
            Debug.Log($"➕ Добавлен житель: {villager.name}. Всего: {allVillagers.Count}, свободных: {freeVillagers.Count}");
        }
    }

    public void RemoveVillager(Villager villager)
    {
        if (allVillagers.Contains(villager))
        {
            allVillagers.Remove(villager);
            UpdateFreeVillagers();
        }
    }

    public int GetVillagerCount()
    {
        return allVillagers.Count;
    }

    public Villager GetAvailableVillager()
    {
        foreach (var villager in freeVillagers)
        {
            if (!villager.IsAssigned)
            {
                freeVillagers.Remove(villager);
                return villager;
            }
        }

        Debug.Log("⚠️ Нет свободных жителей!");
        return null;
    }

    public void ReleaseVillager(Villager villager)
    {
        if (villager != null && allVillagers.Contains(villager))
        {
            if (villager.IsAssigned)
            {
                villager.SetAssignedBuilding(null);
                villager.gameObject.SetActive(true);
            }

            if (!freeVillagers.Contains(villager))
            {
                freeVillagers.Add(villager);
            }

            Debug.Log($"✅ Житель {villager.name} теперь свободен.");
        }
    }

    public void AssignVillager(Villager villager)
    {
        if (freeVillagers.Contains(villager))
        {
            freeVillagers.Remove(villager);
        }
    }

    public void UpdateFreeVillagers()
    {
        freeVillagers.Clear();
        foreach (Villager villager in allVillagers)
        {
            if (!villager.IsAssigned)
            {
                freeVillagers.Add(villager);
            }
        }

        Debug.Log($"🔄 Обновление списка свободных жителей: {freeVillagers.Count}");
    }

    public List<Villager> FreeVillagers => freeVillagers;

    public void UseResources(float foodAmount, float waterAmount, float energyAmount)
    {
        food = Mathf.Max(0, food - foodAmount);
        water = Mathf.Max(0, water - waterAmount);
        energy = Mathf.Max(0, energy - energyAmount);
        UpdateResourceUI();
        if (food <= 0 || water <= 0 || energy <= 0)
        {
            GameOver();
        }
    }

    public void AddResources(float foodAmount, float waterAmount, float energyAmount)
    {
        food += foodAmount;
        water += waterAmount;
        energy += energyAmount;
        UpdateResourceUI();
        if (food <= 0 || water <= 0 || energy <= 0)
        {
            GameOver();
        }
    }

    private void UpdateResourceUI()
    {
        if (foodText != null) foodText.text = $"🍞 Еда: {food:F1}";
        if (waterText != null) waterText.text = $"💧 Вода: {water:F1}";
        if (energyText != null) energyText.text = $"⚡ Энергия: {energy:F1}";
    }

    private void ConsumeResources()
    {
        int activeVillagerCount = allVillagers.Count;

        if (allVillagers.Count > 0)
        {
            activeVillagerCount -= 1;
        }

        float totalFoodConsumption = activeVillagerCount * foodConsumptionPerVillager;
        float totalWaterConsumption = activeVillagerCount * waterConsumptionPerVillager;
        float totalEnergyConsumption = activeVillagerCount * energyConsumptionPerVillager;

        UseResources(totalFoodConsumption, totalWaterConsumption, totalEnergyConsumption);

        Debug.Log($"⚠️ Потреблены ресурсы: -{totalFoodConsumption} 🍞 -{totalWaterConsumption} 💧 -{totalEnergyConsumption} ⚡");

        if (food <= 0 || water <= 0 || energy <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Игра окончена! Один из ресурсов исчерпан.");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // чтобы остановить игру в редакторе
#else
        Application.Quit(); // чтобы закрыть билд
#endif
    }


    private IEnumerator RandomVillagerDeath()
    {
        while (true)
        {
            float waitTime = Random.Range(60f, 180f);
            yield return new WaitForSeconds(waitTime);

            if (allVillagers.Count > 0)
            {
                Villager villagerToDie = allVillagers[Random.Range(0, allVillagers.Count)];
                RemoveVillager(villagerToDie);
                Destroy(villagerToDie.gameObject);

                Debug.Log($"💀 Житель {villagerToDie.name} умер! Осталось жителей: {allVillagers.Count}");
            }
        }
    }
}
