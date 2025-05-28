using UnityEngine;

public class VillagerSpawner : MonoBehaviour
{
    public GameObject villagerPrefab; // Префаб жителя
    public int maxVillagers = 10; // Лимит жителей
    private int currentVillagers = 0; // Текущее количество

    public void SpawnVillager()
    {
        // Проверка на максимальное количество жителей
        if (currentVillagers < maxVillagers)
        {
            // Генерируем случайные координаты (например, в диапазоне -5 до 5)
            Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

            // Создаём жителя
            GameObject villagerObj = Instantiate(villagerPrefab, randomPosition, Quaternion.identity);

            // Проверяем, есть ли компонент Villager на новом объекте
            Villager villager = villagerObj.GetComponent<Villager>();
            if (villager == null)
            {
                // Если компонента нет, добавляем его
                villager = villagerObj.AddComponent<Villager>();
            }

            // Добавляем ему AI-движение
            VillagerAI ai = villagerObj.GetComponent<VillagerAI>();
            if (ai == null)
            {
                ai = villagerObj.AddComponent<VillagerAI>();
            }

            // Добавляем жителя в VillageManager
            VillageManager villageManager = FindObjectOfType<VillageManager>();
            if (villageManager != null)
            {
                villageManager.AddVillager(villager);
            }
            else
            {
                Debug.LogError("❌ VillageManager не найден!");
            }

            currentVillagers++;
            Debug.Log($"✅ Создан житель {currentVillagers}/{maxVillagers}");
        }
        else
        {
            Debug.Log("⚠️ Достигнут лимит жителей");
        }
    }
}
