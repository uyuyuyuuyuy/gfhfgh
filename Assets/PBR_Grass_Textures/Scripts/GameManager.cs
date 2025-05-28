using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Данные для сохранения состояния
    public float savedResources = 100f;
    public string[] savedBuildings = { "Building1", "Building2" };
    public string[] savedVillagers = { "Villager1", "Villager2" };

    // Объект, который не будет уничтожен при переходах
    void Awake()
    {
        // Если объект уже существует, уничтожаем его
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняем объект между сценами
        }
        else
        {
            Destroy(gameObject); // Уничтожаем дубликат
        }
    }

    // Метод для сохранения состояния
    public void SaveState(float resources, string[] buildings, string[] villagers)
    {
        savedResources = resources;
        savedBuildings = buildings;
        savedVillagers = villagers;
    }

    // Метод для восстановления состояния
    public void RestoreState()
    {
        Debug.Log("Restored Resources: " + savedResources);
        Debug.Log("Restored Buildings: " + string.Join(", ", savedBuildings));
        Debug.Log("Restored Villagers: " + string.Join(", ", savedVillagers));
    }
}
