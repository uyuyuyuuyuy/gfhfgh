using UnityEngine;

public class VillagerSkills
{
    public int strength; // ���� ��� �����
    public int endurance; // ������������ ��� ������������ �������
    public int technicalSkills; // ����������� ������ ��� ����������

    private const int MaxSkillValue = 10; // ������������ ������� �������

    // ����������� ��� ��������� ��������� �������� �������
    public VillagerSkills()
    {
        strength = Random.Range(0, MaxSkillValue + 1); // ���� �� 0 �� MaxSkillValue
        endurance = Random.Range(0, MaxSkillValue + 1); // ������������ �� 0 �� MaxSkillValue
        technicalSkills = Random.Range(0, MaxSkillValue + 1); // ����������� ������ �� 0 �� MaxSkillValue
    }

    // ����� ��� ������ ���������� � ������� ������� � �������
    public void PrintSkills()
    {
        Debug.Log($"����: {strength}, ������������: {endurance}, ����������� ������: {technicalSkills}");
    }

    // ���������� ������� � ����������� �� ���� ������
    public void IncreaseSkill(Building building, Villager villager)
    {
        bool updated = false; // ���� ��� �������� ���������� �������

        switch (building.GetBuildingType())
        {
            case Building.BuildingType.Farm:
                updated = IncreaseStrength(villager);
                break;
            case Building.BuildingType.WaterStation:
                updated = IncreaseEndurance(villager);
                break;
            case Building.BuildingType.Generator:
                updated = IncreaseTechSkills(villager);
                break;
        }
    }

    // ���������� ����
    private bool IncreaseStrength(Villager villager)
    {
        if (strength < MaxSkillValue)
        {
            strength++;
            Debug.Log($"���� ��������� �� {strength}");
            return true;
        }
        return false;
    }

    // ���������� ������������
    private bool IncreaseEndurance(Villager villager)
    {
        if (endurance < MaxSkillValue)
        {
            endurance++;
            Debug.Log($"������������ ��������� �� {endurance}");
            return true;
        }
        return false;
    }

    // ���������� ����������� �������
    private bool IncreaseTechSkills(Villager villager)
    {
        if (technicalSkills < MaxSkillValue)
        {
            technicalSkills++;
            Debug.Log($"����������� ������ ��������� �� {technicalSkills}");
            return true;
        }
        return false;
    }
}
