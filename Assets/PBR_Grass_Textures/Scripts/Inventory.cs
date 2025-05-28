using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    private static Inventory _instance; // ��� �������� ��������� ���������
    private Dictionary<string, int> resources = new Dictionary<string, int>();
    public TextMeshProUGUI scrap;
    // ����� ��� �������� ��� ��������� ���������� ���������
    public static Inventory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Inventory>(); // ������� ��������� � �����
                if (_instance == null)
                {
                    GameObject inventoryObject = new GameObject("Inventory");
                    _instance = inventoryObject.AddComponent<Inventory>(); // ������� ����� ���������, ���� ��� ���
                }
               DontDestroyOnLoad(_instance.gameObject); // ��������� ��������� ����� �������
            }
            return _instance;
        }
    }

    // ����� ��� ���������� ��������
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
        scrap.text = "����������: " + resources[resourceName];
        // ������� ������� � ������� (��� ��������)
        Debug.Log($"{resourceName} ��������. ������� ����������: {resources[resourceName]}");
    }

    // ����� ��� ��������� ���������� �������
    public int GetResourceAmount(string resourceName)
    {
        if (resources.ContainsKey(resourceName))
        {
            return resources[resourceName];
        }
        return 0;
    }
}
