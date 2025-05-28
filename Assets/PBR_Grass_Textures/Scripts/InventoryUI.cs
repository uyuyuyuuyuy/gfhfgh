using UnityEngine;
using UnityEngine.UI;
using TMPro; // ��� ������������� TextMeshPro
using UnityEngine.EventSystems; // ��� �������� ������

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI resourceText;  // ��� ����������� ��������

    public GameObject bookButtonPrefab; // ������ ������ ��� �����
    public Transform inventoryPanel;    // ������ ��� ����������� ����

    private Inventory inventory;        // ���������
    private Villager selectedVillager;  // ��������� ������ ��� ������������� ����

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();  // �������� ��������� �� �����
    }

    private void Update()
    {
        if (inventory != null && resourceText != null)
        {
            int amount = inventory.GetResourceAmount("����������");
            resourceText.text = $"����������: {amount}";
        }

        // �������� �� ���� ��� ������ ���������
        if (Input.GetMouseButtonDown(0)) // ����� ������ ����
        {
            if (!EventSystem.current.IsPointerOverGameObject() || !inventoryPanel.gameObject.activeSelf)
            {
                HideInventory(); // �������� ���������, ���� ���� ��� ��� ������
            }
        }
    }

    // ����� ��� ����������� ���� � ���������
    public void DisplayInventory(Book[] books, Villager villager)
    {
        selectedVillager = villager; // ������������� ���������� ������

        // ������� ������ ��������� ����� ����������� ����� ���������
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        Vector3 offset = new Vector3(-300, 0, 0);
        // ���������� ������ ���������
        ShowInventory();

        // ������� ������ ��� ������ �����
        foreach (Book book in books)
        {
            // ������� ������ ��� �����
            GameObject bookButton = Instantiate(bookButtonPrefab, inventoryPanel.position + offset, Quaternion.identity);
            offset.x += 300f;
            bookButton.transform.SetParent(inventoryPanel);
            Debug.Log(book.bookImage);
            bookButton.GetComponent<Image>().sprite = book.bookImage; // ������������� ����������� �����

            // ��������� ���������� ��� ������
            Button button = bookButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners(); // ������� ������ ���������
                button.onClick.AddListener(() => EquipBook(book)); // ��������� ��������� ��� ���� �����
                Debug.Log($"��������� �������� ��� �����: {book.bookType}");
            }
            else
            {
                Debug.LogError("�� ������ ��������� Button �� ������!");
            }
        }
    }

    // ����� ��� ������������� �����
    private void EquipBook(Book book)
    {
        if (selectedVillager != null)
        {
            // �����������, ����� ���������, ��� ����� ����������
            Debug.Log($"������ �����: {book.bookType}. ��������� ���� � ������: {selectedVillager.name}");

            // ��������� ����� � ���������� ������
            selectedVillager.EquipBook(book);

            // �����������, ����� ���������, ��� ������ ����������
            Debug.Log($"������ ����� ���������� �����: ���� = {selectedVillager.skills.strength}, ������������ = {selectedVillager.skills.endurance}, ����������� ������ = {selectedVillager.skills.technicalSkills}");
            HideInventory();
        }
        else
        {
            Debug.LogWarning("�� ������ ������ ��� ���������� �����!");
        }
    }

    // ����� ��� ����������� ������ ���������
    public void ShowInventory()
    {
        inventoryPanel.gameObject.SetActive(true); // ������ ������ ��������� �������
    }

    // ����� ��� ������� ������ ���������
    public void HideInventory()
    {
        inventoryPanel.gameObject.SetActive(false); // �������� ������ ���������
    }
}
