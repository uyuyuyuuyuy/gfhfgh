using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Resource"))
        {
            Debug.Log("Ресурс собран!");

            // Используем инвентарь через синглтон
            Inventory.Instance.AddResource("Металлолом", 10);  // Здесь добавляем 10 единиц металлолома

            // Уничтожаем ресурс (куб)
            Destroy(other.gameObject);
        }
    }
}
