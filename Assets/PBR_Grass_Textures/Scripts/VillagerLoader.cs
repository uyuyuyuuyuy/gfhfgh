using UnityEngine;
using UnityEngine.SceneManagement;

public class VillagerLoader : MonoBehaviour
{
    public GameObject villagerPrefab; // Префаб для создания жителя

    private void Start()
    {
        // Загружаем жителей только на первой сцене
        if (SceneManager.GetActiveScene().name == "WorkingProject")
        {
            Debug.Log("Загружаем жителей на первую сцену");

            // Здесь создаем жителей на заранее определенных позициях
            CreateVillagers();
        }
    }

    // Метод для создания жителей
    private void CreateVillagers()
    {
        // Пример заранее заданных позиций для жителей
        Vector3[] villagerPositions = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(5, 0, 5),
            new Vector3(-5, 0, -5)
        };

        foreach (var position in villagerPositions)
        {
            // Создаем жителя на текущей позиции
            GameObject villager = Instantiate(villagerPrefab, position, Quaternion.identity);
            Debug.Log($"Создан юнит в {position}");

            // Включаем управление кликами для каждого жителя
            villager.AddComponent<VillagerControl>();
        }
    }
}
