using UnityEngine;
using UnityEngine.AI;

public class BuildingPlacer : MonoBehaviour
{
    public GameObject buildingPrefab;
    public Vector2 buildAreaMin = new Vector2(-8, -8);
    public Vector2 buildAreaMax = new Vector2(8, 8);
    public float gridSize = 1f;
    public LayerMask buildingLayer;
    public int buildingCost = 2; // Стоимость постройки в металлоломе

    private bool isBuildingMode = false;
    private GameObject previewBuilding;
    private NavMeshAgent playerAgent;
    private Inventory inventory; // Ссылка на инвентарь

    void Start()
    {
        playerAgent = FindObjectOfType<NavMeshAgent>();
        inventory = FindObjectOfType<Inventory>(); // Находим инвентарь
    }

    void Update()
    {
        if (isBuildingMode)
        {
            Vector3 position = GetMouseWorldPosition();
            position = SnapToGrid(position);

            if (position.x < buildAreaMin.x || position.x > buildAreaMax.x ||
                position.z < buildAreaMin.y || position.z > buildAreaMax.y)
            {
                return;
            }

            if (previewBuilding == null)
            {
                previewBuilding = Instantiate(buildingPrefab, position, Quaternion.identity);
                previewBuilding.GetComponent<Collider>().enabled = false;
            }
            else
            {
                previewBuilding.transform.position = position;
            }

            if (IsOverlapping(position))
            {
                previewBuilding.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                previewBuilding.GetComponent<Renderer>().material.color = Color.green;
            }

            // **ЛКМ - Строим здание**
            if (Input.GetMouseButtonDown(0) && !IsOverlapping(position))
            {
                if (inventory.GetResourceAmount("Металлолом") >= buildingCost)
                {
                    inventory.AddResource("Металлолом", -buildingCost); // Вычитаем 2 металлолома
                    Instantiate(buildingPrefab, position, Quaternion.identity);
                    ExitBuildMode();
                }
                else
                {
                    Debug.Log("Не хватает металлолома!");
                }
            }

            // **ПКМ - Отмена строительства**
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Строительство отменено.");
                ExitBuildMode();
            }
        }
    }

    public void StartBuilding()
    {
        isBuildingMode = true;
        if (playerAgent != null)
        {
            playerAgent.isStopped = true;
        }
    }

    private void ExitBuildMode()
    {
        isBuildingMode = false;
        if (playerAgent != null)
        {
            playerAgent.isStopped = false;
        }
        if (previewBuilding != null)
        {
            Destroy(previewBuilding);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        position.x = Mathf.Round(position.x / gridSize) * gridSize;
        position.z = Mathf.Round(position.z / gridSize) * gridSize;
        return position;
    }

    private bool IsOverlapping(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, buildingLayer);
        return colliders.Length > 0;
    }
}
