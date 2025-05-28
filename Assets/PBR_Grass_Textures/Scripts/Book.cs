using UnityEngine;

public class Book : MonoBehaviour
{
    public enum BookType
    {
        Strength,     // ����� ����
        Endurance,    // ����� ������������
        Technical     // ����� ����������� �������
    }

    public BookType bookType; // ��� �����
    public Sprite bookImage;  // ����������� ����� (��������, ������� ����� ������������ � ���������)
    public int buffAmount;    // ������� ����������� �����

    // ����� ��� ������������� �����
    public void Use(Villager villager)
    {
        switch (bookType)
        {
            case BookType.Strength:
                villager.skills.strength += buffAmount;
                break;
            case BookType.Endurance:
                villager.skills.endurance += buffAmount;
                break;
            case BookType.Technical:
                villager.skills.technicalSkills += buffAmount;
                break;
        }
        villager.UpdateUI();  // ��������� UI ����� ������������� �����
    }
}
