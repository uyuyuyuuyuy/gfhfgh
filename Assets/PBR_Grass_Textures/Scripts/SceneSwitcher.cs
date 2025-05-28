using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance;

    [Header("Cameras")]
    public Camera mainCamera;
    public Camera secondCamera;

    [Header("Canvas")]
    public Canvas mainCanvas;
    public Canvas scoutingCanvas;

    [Header("Player Settings")]
    public GameObject playerObject;
    private NewPlayerMovement newPlayerMovementComponent;

    [Header("Scouting Object Spawn")]
    public GameObject prefabToSpawn; // Префаб объекта для создания
    public GameObject planeForSpawn; // Плэйн, на котором рандомим позицию
    private GameObject spawnedObject; // Ссылка на созданный объект

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (mainCamera != null) mainCamera.enabled = true;
        if (secondCamera != null) secondCamera.enabled = false;

        if (mainCanvas != null) mainCanvas.enabled = true;
        if (scoutingCanvas != null) scoutingCanvas.enabled = false;
    }

    public void SwitchToScoutingMode()
    {
        if (mainCamera != null) mainCamera.enabled = false;
        if (secondCamera != null)
        {
            secondCamera.enabled = true;
            // Можно фиксировать позицию и поворот, если нужно
        }

        if (mainCanvas != null) mainCanvas.enabled = false;
        if (scoutingCanvas != null) scoutingCanvas.enabled = true;

        if (playerObject != null)
        {
            newPlayerMovementComponent = playerObject.GetComponent<NewPlayerMovement>();
            if (newPlayerMovementComponent == null)
            {
                newPlayerMovementComponent = playerObject.AddComponent<NewPlayerMovement>();
            }
        }

        SpawnObjectOnPlane();
    }

    public void SwitchToMainMode()
    {
        if (mainCamera != null) mainCamera.enabled = true;
        if (secondCamera != null) secondCamera.enabled = false;

        if (mainCanvas != null) mainCanvas.enabled = true;
        if (scoutingCanvas != null) scoutingCanvas.enabled = false;

        if (playerObject != null && newPlayerMovementComponent != null)
        {
            Destroy(newPlayerMovementComponent);
            newPlayerMovementComponent = null;
        }

        // Удаляем созданный объект при выходе из разведки
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            spawnedObject = null;
        }
    }

    private void SpawnObjectOnPlane()
    {
        if (prefabToSpawn == null || planeForSpawn == null)
        {
            Debug.LogWarning("Prefab or Plane for spawning is not assigned!");
            return;
        }

        // Получаем размеры плейна (предполагается, что у плейна есть Collider)
        Collider planeCollider = planeForSpawn.GetComponent<Collider>();
        if (planeCollider == null)
        {
            Debug.LogWarning("Plane object must have a Collider component");
            return;
        }

        Bounds bounds = planeCollider.bounds;

        // Генерируем случайные X и Z внутри границ плейна
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);
        float fixedY = 0.25f;

        Vector3 spawnPosition = new Vector3(randomX, fixedY, randomZ);

        // Создаём объект
        spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }
}
